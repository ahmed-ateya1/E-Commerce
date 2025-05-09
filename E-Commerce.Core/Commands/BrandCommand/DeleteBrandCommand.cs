﻿using E_Commerce.Core.Dtos.BrandDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Commands.BrandCommand
{
    public class DeleteBrandCommand : IRequest<bool>
    {
        public Guid BrandID { get; set; }
        public DeleteBrandCommand(Guid brandID)
        {
            BrandID = brandID;
        }
    }
}
