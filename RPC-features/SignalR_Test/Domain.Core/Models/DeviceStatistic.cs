namespace Domain.Core.Models;

public record DeviceStatistic(
    string DeviceKey,
    string ExchangeKey,
    bool IsConnect,
    bool TransportIsOpen,
    bool TransportIsCycleReopened,
    bool TransportIsStartedBg,
    ResponsePieceOfDataVm ResponsePieceOfDataVm
);

public record ResponsePieceOfDataVm(
    int TimeAction,
    bool IsValidAll,
    EvaluateVm Evaluate,
    string DataAction,
    string? ExceptionExchangePipline
);

public record EvaluateVm(
    int CountAll,
    int CountIsValid,
    int NumberPreparedPackages,
    string[]? ErrorStatArray = null
    
);