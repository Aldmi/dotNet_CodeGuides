## ToDoList

 Шаблон базового MVVM приложения Avalonia.

 #### особенности:
 MainWindow отрисовывает контент динамически - загружая ToDoListView или AddItemView.

 `ToDoListView` - окно, которое отрисовывает коллекцию ListItems.
  `Command="{Binding $parent[Window].DataContext.AddItem}"` кнопка AddItem вызывает метод не в своем DataContext, а вызывает метод AddItem главного окна.  

 `AddItemView` - окно добавления нового элемента.
 `OkCommand и CancelCommand` - задают Observable последовательность.
  На которые мы подписываемся  в методе AddItem.


 - [Code](https://docs.avaloniaui.net/ru/docs/tutorials/todo-list-app/)


