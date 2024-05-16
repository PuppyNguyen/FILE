using EA.Domain.FILE.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Application.FILE.Commands.Validations
{
    public abstract class ItemValidation<T> : AbstractValidator<T> where T : ItemCommand
    {
        protected readonly IItemRepository _context;
        public ItemValidation(IItemRepository context)
        {
            _context = context;
        }
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Id");
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the UserName")
                .Length(2, 225).WithMessage("The Name must have between 2 and 225 characters");
        }
        protected void ValidateIdExists()
        {
            RuleFor(x => x.Id).Must(IsExist).WithMessage("Id not exists");
        }
        private bool IsExist(Guid id)
        {
            return _context.CheckExistById(id).Result;
        }
    }
    public class ItemAddCommandValidation : ItemValidation<ItemAddCommand>
    {
        public ItemAddCommandValidation(IItemRepository context) : base(context)
        {
            ValidateName();
            ValidateId();
        }
    }
    public class ItemEditCommandValidation : ItemValidation<ItemEditCommand>
    {
        public ItemEditCommandValidation(IItemRepository context) : base(context)
        {
            ValidateName();
            ValidateId();
            ValidateIdExists();
        }
    }

    public class ItemDeleteCommandValidation : ItemValidation<ItemDeleteCommand>
    {
        public ItemDeleteCommandValidation(IItemRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
