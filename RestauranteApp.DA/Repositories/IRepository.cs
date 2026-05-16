namespace RestauranteApp.DA.Repositories
{
    /// <summary>
    /// Interfaz genérica del patrón Repositorio.
    /// Define las operaciones CRUD aplicables a cualquier entidad.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad del dominio.</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
