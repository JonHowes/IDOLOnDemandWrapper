using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace IODWrapper
{

	public class IODJobBatchRequest
	{

		private IODJobBatch _batch = new IODJobBatch();
		private Dictionary<string, string> _files = new Dictionary<string, string>();

		
		public IODJobBatchRequest()
		{
			TimeoutSpan = new TimeSpan(0, 2, 0);
		}

		
		public string ApiKey { get; set; }

		
		public TimeSpan TimeoutSpan { get; set; }

		
		public void AddFile(string key, string filePath)
		{
			_files.Add(key, filePath);
		}


		public void AddRequest(IODRequest req)
		{
			_batch.Requests.Add(req);
		}
	

		public void MakeRequest()
		{

			var client = new RestClient(@"https://api.idolondemand.com");

			string res = ExecuteJobRequest(client);
		
			JObject resParsed = ParseResponse(res);
			
			string jobId = (string)resParsed["jobID"];
			
			res = ExecuteResponseRequest(client, jobId);
			
			resParsed = ParseResponse(res);

			ParseResults(res);
		
		}


		private string ExecuteJobRequest(RestClient client)
		{

			var req = new RestRequest(@"1/job", Method.POST);
			req.AlwaysMultipartFormData = true;

			req.AddParameter("apikey", ApiKey);
			
			req.AddParameter("job", JsonConvert.SerializeObject(_batch));
			
			foreach (var v in _files)
			{
				req.AddFile(v.Key, v.Value);
			}

			string resp = null;
			client.ExecuteAsync(req, response => { resp = response.Content; });
			
			DateTime timeoutTime = DateTime.Now + TimeoutSpan;
			while (string.IsNullOrEmpty(resp) && DateTime.Now < timeoutTime)
			{
				System.Threading.Thread.Sleep(1000);
			}

			if (resp == null)
			{
				throw new TimeoutException("Initial job request timed out");
			}

			return resp;
		
		}


		private JObject ParseResponse(string res)
		{

			if (res == null)
			{
				throw new Exception("IDOL OnDemand returned null");
			}

			if (res.TrimStart().StartsWith("<html>"))
			{
				throw new Exception(string.Format("IDOL OnDemand returned non-Json: {0}", res));
			}

			JObject parsedRes = JObject.Parse(res);

			string message = (string)parsedRes["message"];
			if (!string.IsNullOrEmpty(message))
			{
				throw new Exception(string.Format("IDOL OnDemand returned a message: {0}", message));
			}

			string error = (string)parsedRes["error"];
			if (!string.IsNullOrEmpty(error))
			{
				string reason = (string)parsedRes["reason"];
				object o = parsedRes["detail"];
				if (o != null)
				{
					string detail = o.ToString();
					reason = reason + "\r\n" + detail;
				}
				throw new Exception(string.Format("IDOL OnDemand returned an error {0}, {1}", error, reason));
			}

			return parsedRes;

		}


		private string ExecuteResponseRequest(RestClient client, string jobId)
		{

			if (string.IsNullOrEmpty(jobId))
			{
				throw new Exception("No JobID was returned from IDOL OnDemand");
			}

			var req = new RestRequest(@"1/job/result/{jobId}", Method.POST);
			req.AddUrlSegment("jobId", jobId);
			req.AddParameter("apikey", ApiKey);

			RestResponse rr = (RestResponse)client.Execute(req);
			return rr.Content;

		}


		private void ParseResults(string res)
		{

			JobBatchResponse rs = JsonConvert.DeserializeObject<JobBatchResponse>(res);
			if (rs == null)
			{
				throw new Exception("Unrecognised response from IDOL OnDemand");
			}

			if (rs.actions.Count != _batch.Requests.Count) 
			{ 
				throw new Exception("Mismatched results count from IDOL OnDemand"); 
			}
			
			for (int i = 0; i < rs.actions.Count; i++)
			{
				Action a = rs.actions[i];
				IODRequest r = _batch.Requests[i];

				if (a.action != r.Action)
				{ 
					throw new Exception("Mismatched results action from IDOL OnDemand"); 
				}

				IODStatus status;
				Enum.TryParse<IODStatus>(a.status, out status);

				string result = (a.result == null) ? null : a.result.ToString();

				r.SetResult(status, result);

			}
			
		}


	}

}
