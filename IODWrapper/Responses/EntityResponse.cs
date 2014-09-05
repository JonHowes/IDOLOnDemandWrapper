using System.Collections.Generic;

// Classes obtained by pasting json into this site http://json2csharp.com/

namespace IODWrapper
{

	public class AdditionalInformation
	{
		public List<string> person_profession { get; set; }
		public string person_date_of_birth { get; set; }
		public int wikidata_id { get; set; }
		public string wikipedia_eng { get; set; }
		public string image { get; set; }
	}

	public class Entity
	{
		public string normalized_text { get; set; }
		public string original_text { get; set; }
		public string type { get; set; }
		public int normalized_length { get; set; }
		public int original_length { get; set; }
		public double score { get; set; }
		public AdditionalInformation additional_information { get; set; }
		public List<object> components { get; set; }
	}

	public class EntityResponse
	{
		public List<Entity> entities { get; set; }
	}

}

