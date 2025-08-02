using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Estado do Brasil
/// </summary>
public enum BrazilState
{
    [Description("Acre")] AC,
    [Description("Alagoas")] AL,
    [Description("Amapá")] AP,
    [Description("Amazonas")] AM,
    [Description("Bahia")] BA,

    [Description("Ceará")] CE,
    [Description("Distrito Federal")] DF,
    [Description("Espírito Santo")] ES,
    [Description("Goiás")] GO,
    [Description("Maranhão")] MA,

    [Description("Mato Grosso")] MT,
    [Description("Mato Grosso do Sul")] MS,
    [Description("Minas Gerais")] MG,
    [Description("Pará")] PA,
    [Description("Paraíba")] PB,

    [Description("Paraná")] PR,
    [Description("Pernambuco")] PE,
    [Description("Piauí")] PI,
    [Description("Rio de Janeiro")] RJ,
    [Description("Rio Grande do Norte")] RN,

    [Description("Rio Grande do Sul")] RS,
    [Description("Rondônia")] RO,
    [Description("Roraima")] RR,
    [Description("Santa Catarina")] SC,
    [Description("São Paulo")] SP,

    [Description("Sergipe")] SE,
    [Description("Tocantins")] TO,
}
