using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for entity operations
/// </summary>
public interface IRepository<T> where T: BaseEntity
{
    /// <summary>
    /// Creates a new entity in the repository
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created entity</returns>
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entityCollection in the repository
    /// </summary>
    /// <param name="entityCollection">The collection to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created entity</returns>
    Task<ICollection<T>> CreateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a entity by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a entity from the database
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity was updated, false if not found</returns>
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a entity from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<T>> GetPaginatedList(string? searchText, string? columnOrder, bool asc, int pageNumber, int pageSize, CancellationToken cancellationToken);
    EntityChanged GetChanges(T entity, EOperationPropertieChanged eOperationPropertieChanged);
    ICollection<EntityChanged> GetChangesList(ICollection<T> updateEntityList, ICollection<T> addEntityList, ICollection<T> removeEntityList);
    Task<IList<T>> GetAllAsync();
}