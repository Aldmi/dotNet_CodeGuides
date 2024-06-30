namespace Application.Features.GetProduct;

public record GetProductDto(
    Guid ProductId,
    string Name,
    int Count,
    decimal Cost,
    DateTime CreateTime,
    DateTime? UpdateTime
    //bool ToolTop //Свойство из за которого будет Exception при компиляции mapping
);
