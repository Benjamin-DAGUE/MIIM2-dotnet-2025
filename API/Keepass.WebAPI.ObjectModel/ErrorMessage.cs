namespace Keepass.WebAPI.ObjectModel;

public record ErrorMessage
{
    public required ErrorCode Code { get; init; }
    public required string Message { get; init; }

    public override string ToString() => $"[{Code}] => {Message}";
}
