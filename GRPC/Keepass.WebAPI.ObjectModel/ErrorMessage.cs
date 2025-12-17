namespace Keepass.WebAPI.ObjectModel;

public record ErrorMessage
{
    private readonly static ErrorMessage _default = new();

    public ErrorCode Code { get; init; } = ErrorCode.Undefined;
    public string Message { get; init; } = "Internal server error";
    public static ErrorMessage Default => _default;

    public override string ToString() => $"[{Code}] => {Message}";
}
