using System.Diagnostics;
using Syki.Back.Commands.Domain.Enums;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Commands.Domain.Commands;

public class Command
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Duration { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }

    /// <summary>
    /// Id do comando que gerou o comando
    /// Utilizado quando um comando gera outro em seu handler
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Id do comando com erro que gerou o comando atual
    /// Utilizado quando o comando original está com erro e é reprocessado
    /// O comando atual é uma cópia do original (imutabilidade)
    /// </summary>
    public int? OriginalId { get; set; }

    /// <summary>
    /// Id do lote que contém o comando
    /// </summary>
    public int? BatchId { get; set; }

    public DateTime? NotBefore { get; set; }

    public string? ActivityId { get; set; }

    /// <summary>
    /// Número máximo de tentativas de reprocessamento automático do comando.
    /// Quando o comando falha e MaxRetries > 0, um novo comando de retry é criado com MaxRetries - 1.
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    /// Which retry attempt this is. 0 for the original command, 1 for first retry, etc.
    /// </summary>
    public int RetryAttempt { get; set; }

    /// <summary>
    /// Backoff strategy for computing delay between retries.
    /// When None (default), retries are immediate.
    /// </summary>
    public BackoffStrategy BackoffStrategy { get; set; }

    /// <summary>
    /// Base delay in seconds used by the backoff formula. Default is 5.
    /// </summary>
    public int BaseDelaySeconds { get; set; }

    public List<string> Logs { get; set; } = [];

    public Institution? Institution { get; set; }

    public Command() { }

    public Command(
        Institution institution,
        object data,
        int? parentId = null,
        int? originalId = null,
        int? batchId = null,
        int? delaySeconds = null,
        string? activityId = null,
        int maxRetries = 0,
        BackoffStrategy backoffStrategy = BackoffStrategy.None,
        int baseDelaySeconds = 5
    ) {
        Institution = institution;
        Type = data.GetType().Name;
        Data = data.Serialize();
        CreatedAt = DateTime.UtcNow;
        ParentId = parentId;
        OriginalId = originalId;
        BatchId = batchId;
        ActivityId = activityId;
        NotBefore = delaySeconds != null ? DateTime.UtcNow.AddSeconds(delaySeconds.Value) : null;
        MaxRetries = maxRetries;
        BackoffStrategy = backoffStrategy;
        BaseDelaySeconds = baseDelaySeconds;
    }

    public Command(
        int institutionId,
        object data,
        int? parentId = null,
        int? originalId = null,
        int? batchId = null,
        int? delaySeconds = null,
        string? activityId = null,
        int maxRetries = 0,
        BackoffStrategy backoffStrategy = BackoffStrategy.None,
        int baseDelaySeconds = 5
    ) {
        InstitutionId = institutionId;
        Type = data.GetType().Name;
        Data = data.Serialize();
        CreatedAt = DateTime.UtcNow;
        ParentId = parentId;
        OriginalId = originalId;
        BatchId = batchId;
        ActivityId = activityId;
        NotBefore = delaySeconds != null ? DateTime.UtcNow.AddSeconds(delaySeconds.Value) : null;
        MaxRetries = maxRetries;
        BackoffStrategy = backoffStrategy;
        BaseDelaySeconds = baseDelaySeconds;
    }

    public void SetAwaiting()
    {
        Status = CommandStatus.Awaiting;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.UtcNow;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? CommandStatus.Error : CommandStatus.Success;
    }

    public ActivityContext GetParentActivityContext()
    {
        ActivityContext.TryParse(ActivityId, null, out var parsedContext);
        return parsedContext;
    }
}
