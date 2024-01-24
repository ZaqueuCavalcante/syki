namespace Syki.Back.Exceptions;

public static class DomainExceptions
{
    public static DomainException DE0000()
    {
        return new DomainException(ExceptionMessages.DE0000);
    }
    public static DomainException DE0001()
    {
        return new DomainException(ExceptionMessages.DE0001);
    }
}
