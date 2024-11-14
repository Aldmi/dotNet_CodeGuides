

using FluentValidation;

namespace Infrastructure.Persistance.Pg;

public class PgDbOption
{
    public string ConnectionString { get; init; }
}


public class PgDbOptionValidator : AbstractValidator<PgDbOption> 
{
    public PgDbOptionValidator()
    {
        RuleFor(option => option.ConnectionString)
            .NotNull()
            .NotEmpty();
    }
}