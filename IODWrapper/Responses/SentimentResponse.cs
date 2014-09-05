using System.Collections.Generic;

// Classes obtained by pasting json into this site http://json2csharp.com/

namespace IODWrapper
{

	public class Sentiment
	{
		public string sentiment { get; set; }
		public string topic { get; set; }
		public double score { get; set; }
		public string original_text { get; set; }
		public int original_length { get; set; }
		public string normalized_text { get; set; }
		public int normalized_length { get; set; }
	}

	public class SentimentAggregate
	{
		public string sentiment { get; set; }
		public double score { get; set; }
	}

	public class SentimentResponse
	{
		public List<Sentiment> positive { get; set; }
		public List<Sentiment> negative { get; set; }
		public SentimentAggregate aggregate { get; set; }
	}

}

