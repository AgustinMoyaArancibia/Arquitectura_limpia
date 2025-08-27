using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTOs;
using static Users.Application.DTOs.UserDto;
namespace Users.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .MaximumLength(160);

            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(6);
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .MaximumLength(160);

            RuleFor(x => x.Role)
                .NotEmpty().MaximumLength(30);

            // Password es opcional al actualizar
            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password!)
                    .MinimumLength(6);
            });
        }
    }
}
