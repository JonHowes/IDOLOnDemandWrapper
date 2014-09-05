using System.Collections.Generic;

// Classes obtained by pasting json into this site http://json2csharp.com/

namespace IODWrapper
{

	public class Action
	{
		public object result { get; set; }
		public string status { get; set; }
		public string action { get; set; }
		public string version { get; set; }
	}

	public class JobBatchResponse
	{
		public List<Action> actions { get; set; }
		public string jobID { get; set; }
		public string status { get; set; }
	}

}
