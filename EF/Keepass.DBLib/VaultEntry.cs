namespace Keepass.DBLib;

public class VaultEntry
{
    public int Identifier { get; set; }
    public int VaultIdentifier { get; set; }
    public string Username { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public Vault? Vault { get; set; }
}
