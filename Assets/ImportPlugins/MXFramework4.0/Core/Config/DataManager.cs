using Mx.Util;

namespace Mx.Config
{
    /// <summary>数据管理</summary>
    public class DataManager : MonoSingleton<DataManager>
    {
        private DatabaseManager databaseManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            databaseManager = new DatabaseManager();
        }

        public void Load()
        {
            databaseManager.Load();
        }

        public T GetDatabase<T>() where T : IDatabase, new()
        {
            return databaseManager.GetDatabase<T>();
        }
    }
}