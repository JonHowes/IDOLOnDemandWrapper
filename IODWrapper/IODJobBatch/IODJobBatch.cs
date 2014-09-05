using System.Collections.Generic;
using Newtonsoft.Json;

namespace IODWrapper
{

	public class IODJobBatch
	{

		private List<IODRequest> _requests;
		private List<IODJob> _actions;

		[JsonIgnore]
		public List<IODRequest> Requests
		{
			get
			{
				if (_requests == null)
				{
					_requests = new List<IODRequest>();
				}
				return _requests;
			}
		}

		public List<IODJob> actions  // Must be all lower case for Json serialisation
		{
			get 
			{
				if (_actions == null || _actions.Count != _requests.Count)
				{
					_actions = new List<IODJob>();
					foreach (IODRequest r in _requests)
					{
						_actions.Add(new IODJob() { name = r.Action, version = "v1", @params = r.Params });
					}
				}
				return _actions; 
			} 
		}

	}

}

