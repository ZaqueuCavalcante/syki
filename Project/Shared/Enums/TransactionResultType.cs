namespace Exato.Shared.Enums;

/// <summary>
/// Tipos de resultado de uma transação do Intelligence.
/// </summary>
public enum TransactionResultType
{
    Success = 1,
    SuccessWithRemarks = 2,
    InvalidInputData = 3,
    UnavailableData = 4,
    EntityNotFound = 5,
    InvalidParameters = 6,
    ParametersNotSupported = 7,
    RemoteSystemUnavailable = 8,
    Timeout = 9,
    AttemptsLimitReached = 10,
    RemoteSystemError = 11,
    AsyncExecutionInProgress = 12,
    AwaitingExecution = 13,

    InsufficientBalance = 101,
    SimultaneousTransactionsLimitReached = 102,
    TransactionUnavailable = 103,
    AccessDenied = 104,
    TransactionCancelled = 105,

    InternalError = 255,
}
