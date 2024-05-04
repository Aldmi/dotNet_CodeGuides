using System.Reactive.Linq;
using System.Reactive.Subjects;
using Confluent.Kafka;
using CSharpFunctionalExtensions;

namespace RxConsumer;

public class RxKafkaConsumer : IDisposable
{
    private readonly string _topicName;
    private readonly IConsumer<Ignore, string> _consumer;
    private Subject<Result<string>>? _subject;


    public RxKafkaConsumer(string brokerEndpoints, string groupId, string topicName)
    {
        _topicName = topicName;

        var conf = new ConsumerConfig
        {
            BootstrapServers = brokerEndpoints,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = true,
            PartitionAssignmentStrategy = PartitionAssignmentStrategy.RoundRobin
        };

        _consumer = new ConsumerBuilder<Ignore, string>(conf)
            .SetLogHandler((consumer, message) =>
            {
                Console.WriteLine($"Logger:{message.Message}");
            }) //Если удалить топик во время работы, то вызовется этот метод, топик пересоздастя консюмером автоматом
            .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
            .Build();
    }


    // public bool ConsumerIsRun => _subject is not null;
    public bool ConsumerIsRun { get; private set; }


    public IObservable<Result<string>> Consume(CancellationToken cancellationToken)
    {
        if (ConsumerIsRun)
        {
            return _subject!;
        }

        _subject = new Subject<Result<string>>();
        _consumer.Subscribe(
            _topicName); //Если запустить Subscribe на топик ктоторого нет, то не будет ошибки, просто не будут идти данные. Если создать топик в процессе работы, то данные пойдут без ошибок.
        Task.Factory.StartNew(() =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            //var res = _consumer.Consume(TimeSpan.FromMilliseconds(500));
                            var res = _consumer.Consume(cancellationToken);
                            if (res != null)
                            {
                                _subject.OnNext(res.Message.Value);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (ConsumeException e)
                        {
                            if (e.Error.IsFatal)
                            {
                                break;
                            }

                            _subject.OnNext(Result.Failure<string>(e.Message));
                        }
                        catch (Exception e)
                        {
                            _subject.OnNext(Result.Failure<string>(e.Message));
                        }
                    }

                    _subject.OnCompleted();
                    _subject.Dispose();
                    _subject = null;
                    _consumer.Unsubscribe();
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default)
            .ConfigureAwait(false);

        return _subject;
    }


    public IObservable<Result<string>>? Consume_Ver2()
    {
        if (ConsumerIsRun)
        {
            return null;
        }

        return Observable.Create<Result<string>>(
            (obs, ct) =>
            {
                ConsumerIsRun = true;
                _consumer.Subscribe(_topicName);
                return Task.Run(() =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            var res = _consumer.Consume(ct);
                            if (res != null)
                            {
                                obs.OnNext(res.Message.Value);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (ConsumeException e)
                        {
                            if (e.Error.IsFatal)
                            {
                                break;
                            }

                            obs.OnNext(Result.Failure<string>(e.Message));
                        }
                        catch (Exception e)
                        {
                            obs.OnNext(Result.Failure<string>(e.Message));
                        }
                    }

                    Console.WriteLine("Loop terminated. Unsubscribe");
                    obs.OnCompleted();
                    _consumer.Unsubscribe();
                    ConsumerIsRun = false;
                }, ct);
            });
    }


    public Result<List<string>> ConsumeLastMessages(long countLastMessages)
    {
        try
        {
            TopicPartition topicPartition = new(_topicName, new Partition(0));
            WatermarkOffsets watermarkOffsets =
                _consumer.QueryWatermarkOffsets(topicPartition, TimeSpan.FromSeconds(1));
            var countAllMessages = watermarkOffsets.High - watermarkOffsets.Low;
            if (countAllMessages < countLastMessages)
            {
                countLastMessages = countAllMessages;
            }

            TopicPartitionOffset topicPartitionOffset =
                new(topicPartition, new Offset(watermarkOffsets.High.Value - countLastMessages));
            _consumer.Assign(topicPartitionOffset);

            ConsumeResult<Ignore, string>? consumeResult;
            List<string> resultList = [];
            do
            {
                consumeResult = _consumer.Consume(TimeSpan.FromSeconds(0.5));
                if (consumeResult != null)
                {
                    resultList.Add(consumeResult.Message.Value);
                }
            } while (consumeResult is {IsPartitionEOF: false});

            return resultList;
        }
        catch (Exception e)
        {
            return Result.Failure<List<string>>(e.Message);
        }
    }


    public void Dispose()
    {
        _subject?.OnCompleted();
        _consumer.Close();
        _consumer.Dispose();
    }
}