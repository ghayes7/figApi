using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace FigApi.Models
{
    /// <summary>
    /// Indicates which field to order by
    /// </summary>
    public enum OrderTeamsByField
    {
        None=0,
        Name=1,
        Location=2,
    }

    public class Team
    {
        [Key]
        public int Id { get; set; }
//        [Required]
        public string Name { get; set; }
//        [Required]
        public string Location { get; set; }


        public List<Player> Players { get; set; }

        //used for seeding
        public Team(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}
