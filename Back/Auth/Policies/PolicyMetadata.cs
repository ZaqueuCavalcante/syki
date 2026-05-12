using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public class PolicyMetadata
{
	public string Name { get; set; }
	public List<SykiPermission> Permissions { get; set; } = [];
}
