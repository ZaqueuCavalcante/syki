namespace Exato.Shared.Auth;

public class FeatureGroup
{
	public int Id { get; set; }
	public string Name { get; set; }

	public FeatureGroup(int id, string name)
    {
		Id = id;
		Name = name;
    }
}
