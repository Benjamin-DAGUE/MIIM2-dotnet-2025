using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Exceptions;

public class UnableToGetOrRegisterAppUserException(Guid entraId) : ApiException
{
    #region Properties

    public override int StatusCode => StatusCodes.Status500InternalServerError;

    public Guid EntraId { get; set; } = entraId;

    #endregion

    #region Methods

    public override ErrorMessage ToObjectModel() => new()
    {
        Code = ErrorCode.AppUserGetOrRegisterIssue,
        Message = $"Unable to get or register AppUser with id {EntraId}."
    };

    #endregion
}   
