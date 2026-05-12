using Syki.Back.Commands.Domain.Enums;

namespace Syki.Back.Shared;

public class BatchTypeCountOut
{
    public CommandBatchType Type { get; set; }
    public int Total { get; set; }
}
