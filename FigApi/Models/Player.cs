using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace FigApi.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
//        [Required]
        public string FirstName { get; set; }
//        [Required]
        public string LastName { get; set; }


        public Team Team { get; set; }
//        [ForeignKey("Team")]
        public int TeamId { get; set; }
        //used in seeding
        public Player(string firstName, string lastName, int teamId=0)
        {
            FirstName = firstName;
            LastName = lastName;
            this.TeamId = teamId;
        }
    }
}
