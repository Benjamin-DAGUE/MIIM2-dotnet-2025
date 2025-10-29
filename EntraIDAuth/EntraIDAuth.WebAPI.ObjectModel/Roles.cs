namespace EntraIDAuth.WebAPI.ObjectModel;
public static class Roles
{
    public const string User = "User";
    public const string Administrator = "Administrator";
    public const string User_Administrator = $"{User},{Administrator}";
}
