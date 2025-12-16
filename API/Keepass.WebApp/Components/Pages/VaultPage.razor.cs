
using Keepass.WebAPI.ObjectModel;
using Keepass.WebApp.Core;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Keepass.WebApp.Components.Pages;

public partial class VaultPage
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
                Vault? vault = await DownstreamApi.GetForUserAsync<Vault>("KeepassWebAPI", options =>
                {
                    options.RelativePath = $"/Vault/{Id}";
                });

                _vault = vault;
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
