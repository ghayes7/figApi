using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FigApi.Models.Helpers
{
    /// <summary>
    /// Validation configuration
    /// </summary>
    [Flags]
    public enum ValidationOptions
    {
        None=0x0,
        /// <summary>
        /// Enforces the id to be zero
        /// </summary>
        IdIsEmpty=0x1,
        //ensures all relationships exist (nav props)
        RelationshipsExist=0x2, //unused
        ValidateRelationships=0x3, //unused
    }

    public class RosterHelper : IRosterHelper
    {
        private readonly ApplicationDbContext _context;

        public RosterHelper(
            ApplicationDbContext appContext)
        {
            _context = appContext;
        }



        public bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }

        public Task<bool> PlayerExistsAsync(int id)
        {
            return _context.Players.AnyAsync(e => e.Id == id);
        }
        public bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
        public Task<bool> TeamExistsAsync(int id)
        {
            return _context.Teams.AnyAsync(e => e.Id == id);
        }
        public Task<int> GetTeamPlayerCount(int id)
        {
            return (from bb in _context.Players
                where bb.TeamId == id
                select bb.Id).CountAsync();
        }
        /// <summary>
        /// Validate a player instance
        /// </summary>
        /// <param name="controller">referring controller</param>
        /// <param name="player">player</param>
        /// <param name="result">out'ted result, may be null</param>
        /// <param name="options">any config options</param>
        /// <returns>bool indicating if player is valid</returns>
        public bool TryValidatePlayer(ControllerBase controller,ref Player player, out ActionResult result, ValidationOptions options = ValidationOptions.None)
        {
            if(options.HasFlag(ValidationOptions.IdIsEmpty))
                player.Id = 0; //enforce Id is empty

            result = null;
            //validate
            if (string.IsNullOrWhiteSpace(player.FirstName))
            {
                result= controller.UnprocessableEntity("Player first name cannot be empty or null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(player.LastName))
            {
                result= controller.UnprocessableEntity("Player last name cannot be empty or null");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Validate a team instance
        /// </summary>
        /// <param name="controller">referring controller</param>
        /// <param name="team">team</param>
        /// <param name="result">out'ted result, may be null</param>
        /// <param name="options">any config options</param>
        /// <returns>bool indicating if team is valid</returns>
        public bool TryValidateTeam(ControllerBase controller,ref Team team, out ActionResult result, ValidationOptions options = ValidationOptions.None)
        {
            if(options.HasFlag(ValidationOptions.IdIsEmpty))
                team.Id = 0; //enforce Id is empty

            result = null;
            //validate
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                result= controller.UnprocessableEntity("Location cannot be empty or null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(team.Location))
            {
                result= controller.UnprocessableEntity("Name cannot be empty or null");
                return false;
            }

            return true;
        }
    }
}
