namespace Taki.Game.Database
{
    internal interface IDatabase<T>
    {
        bool Create(T value);
        T Read(string key, string val);
        void Update(string key, string val, T newValue);
        bool Delete(string key, string val);
        bool CloseDB();
    }
}
