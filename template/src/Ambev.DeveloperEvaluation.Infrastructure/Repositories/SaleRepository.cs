using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Infrastructure.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(DefaultContext context) : base(context)
    {
    }

    public override IQueryable<Sale> GetQuery()
    {
        return base.GetQuery()
                   .Include(i => i.Branch)
                   .Include(i => i.Customer)
                   .Include(i => i.SaleItemCollection).DefaultIfEmpty();
    }

    public override IQueryable<Sale> GetQuery(string? searchText, string? colunaOrdenacao, bool ordenacaoAscendente, CancellationToken cancellationToken)
    {
        return GetQuery().OrderByDescending(i => i.Number);
    }
}
