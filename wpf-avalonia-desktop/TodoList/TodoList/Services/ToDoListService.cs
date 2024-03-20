using System.Collections.Generic;
using TodoList.Models;

namespace TodoList.Services;

public class ToDoListService
{
    public IEnumerable<ToDoItem> GetItems() => new[]
    {
        new ToDoItem { Description = "Walk the dog" },
        new ToDoItem { Description = "Buy some milk" },
        new ToDoItem { Description = "Learn Avalonia", IsChecked = true },
    };
}