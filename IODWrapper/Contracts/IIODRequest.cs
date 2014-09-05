
namespace IODWrapper
{

	public interface IIODRequest
	{
		string Action { get; }
		object Params { get; }
		void SetResult(string result);
	}

}

