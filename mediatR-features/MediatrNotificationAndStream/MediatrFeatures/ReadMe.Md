## Request.
Выполнение по умолчанию. Должен быть зарегистрирован хотя бы один обработчик.
связь One2One

## Notifications.
Обработчики выполянются последовательно один за другим, если нет ни одного обрабюотчика, то исключение НЕ выдается.
связь One2Many

## Stream.
конечная точка отдает `IAsyncEnumerable<TResponse>`.
клиент должен уметь работать с потоковыми данными.
`IStreamRequestHandler<in TRequest, out TResponse>` выдает `IAsyncEnumerable<TResponse>` поэтмоу легко отдать потоковые данные

для получения потоковых данных:
`curl http://localhost:5037/stream/erer`

