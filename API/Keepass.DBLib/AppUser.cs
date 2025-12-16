namespace Keepass.DBLib;

public class AppUser
{
    public Guid Id { get; set; }
    public Guid EntraId { get; set; }

    public HashSet<Vault>? Vaults { get; set; }
    public HashSet<Vault>? SharedVaults { get; set; }
}
