using FluentValidation;
using System.Reflection;

namespace TatBlog.WebApi.Validations
{
    public static class FluentValidationDependencyInjection
    {
        public static WebApplicationBuilder configureFluentValidation(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly());
            return builder;
        }
    }
}
