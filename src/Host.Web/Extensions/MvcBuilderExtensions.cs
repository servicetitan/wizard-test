using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Host.Web;

internal static class MvcBuilderExtensions
{
    public static IMvcBuilder AddValidationHandling(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.AddMvcOptions(o => o.Filters.Add<ValidationExceptionFilter>());

    private class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception) {
                case null: return;
                case ValidationException validationException: {
                    var messages = validationException.Errors.Select(x => $"{x.ErrorCode}: {x.ErrorMessage}").ToList();
                    Exception? exception = validationException;

                    while (exception is not null) {
                        messages.Add(exception.Message);
                        exception = exception.InnerException;
                    }

                    var content = JsonSerializer.Serialize(new {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "One or more validation errors occurred.",
                        status = 400,
                        errors = new { _ = messages }
                    });
                    context.Result = new ContentResult
                        { Content = content, StatusCode = 400, ContentType = "application/json" };
                    context.ExceptionHandled = true;
                    break;
                }
            }
        }
    }
}
