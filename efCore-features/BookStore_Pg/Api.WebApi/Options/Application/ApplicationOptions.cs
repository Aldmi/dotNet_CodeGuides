using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using FluentValidation;
using Infrastructure.Persistance.Pg;

namespace Api.WebApi.Options.Application;

public class ApplicationOptions
{

    public PgDbOption PgDbOption { get; init; }



    public Result Validate()
    {
        var validator = new ApplicationOptionsValidator();
        var result = validator.Validate(this);
        var allErrors= result.ToString();
        return string.IsNullOrEmpty(allErrors) ?
            Result.Success() :
            Result.Failure(allErrors);
    }


    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(this, options);
        return jsonString;
    }
}


internal class ApplicationOptionsValidator : AbstractValidator<ApplicationOptions> 
{
    public ApplicationOptionsValidator()
    {
        RuleFor(options => options.PgDbOption)
            .NotNull()
            .SetValidator(new PgDbOptionValidator());
    }
}