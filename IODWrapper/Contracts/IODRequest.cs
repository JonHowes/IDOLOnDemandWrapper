
namespace IODWrapper
{

	abstract public class IODRequest
	{

		protected IODSource _srcType;
		protected string _src;

		abstract public string Action { get ; }

		abstract public object Params { get ; }

		abstract public void SetResult(IODStatus status, string result);

	}

}

