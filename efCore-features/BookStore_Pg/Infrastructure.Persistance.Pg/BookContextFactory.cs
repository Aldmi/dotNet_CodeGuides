﻿using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistance.Pg;

public class BookContextFactory: IDesignTimeDbContextFactory<BookContext>
{
    public BookContext CreateDbContext(string[] args)
    {
        var pathToFile= Path.Combine(Directory.GetCurrentDirectory(), "..", "Api.WebApi");
        Console.WriteLine("Creating book context...");
        
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(pathToFile)
            .AddJsonFile("appsettings.json")
            .Build();
        
        var pgDbOption = configuration
            .GetSection("ApplicationOptions")
            .GetSection("PgDbOption")
            .Get<PgDbOption>();

        if (pgDbOption is null)
            throw new DesignTimeDbContextFactoryException("Not found Configurtion section `ApplicationOptions.PgDbOption`");
        
        var validator = new PgDbOptionValidator();
        var result=validator.Validate(pgDbOption);
        if (!result.IsValid)
        {
            throw new DesignTimeDbContextFactoryException(result.Errors);
        }
        
        Console.WriteLine($"PgDbOption: {pgDbOption!.ConnectionString}");
        var bookContext = new BookContext(pgDbOption!.ConnectionString);
        Console.WriteLine("Created book context");
        return bookContext;
    }
}


public class DesignTimeDbContextFactoryException : Exception
{
    public DesignTimeDbContextFactoryException(string message) : base(message)
    {
    }
    
    public DesignTimeDbContextFactoryException(IEnumerable<ValidationFailure> listValidationFailures) : base(ListValidationFailuresToMessage(listValidationFailures))
    {
    }

    private static string ListValidationFailuresToMessage(IEnumerable<ValidationFailure> listValidationFailures)=>
        string.Join(", ", listValidationFailures.Select(err=>err.ErrorMessage).ToArray() );
}
