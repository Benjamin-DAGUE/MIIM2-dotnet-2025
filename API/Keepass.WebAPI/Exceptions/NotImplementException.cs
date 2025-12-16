using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Exceptions;

public class NotImplementException() : ApiException
{
    #region Properties

    public override int StatusCode => StatusCodes.Status501NotImplemented;

    #endregion

    #region Methods

    public override ErrorMessage ToObjectModel() => base.ToObjectModel() with
    {
        Code = ErrorCode.NotImplemented
    };

    #endregion
}