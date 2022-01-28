using Client.App.PeriodicExecutor;
using Client.Infrastructure.Settings;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client.App.Shared
{
    public partial class MainLayout
    {
        private string CurrentUserId { get; set; }
        private bool _isAuthenticated { get; set; }

        private async Task LoadDataAsync()
        {
            await _stateProvider.GetAuthenticationStateAsync();
        }

        private MudTheme _currentTheme;
        private bool _drawerOpen = true;

        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            _isAuthenticated = user != null && user.Identity?.IsAuthenticated == true;
            _currentTheme = AppTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
            WalletExecutor.StartExecuting();
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
    }
}