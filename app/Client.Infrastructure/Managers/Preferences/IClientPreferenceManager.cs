using MudBlazor;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();
        Task<bool> ToggleDarkModeAsync();
    }
}