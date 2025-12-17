
using Keepass.WebAPI.ObjectModel;
using Keepass.WebApp.Core;

namespace Keepass.WebApp.Components.Pages;

public partial class VaultsPage
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
            IEnumerable<Vault>? vaults = await DownstreamApi.GetForUserAsync<IEnumerable<Vault>>("KeepassWebAPI", options =>
            {
                options.RelativePath = "Vault";
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
            Vault? vault = await DownstreamApi.PostForUserAsync<AddVaultQuery, Vault>("KeepassWebAPI", new AddVaultQuery()
            {
                Name = Guid.NewGuid().ToString(),
                Description = DateTime.Now.ToString()
            }, options =>
            {
                options.RelativePath = "Vault";
            });

            if(vault != null)
            {
                NavigationManager.NavigateTo($"/vaults/{vault.Id}");
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
