using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Exceptions;

public class NotImplementException() : ApiException
{
    #region Properties

    public override int StatusCode => StatusCodes.Status501NotImplemented;

    #endregion

    #region Methods

    public override ErrorMessage ToObjectModel() => new()
    {
        Code = ErrorCode.NotImplemented,
        Message = "Server not implemented"
    };

    #endregion
}