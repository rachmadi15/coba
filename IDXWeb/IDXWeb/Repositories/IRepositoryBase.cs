namespace IDXWeb.Repositories
{
    public interface IRepositoryBase<T>
    {
        void Save(T entity);
        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void Update(T entity);
        T GetByUmbracoId(string itemId);
        void RemoveByUmbracoId(string itemId);
    }
}
