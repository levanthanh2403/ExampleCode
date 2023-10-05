using map.backend.shared.Entities;

namespace map.backend.shared.Interfaces.UnitOfWork
{
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets the specified repository for the <typeparamref name="T"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{T}"/> interface.</returns>
        IAsyncRepository<T> GetRepository<T>(bool hasCustomRepository = false) where T : EntityBase;
    }
}
