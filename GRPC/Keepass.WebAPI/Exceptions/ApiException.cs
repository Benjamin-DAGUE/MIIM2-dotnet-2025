using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Exceptions;

public abstract class ApiException : Exception
{
    #region Properties

    public virtual int StatusCode { get; } = StatusCodes.Status500InternalServerError;

    #endregion

    #region Methods

    public virtual ErrorMessage ToObjectModel() => new()
    {
        Code = ErrorCode.Undefined,
        Message = $"Internal server error"
    };

    #endregion
}
