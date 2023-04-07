using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        public PostValidator()
        {
            RuleFor(a => a.Title)
                .NotEmpty()
                .WithMessage("ten tieu de khong duoc de trong")
                .MaximumLength(100)
                .WithMessage("ten tac gia toi da 100 ky tu");

            //RuleFor(a => a.UrlSlug)
            //   .NotEmpty()
            //   .WithMessage("Urlslug khong duoc de trong")
            //   .MaximumLength(100)
            //   .WithMessage("Urlslug toi da 100 ky tu");


            RuleFor(a => a.ShortDesciption)
                .NotEmpty()
                .WithMessage("Description khong duoc de trong")
                .MaximumLength(100)
                .WithMessage("Description chua toi da 100 ky tu ");

            RuleFor(a => a.Description)
               .NotEmpty()
               .WithMessage("Description khong duoc de trong")
               .MaximumLength(100)
               .WithMessage("Description chua toi da 100 ky tu ");

         



        }
    }
}
