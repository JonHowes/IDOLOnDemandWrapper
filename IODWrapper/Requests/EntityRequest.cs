using Newtonsoft.Json;
using System;

namespace IODWrapper
{

	public class EntityRequest : IODRequest
	{

		string _entityType;
		bool _showAlternatives;

		public EntityRequest(IODSource srcType, string src, string entityType, bool showAlternatives=false)
		{
			_srcType = srcType; 
			_src = src;
			_entityType = entityType; 
			_showAlternatives = showAlternatives;
		}

		public Action<IODStatus, EntityResponse> OnResponse { get; set; }

		override public string Action
		{
			get { return "extractentity"; }
		}

		override public object Params
		{
			get 
			{
				switch (_srcType)
				{
					case IODSource.file:      
						return new { file = _src, entity_type = _entityType, show_alternatives = _showAlternatives };
					case IODSource.reference: 
						return new { reference = _src, entity_type = _entityType, show_alternatives = _showAlternatives };
					case IODSource.text:      
						return new { text = _src, entity_type = _entityType, show_alternatives = _showAlternatives };
					case IODSource.url: 
						return new { url = _src, entity_type = _entityType, show_alternatives = _showAlternatives };
				}
				return null;
			}
		}

		override public void SetResult(IODStatus status, string result)
		{
			if (OnResponse != null)
			{
				OnResponse(status,  (result == null) ? null : JsonConvert.DeserializeObject<EntityResponse>(result)  );
			}
		}

	}


}

