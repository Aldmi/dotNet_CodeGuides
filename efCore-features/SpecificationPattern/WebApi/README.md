# SpecificationPattern

SpecificationPattern - представляет бизнесс-правила в виде цепочки объектов, связанных бизнес-логикой.
Используеься только для фильтрации данных, только для `Where(...)`

1. Инкапсуляция правил
2. Комбинация правил



### Описание

`Func<T, bool>` - IEnumerable </br>
`Expression<Func<T, bool>>` - IQueryable

`Func<T, bool>` - работает только с данными материализованными в памяти, т.е. не может превращаться в SQL запроос через EF
`Expression<Func<T, bool>>` - по Expression можно генерировать SQL выражение.

из `Expression` легко получить делегат `Func`, а вот обратная задача сложнее

### использование EntityFrameworkCore.Projectables

Позволяет комбинировать спецификации в `where` выражении




