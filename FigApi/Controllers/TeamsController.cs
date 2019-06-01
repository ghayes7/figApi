using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FigApi.Models;
using FigApi.Models.Helpers;

namespace FigApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRosterHelper _rosterHelper;

        public TeamsController(
            IRosterHelper rosterHelper,
            ApplicationDbContext context)
        {
            _rosterHelper = rosterHelper;
            _context = context;
        }
        #region Queries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            OrderTeamsByField orderByField = OrderTeamsByField.None;
            //see if they passed orderBy
            if (HttpContext.Request.Query.ContainsKey("orderBy"))
            {
                string field = HttpContext.Request.Query["orderBy"]; //get field

                //if we are set to crit and it's null, throw
                if (Settings.QueryByCritOnInvalid && string.IsNullOrEmpty(field))
                    return UnprocessableEntity("queryBy cannot be null or empty");
                //convert field(str) to enum
                switch (field.ToLower())
                {
                    case "name":
                    {
                        orderByField = OrderTeamsByField.Name;
                        break;
                    }
                    case "location":
                    {
                        orderByField = OrderTeamsByField.Location;
                        break;
                    }
                    default:
                    {
                        if (!Settings.QueryByCritOnInvalid) //default it then
                            orderByField = Settings.QueryByDefaultField;
                        else
                            return UnprocessableEntity("invalid queryBy field");
                        break;
                    }
                        
                }
            }

            var query = _context.Teams.AsQueryable();
            if (orderByField != OrderTeamsByField.None)
            {
                switch (orderByField)
                {
                    case OrderTeamsByField.Name:
                        query = query.OrderBy(e => e.Name);
                        break;
                    case OrderTeamsByField.Location:
                        query = query.OrderBy(e => e.Location);
                        break;
                    default:
                        return UnprocessableEntity("queryBy cannot be null or empty"); //cannot get here, but still... in case of changes or something
                }
            }

            return await query.ToListAsync();
        }


        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }
        #endregion

        // POST: api/Teams
        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam(Team team)
        {
            //validate
            if (string.IsNullOrWhiteSpace(team.Location))
                return UnprocessableEntity("Location cannot be empty or null");

            if (string.IsNullOrWhiteSpace(team.Name))
                return UnprocessableEntity("Name cannot be empty or null");

            //trim strings, comment out if undesired
            team.Name = team.Name.Trim();
            team.Location = team.Location.Trim();
            team.Id = 0; //force id to be zero, otherwise EF will crit

            //validate players if attached
            if (team.Players?.Any() ?? false)
            {
                //ensure team does not exceed 8
                if (team.Players.Count > Settings.MaxPlayersPerTeam)
                    return UnprocessableEntity($"A Team cannot have more than {Settings.MaxPlayersPerTeam} players");

                foreach (var player in team.Players)
                {
                    player.Id = 0; //enforce Id is empty
                    //validate
                    if (string.IsNullOrWhiteSpace(player.FirstName))
                        return UnprocessableEntity("Player first name cannot be empty or null");

                    if (string.IsNullOrWhiteSpace(player.LastName))
                        return UnprocessableEntity("Player last name cannot be empty or null");
                }
            }
            //ensure there are no other teams with the same name, use ef.like for case insensitive
            //if you wanted to tell the user which matched, you'd break this into two separate queries
            var tTeam = (from bb in _context.Teams
                where EF.Functions.Like(bb.Name, $"{team.Name}") || 
                      EF.Functions.Like(bb.Location, $"{team.Location}")
                select bb).FirstOrDefault();
            if(tTeam != null)
                return UnprocessableEntity("Name & Location must be unique!");

            _context.Teams.Add(team);
            await _context.SaveChangesAsync(); //PFP: maybe try catch...

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }


        [HttpGet("{teamId}/addPlayer/{playerId}")]
        public async Task<IActionResult> AddPlayerToTeam(int teamId, int playerId)
        {
            //ensure both exist
            if(!(await _rosterHelper.TeamExistsAsync(teamId)))
                return NotFound("Team does not exist");
            
            var player = await _context.Players.FindAsync(playerId);
            if (player == null)
            {
                return NotFound("Player does not exist");
            }

            if(await _rosterHelper.GetTeamPlayerCount(teamId) >= Settings.MaxPlayersPerTeam) //team over player limit
                return BadRequest($"Player's team already has {Settings.MaxPlayersPerTeam} members");

            player.TeamId = teamId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("{teamId}/removePlayer/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromTeam(int teamId, int playerId)
        {
            //ensure both exist
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return NotFound("Team does not exist");
            }
            var player = await _context.Players.FindAsync(playerId);
            if (player == null)
            {
                return NotFound("Player does not exist");
            }

            if (player.TeamId != teamId)
                return BadRequest("Player Not On specified team");

            player.TeamId = 0;
            player.Team = null;
            //TODO: ensure this does not delete the team!
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
