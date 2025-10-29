namespace Keepass.DBLib;
public class Vault
{
    public int Identifier { get; set; }
    public int AppUserIdentifier { get; set; }
    public string Name { get; set; } = string.Empty;
    public HashSet<VaultEntry> Entries { get; set; } = [];
    public HashSet<AppUser> SharedUsers { get; set; } = [];
    public AppUser? Creator { get; set; }
}
