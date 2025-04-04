using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace Ambev.DeveloperEvaluation.Infrastructure.Repositories;

/// <summary>
/// Implementation of IRepository using Entity Framework Core
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DefaultContext _context;
    internal DbSet<T> DbSet {
        get {
            return _context.Set<T>();
        }
    }

    /// <summary>
    /// Initializes a new instance of Repository
    /// </summary>
    /// <param name="context">The database context</param>
    public Repository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new entity in the database
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created entity</returns>
    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.SetNumber(GetLastNumber() + 1);
        await DbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Creates a new entity in the database
    /// </summary>
    /// <param name="entityCollection">The entity to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created entity</returns>
    public virtual async Task<ICollection<T>> CreateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entityCollection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entityCollection;
    }

    /// <summary>
    /// Retrieves a entity by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The entity if found, null otherwise</returns>
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetQuery().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<T?> GetAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Updates a entity from the database
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity was updated, false if not found</returns>
    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Deletes a entity from the database
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return false;

        DbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public virtual IQueryable<T> GetQuery()
    {
        return DbSet;
    }

    public async Task<IList<T>> GetAllAsync() { 
        return await GetQuery().ToListAsync();
    }

    public virtual IQueryable<T> GetQuery(string? searchText, string? colunaOrdenacao, bool ordenacaoAscendente, CancellationToken cancellationToken)
    {
        return GetQuery();
    }
    public async Task<PaginatedList<T>> GetPaginatedList(string? searchText, string? columnOrder, bool asc, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var queryable = GetQuery(searchText, columnOrder, asc, cancellationToken);

        var pagination = await PaginatedList<T>.CreateInstanceAsync(pageNumber, pageSize, queryable);

        return await Task.FromResult(pagination);
    }

    public int GetLastNumber()
    {
        var model = DbSet.OrderByDescending(s => s.Number).FirstOrDefault();
        return model?.Number ?? 0;
    }

    public EntityChanged GetChanges(T entity, EOperationPropertieChanged eOperationPropertieChanged) {
        var entityEntry = _context.Entry(entity);
        var entityChanged = new EntityChanged { Key = entity.Id, Name = typeof(T).Name, OperationPropertieChanged = eOperationPropertieChanged };

        foreach (var item in entityEntry.CurrentValues.Properties.Where(i => new List<Type> { typeof(Guid), typeof(string), typeof(int) }.Contains(i.ClrType)))
        {
            var propEnrty = entityEntry.Property(item.Name);

            if (eOperationPropertieChanged != EOperationPropertieChanged.Updated || propEnrty.IsModified)
                entityChanged.Properties.Add(new PropertieChanged { Name = item.Name, Value = propEnrty.CurrentValue ?? propEnrty.OriginalValue });
        }

        return entityChanged;
    }

    public ICollection<EntityChanged> GetChangesList(ICollection<T> updateEntityList, ICollection<T> createEntityList, ICollection<T> deleteEntityList) {
        var updatedList = updateEntityList.Select(i => GetChanges(i, EOperationPropertieChanged.Updated)).Where(i => i.Properties.Count > 0).ToList();

        var createdList = createEntityList.Select(i => GetChanges(i, EOperationPropertieChanged.Created)).ToList();

        var deletedList = deleteEntityList.Select(i => GetChanges(i, EOperationPropertieChanged.Deleted)).ToList();

        var list = new List<EntityChanged>();

        list.AddRange(updatedList);
        list.AddRange(createdList);
        list.AddRange(deletedList);

        return list;
    }
}