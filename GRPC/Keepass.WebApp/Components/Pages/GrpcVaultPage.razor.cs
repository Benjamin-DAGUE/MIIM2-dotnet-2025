
using Google.Protobuf;
using Grpc.Net.Client;
using Keepass.RPCClient;
using Keepass.WebAPI.ObjectModel;
using Keepass.WebApp.Core;
using Microsoft.AspNetCore.Components;

namespace Keepass.WebApp.Components.Pages;

public partial class GrpcVaultPage
{
    #region Fields

    private bool _isLoading = true;

    private Vault? _vault = null;

    private ErrorMessage? _errorMessage;

    #endregion

    #region Properties

    [Parameter]
    public Guid Id { get; set; }

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (Id != Guid.Empty)
            {
                using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7123");
                VaultEndpoint.VaultEndpointClient client = new(channel);

                GetVaultReply reply = await client.GetOneAsync(new GetOneVaultRequest()
                {
                    Id = ByteString.CopyFrom(Id.ToByteArray()),
                });

                _vault = new Vault()
                {
                    Id = new Guid(reply.Id.ToByteArray()),
                    AppUserId = new Guid(reply.ApUserId.ToByteArray()),
                    Name = reply.Name,
                    Description = reply.Description
                };
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage? error = ex.TryGetErrorMessage();

            _errorMessage = error ?? ErrorMessage.Default;
        }
        catch (Exception)
        {
            _errorMessage = ErrorMessage.Default;
        }
        _isLoading = false;
    }

    #endregion
}
