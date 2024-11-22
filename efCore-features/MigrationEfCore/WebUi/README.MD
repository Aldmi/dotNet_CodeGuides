### Для выполнения команды нужно находится в корне проекта (и указывать относительный путь для --project и --startup-project)

#### Вывести список миграций

 `dotnet ef migrations list --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


#### Создать миграцию

`dotnet ef migrations add Init --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


#### обновить БД до последней миграции

`dotnet ef database update --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


#### Удалить последнюю миграцию


`dotnet ef migrations remove --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


#### Информация про контекст БД

`dotnet ef dbcontext info --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


#### Создать скрипт ВСЕХ миграций и записать его в файл "script.sql"

` dotnet ef migrations script --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi | out-file ./script.sql`


#### Создать скрипт не примененных миграций

`dotnet ef migrations script --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi --idempotent`

#### (можно создать скрипт после определенной миграции)

`dotnet ef migrations script 20231206141937_ADD_ADDRESS --project Infrastructure.Persistence.Pg --context ApplicationDbContext --startup-project WebUi`


## Использовать IDesignTimeDbContextFactory
Если реализовать фабрику DbContext в проекте c самим `ApplicationDbContext`  `Infrastructure.Persistence.Pg`
тогда можно отказаться от зависимости `Microsoft.EntityFrameworkCore.Design` в startup проекте `WebUi`

`dotnet ef migrations list --project Infrastructure.Persistence.Pg --context ApplicationDbContext`




