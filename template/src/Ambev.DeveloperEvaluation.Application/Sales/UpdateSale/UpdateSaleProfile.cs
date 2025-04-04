﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Sale>();
        CreateMap<SaleItemCommand, SaleItem>();
        CreateMap<Sale, UpdateSaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
    }
}
