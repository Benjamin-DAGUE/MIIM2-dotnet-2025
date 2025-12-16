namespace Keepass.DBLib;

public class VaultEntry
{
    public Guid Id { get; set; }
    public Guid VaultId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; //NOTE : A revoir pour ne pas garder le MDP en claire

    public Vault? Vault { get; set; }
}
