namespace Exato.Front.Components.Empresas;

public class EmpresaRoleSelectData
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
