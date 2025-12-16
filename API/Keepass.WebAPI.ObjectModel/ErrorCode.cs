using System.Text.Json.Serialization;

namespace Keepass.WebAPI.ObjectModel;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCode
{
    Undefined = 0,
    NotImplemented = 1,
    VaultNotFound = 2,
    VaultLocked = 3,
    AppUserGetOrRegisterIssue = 4,
}
