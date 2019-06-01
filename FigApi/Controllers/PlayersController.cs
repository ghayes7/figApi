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
    public class PlayersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRosterHelper _rosterHelper;

        public PlayersController(
            IRosterHelper rosterHelper,
            ApplicationDbContext context)
        {
            _rosterHelper = rosterHelper;
            _context = context;
        }
        #region Queries
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            //by team id
            if (HttpContext.Request.Query.ContainsKey("teamId"))
            {
                string rawId = HttpContext.Request.Query["teamId"];
                int teamId = 0;
                if (string.IsNullOrWhiteSpace(rawId) || !int.TryParse(rawId, out teamId))
                {
                    if(Settings.QueryByCritOnInvalid)
                        return BadRequest("teamId invalid");
                    //else do default
                }
                else
                {
                    return await GetPlayersByTeamId(teamId);
                }
            }
            //last name
            if (HttpContext.Request.Query.ContainsKey("lastName"))
            {
                string name = HttpContext.Request.Query["lastName"];
                if (string.IsNullOrWhiteSpace(name))
                {
                    if(Settings.QueryByCritOnInvalid)
                        return BadRequest("lastName cannot be null or empty");
                    //else do default
                }
                else
                {
                    return await GetPlayersByLastName(name);
                }
               
            }
            //team name
            if (HttpContext.Request.Query.ContainsKey("teamName"))
            {
                string name = HttpContext.Request.Query["teamName"];
                if (string.IsNullOrWhiteSpace(name))
                {
                    if(Settings.QueryByCritOnInvalid)
                        return BadRequest("teamName cannot be null or empty");
                    //else do default
                }
                else
                {
                    return await GetPlayersByTeamName(name);
                }
               
            }
            //else get all
            return await _context.Players.ToListAsync(); //PFP: paginate and limit
        }

        [HttpGet("query/byLastName/{name}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByLastName(string name)
        {
            //no need to validate name, if it's empty just return all, could maybe trim though?
            //PFP: if you want to support unicode, may need to move to a query param so we can decode it... otherwise I'm assuming it's outside the scope of this exercise
            return await (from bb in _context.Players
                where EF.Functions.Like(bb.LastName, $"{name}")
                select bb).ToListAsync();
        }
        [HttpGet("query/onTeamId/{id}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeamId(int id)
        {
            return await (from bb in _context.Players
                where bb.TeamId== id
                select bb).ToListAsync();
        }
        [HttpGet("query/onTeamName/{name}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeamName(string name)
        {
            //find team
            var team= await (from bb in _context.Teams
                where EF.Functions.Like(bb.Name, $"{name}")
                select bb.Id).FirstOrDefaultAsync();

            if (team == 0)
                return NotFound("could not find team");
            
            return await (from bb in _context.Players
                where bb.TeamId== team
                select bb).ToListAsync();
        }
        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }
        #endregion

        // POST: api/Players
        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            //validate and set id=0
            if(!_rosterHelper.TryValidatePlayer(this,ref player,out var validationResult,ValidationOptions.IdIsEmpty))
                return validationResult;

            if(player.Team != null)
                return UnprocessableEntity("Player.Team must be null (to add to team, give TeamId, or use [put] /teams to create a team");


            player.Id = 0;

            //if team was passed, ensure it exists, and is not over player limit
            if (player.TeamId > 0)
            {
                if(!await _rosterHelper.TeamExistsAsync(player.TeamId)) //team does not exist
                    return NotFound("Player's Team does not exist");
                else if(await _rosterHelper.GetTeamPlayerCount(player.TeamId) > Settings.MaxPlayersPerTeam) //team over player limit
                    return BadRequest($"Player's team already has {Settings.MaxPlayersPerTeam} members");
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayerById", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }
    }
}
