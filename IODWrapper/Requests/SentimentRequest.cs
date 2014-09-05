using Newtonsoft.Json;
using System;

namespace IODWrapper
{

	public class SentimentRequest : IODRequest
	{

		string _language;

		public SentimentRequest(IODSource srcType, string src, string language="eng")
		{
			_srcType = srcType;
			_src = src;
			_language = language;
		}

		public Action<IODStatus,SentimentResponse> OnResponse { get; set; }

		override public string Action
		{
			get { return "analyzesentiment"; }
		}

		override public object Params
		{
			get
			{
				switch (_srcType)
				{
					case IODSource.file:
						return new { file = _src, language = _language };
					case IODSource.reference:
						return new { reference = _src, language = _language };
					case IODSource.text:
						return new { text = _src, language = _language };
					case IODSource.url:
						return new { url = _src, language = _language };
				}
				return null;
			}
		}

		override public void SetResult(IODStatus status, string result)
		{
			if (OnResponse != null)
			{
				OnResponse(status, (result == null) ? null : JsonConvert.DeserializeObject<SentimentResponse>(result));
			}
		}

	}

}

