using Client.Infrastructure.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace Client.App.Shared
{
    public partial class AuthLayout
    {
        private MudTheme _currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = AppTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
        }

    }
}
