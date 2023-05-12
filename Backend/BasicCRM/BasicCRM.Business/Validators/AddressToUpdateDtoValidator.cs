﻿using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository;
using BasicCRM.Data.Repository.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Validators
{
    public class AddressToUpdateDtoValidator : AbstractValidator<AddressToUpdateDto>
    {
        private readonly IAddressRepository _repository;
        public AddressToUpdateDtoValidator(IAddressRepository repository)  
        {
            RuleFor(p => p.AddressID)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MustAsync(AddressExists).WithMessage("There is no address with that ID");

            RuleFor(p => p.AddressLine)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.City)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.State)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.ZipCode)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(x => x.ToString().Length == 5).WithMessage("{PropertyName} need to be 5 digits");

            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            _repository = repository;
        }

        private async Task<bool> AddressExists(Guid id, CancellationToken cancellationToken)
        {
            var addressFromDb = await _repository.GetByIdAsync(id);

            return !addressFromDb.AddressID.Equals(Guid.Empty);
        }
    }
}
