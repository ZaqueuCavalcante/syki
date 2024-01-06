namespace Syki.Back.Domain;

public class Nota
{
    public static decimal Get(decimal nota1, decimal nota2, decimal notaFinal)
    {
        return (nota1 + nota2) / 2.0M;
    }
}
