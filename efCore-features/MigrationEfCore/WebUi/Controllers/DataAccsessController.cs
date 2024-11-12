
using Domain.Core.Entities;
using Infrastructure.Persistence.Pg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUi.Controllers;

[ApiController]
[Route("[controller]")]
public class DataAccsessController : ControllerBase
{
    
    private readonly ILogger<DataAccsessController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public DataAccsessController(ILogger<DataAccsessController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet(Name = "GetPersones")]
    public ActionResult<List<Persone>> Get()
    {
        var persones = _dbContext.Persones
            .Include(p=>p.Address)
            .ToList();
        
        return Ok(persones);
    }
    
    
    [HttpPost("AddPersone")]
    public ActionResult<Guid> AddPersone([FromBody]Persone persone)
    {
        var address= _dbContext.Set<Address>().FirstOrDefault(a => a.City == "Unknown");
        persone.Address = address!;
        var persones = _dbContext.Persones.Add(persone);
        _dbContext.SaveChanges();
        
        return Ok(persones);
    }
    
    
    
    /// <summary>
    /// PersoneDto - в режиме detached содержит полный объект для изменения.
    /// Вызов метода Update:
    /// если Id не выставленно в графе объектов сущности, то такой объект добавляется в БД
    /// если Id выставленно в графе объектов сущности, то такой объект изменяется.
    /// </summary>
    [HttpPost("AddPersone_detached")]
    public ActionResult<Guid> AddPersone_detached([FromBody]PersoneDto personeDto)
    {
        var persone = new Persone()
        {
            Id = personeDto.Id ?? Guid.Empty,
            Name = personeDto.Name,
            Age = personeDto.Age,
            Email =  Email.Create(personeDto.Email).Value,
            Address = new Address
            {
                Id = personeDto.Address.Id ?? Guid.Empty,
                Country = personeDto.Address.Country,
                City = personeDto.Address.City
            },
        };
        
         var persones = _dbContext.Persones.Update(persone);
         _dbContext.SaveChanges();
        
         return Ok(persones);

        //return Guid.NewGuid();
    }
}


public class PersoneDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public AddressDto Address { get; set; }
    public string Email { get; set; }
    public List<string>? CarsNumber { get; set; }
}

public class AddressDto
{
    public Guid? Id { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}