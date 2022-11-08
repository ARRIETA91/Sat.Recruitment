using FluentValidation;
using System.Text.RegularExpressions;

namespace Sat.Recruitment.Core.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.CreateUserRequest.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The name is required");

            RuleFor(x => x.CreateUserRequest.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The address is required");

            RuleFor(x => x.CreateUserRequest.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The email is required")
            .Must(x => Regex.IsMatch(x, Constants.EmailFormatPattern))
            .WithMessage("The email is not valid");

            RuleFor(x => x.CreateUserRequest.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The phone is required");
        }
    }
}
