using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class AuthorValidator : AbstractValidator<AuthorEditModel>
    {
        public AuthorValidator() 
        {
            RuleFor(a => a.FullName)
                .NotEmpty()
                .WithMessage("ten tac gia khong duoc de trong")
                .MaximumLength(100)
                .WithMessage("ten tac gia toi da 100 ky tu");

            RuleFor(a => a.UrlSlug)
               .NotEmpty()
               .WithMessage("Urlslug khong duoc de trong")
               .MaximumLength(100)
               .WithMessage("Urlslug toi da 100 ky tu");
            RuleFor(a => a.JoinedDate)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("ngay tham gia khong hop le");

            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage("email khong duoc de trong")
                .MaximumLength(100)
                .WithMessage("email chua toio da 100 ky tu ");

            RuleFor(a => a.Notes)
                .MaximumLength(500)
                .WithMessage("ghi chu toio da 500 ky tu");
        }
    }
}
