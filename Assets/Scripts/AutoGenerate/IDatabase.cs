
namespace Mx.Config
{
	public interface IDatabase
	{
		uint TypeID();
		string DataPath();
		void Load();
	}
}
