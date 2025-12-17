using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Keepass.RPCClient;
using Keepass.WebAPI.ObjectModel;
using Keepass.WebApp.Core;

namespace Keepass.WebApp.Components.Pages;

public partial class GrpcVaultsPage
{
    #region Fields

    private bool _isLoading = true;
    private Vault[]? _vaults = null;
    private ErrorMessage? _errorMessage;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        try
        {
            using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7123");
            VaultEndpoint.VaultEndpointClient client = new(channel);

            GetAllVaultReply reply = await client.GetAllAsync(new Empty());

            IEnumerable<Vault>? vaults = reply.Vaults.Select(v => new Vault()
            {
                Id = new Guid(v.Id.ToByteArray()),
                AppUserId = new Guid(v.ApUserId.ToByteArray()),
                Name = v.Name,
                Description = v.Description
            });

            _vaults = vaults?.ToArray();
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

    private async Task OnAddButtonClick()
    {
        _isLoading = true;
        try
        {
            using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7123");
            VaultEndpoint.VaultEndpointClient client = new(channel);

            GetVaultReply reply = await client.AddAsync(new AddVaultRequest()
            {
                ApUserId = ByteString.CopyFrom(Guid.NewGuid().ToByteArray()),
                Name = Guid.NewGuid().ToString(),
                Description = DateTime.Now.ToString()
            });

            Guid guid = new(reply.Id.ToByteArray());

            NavigationManager.NavigateTo($"/grpcvaults/{guid}");
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
