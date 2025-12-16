namespace Keepass.DBLib;
public class Vault
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public HashSet<VaultEntry>? Entries { get; set; }
    public HashSet<AppUser>? SharedUsers { get; set; }
    public AppUser? Creator { get; set; }
}
