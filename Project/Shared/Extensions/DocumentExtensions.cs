namespace Exato.Shared.Extensions;

public static class DocumentExtensions
{
    public static bool IsValidCpf(this string cpf)
    {
        if (cpf.OnlyNumbers().IsEmpty()) return false;

        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        cpf = cpf.OnlyNumbers();
        if (cpf.Length != 11)
            return false;

        for (int i = 0; i < 10; i++)
            if (i.ToString().PadLeft(11, char.Parse(i.ToString())) == cpf)
                return false;

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }

    public static bool IsValidCnpj(this string cnpj)
    {
        if (cnpj.OnlyNumbers().IsEmpty()) return false;

        int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        cnpj = cnpj.OnlyNumbers();
        if (cnpj.Length != 14)
            return false;

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cnpj.EndsWith(digito);
    }

    public static string AsFormatedDocument(this string value)
    {
        var doc = value.OnlyNumbers();

        if (doc.IsValidCpf()) return doc.MaskNumber("###.###.###-##");

        if (doc.IsValidCnpj()) return doc.MaskNumber("##.###.###/####-##");

        return value;
    }

    public static string MaskNumber(this string value, string mask)
    {
        int valueIndex = 0;
        return new string(mask.Select(maskChar => maskChar == '#' ? value[valueIndex++] : maskChar).ToArray());
    }

    public static bool CanBeDocument(this string? value)
    {
        if (value.IsEmpty()) return false;

        var s = value!.Trim();
        foreach (var ch in s)
        {
            if ((ch >= '0' && ch <= '9') || ch == '-' || ch == '.' || ch == '/')
                continue;
            return false;
        }
        return true;
    }

    public static bool CanBeEmail(this string? value)
    {
        if (value.IsEmpty()) return false;

        return value!.Contains('@');
    }
}
