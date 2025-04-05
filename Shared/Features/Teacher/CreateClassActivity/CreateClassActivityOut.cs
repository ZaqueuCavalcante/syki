namespace Syki.Shared;

public class CreateClassActivityOut
{
    public Guid Id { get; set; }
    
    public static implicit operator CreateClassActivityOut(OneOf<CreateClassActivityOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
