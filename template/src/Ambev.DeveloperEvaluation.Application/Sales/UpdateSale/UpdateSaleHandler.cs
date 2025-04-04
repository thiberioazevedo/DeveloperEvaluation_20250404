using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public UpdateSaleHandler(ISaleRepository saleRepository, ISaleItemRepository saleItemRepository, IProductRepository productRepository, IBranchRepository branchRepository, ICustomerRepository customerRepository, IMapper mapper, IEventBus eventBus)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
        if (branch == null)
            throw new InvalidOperationException($"Branch not found");

        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer == null)
            throw new InvalidOperationException($"Customer not found");

        var sale = await _saleRepository.GetByIdAsync(command.Id);

        if (sale == null)
            throw new InvalidOperationException($"Sale not found");

        if (sale.Cancelled)
            throw new InvalidOperationException($"Sale has cancelled");

        var saleItemCreateList = command.SaleItemCollection.Where(c => !sale.SaleItemCollection.Any(s => s.ProductId == c.ProductId)).Select(i => _mapper.Map<SaleItem>(i)).ToList();
        var saleItemUpdateList = sale.SaleItemCollection.
            Select(s => new 
                {
                    Command = command.SaleItemCollection.FirstOrDefault(c => c.ProductId == s.ProductId),
                    Entity = s
                }
            )
            .Where(i => i.Command != null)
            .ToList();

        var saleItemDeleteList = sale.SaleItemCollection.Where(s => !command.SaleItemCollection.Any(c => c.ProductId == s.ProductId)).ToList();

        sale.SetBranchId(command.BranchId);
        sale.SetCustomerId(command.CustomerId);
        sale.SetDate(command.Date);

        var saleEntityChanged = _saleRepository.GetChanges(sale, Domain.Common.EOperationPropertieChanged.Updated);

        var saleItemEntityChangedList = _saleItemRepository.GetChangesList(saleItemUpdateList.Select(i => i.Entity).ToList(), saleItemCreateList, saleItemDeleteList);

        sale.AddRange(saleItemCreateList);

        foreach (var item in saleItemUpdateList)
        {
            item.Entity.SetQuantity(item.Command.Quantity);
        }

        sale.RemoveRange(saleItemDeleteList);

        await SetUnitPriceAsync(saleItemCreateList);

        await _saleItemRepository.CreateRangeAsync(saleItemCreateList, cancellationToken);

        sale.Calculate();

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _eventBus.SendAsync(new SaleModifiedEvent { Id = command.Id });

        if (saleItemDeleteList.Count > 0)
            await _eventBus.SendAsync(new SaleItemDeletedEvent { Id = command.Id, ProductIdList = saleItemDeleteList.Select(i => i.ProductId).ToList() });

        return _mapper.Map<UpdateSaleResult>(sale);
    }

    async Task SetUnitPriceAsync(ICollection<SaleItem> saleItemCollection, CancellationToken cancellationToken = default)
    {
        foreach (var saleItem in saleItemCollection)
        {
            var product = await _productRepository.GetByIdAsync(saleItem.ProductId, cancellationToken);

            saleItem.SetUnitPrice(product?.UnitPrice ?? 0);
        }
    }
}