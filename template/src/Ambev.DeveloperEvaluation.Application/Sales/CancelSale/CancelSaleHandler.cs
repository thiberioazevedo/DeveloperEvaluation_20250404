using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventBus eventBus)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale == null)
            throw new InvalidOperationException($"Sale not found");

        if (sale.Cancelled)
            throw new InvalidOperationException($"Sale has cancelled");

        sale.Cancel();

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        var result = _mapper.Map<CancelSaleResult>(sale);

        await _eventBus.SendAsync(new SaleCancelledEvent(result.Id));

        return result;
    }
}
