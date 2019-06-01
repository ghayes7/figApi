using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FigApi.Models.Helpers
{
    /// <summary>
    /// Helper for Various controller tasks
    /// </summary>
    public interface IRosterHelper
    {
        bool PlayerExists(int id);
        Task<bool> PlayerExistsAsync(int id);
        bool TeamExists(int id);
        Task<bool> TeamExistsAsync(int id);
        Task<int> GetTeamPlayerCount(int id);
        bool TryValidatePlayer(ControllerBase controller,ref Player player, out ActionResult result, ValidationOptions options = ValidationOptions.None);
        bool TryValidateTeam(ControllerBase controller,ref Team team, out ActionResult result, ValidationOptions options = ValidationOptions.None);
    }
}