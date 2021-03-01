using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace ExadelBonusPlus.Services.Models
{
    public class AddBonusDtoValidator : AbstractValidator<AddBonusDto>
    {
		public AddBonusDtoValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Title).Length(1, 9999);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.DateStart).NotNull();
            RuleFor(x => x.DateEnd).NotNull();
            RuleFor(x => x.CompanyId).NotNull().NotEmpty();
        }
	}

    public class UpdateBonusDtoValidator : AbstractValidator<UpdateBonusDto>
    {
        public UpdateBonusDtoValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Title).Length(1, 9999);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.DateStart).NotNull();
            RuleFor(x => x.DateEnd).NotNull();
            RuleFor(x => x.CompanyId).NotNull();
            RuleFor(x => x.CompanyId).NotNull().NotEmpty();
        }
    }

    public class BonusFilterValidator : AbstractValidator<BonusFilter>
    {
        public BonusFilterValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.SortBy).NotEmpty().WithMessage("Please, fill 'SortBy' parameter");
        }
    }
}
