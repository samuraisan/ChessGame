namespace Utility
{
    public abstract class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance => GetInstance();

        protected Singleton()
        {
            
        }

        public static T GetInstance()
        {
            return _instance ??= System.Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}