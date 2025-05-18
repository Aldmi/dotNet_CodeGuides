using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Todos;

internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.DueDate).HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        
        /*
         Даже без навигационных свойств такая конфигурация полезна, так как:
        Чётко определяет бизнес-правило ("задача принадлежит пользователю").
        Обеспечивает целостность данных на уровне БД.
         Позволяет при необходимости добавить навигационные свойства позже без изменения конфигурации.
        */
        builder.HasOne<User>().WithMany().HasForeignKey(t => t.UserId);
    }
}
