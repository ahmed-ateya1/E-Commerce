﻿using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.BrandDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.BrandQueries
{
    public class GetAllBrandQuery : IRequest<IEnumerable<BrandResponse>>
    {
        public PaginationDto Pagination { get; set; }
        public Expression<Func<Brand, bool>> Filter { get; set; }

        public GetAllBrandQuery(PaginationDto? pagination = null, Expression<Func<Brand, bool>>? filter = null)
        {
            Pagination = pagination;
            Filter = filter;
        }

    }
}
