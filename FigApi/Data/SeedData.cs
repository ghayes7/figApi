using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FigApi.Models;

namespace FigApi.Data
{
    public class SeedData
    {
        public static void SeedFakeData(ApplicationDbContext db)
        {
            var team1 = new Team("Hawks", "Texas");
            var team2 = new Team("Tigers", "South Carolina");
            var team3 = new Team("Vikings", "Florida");
            var team4 = new Team("Bears", "Arizona");
            var team5 = new Team("Banana", "Illinois");
            db.Teams.AddRange(team1,team2,team3,team4,team5);
            db.SaveChanges();

            db.Add(new Player("marian", "green",team1.Id));
            db.Add(new Player("hailey", "burke",team1.Id));
            db.Add(new Player("tracy", "henderson",team1.Id));
            db.Add(new Player("wesley", "mitchelle",team1.Id));
            db.Add(new Player("ian", "garcia",team1.Id));
            db.Add(new Player("lynn", "wood",team1.Id));
            db.Add(new Player("connor", "smith",team1.Id));
            db.Add(new Player("wendy", "boyd",team1.Id));
            db.SaveChanges();
            //8
            db.Add(new Player("lance", "harris",team2.Id));
            db.Add(new Player("carter", "duncan",team2.Id));
            db.Add(new Player("lydia", "bates",team2.Id));
            db.Add(new Player("vera", "little",team2.Id));
            db.Add(new Player("carolyn", "palmer",team2.Id));
            db.Add(new Player("corey", "morgan",team2.Id));
            db.Add(new Player("april", "dunn",team2.Id));
            db.SaveChanges();
            //7
            db.Add(new Player("albert", "martinez",team3.Id));
            db.Add(new Player("anita", "fowler",team3.Id));
            db.Add(new Player("eric", "adams",team3.Id));
            db.Add(new Player("dave", "holmes",team3.Id));
            db.Add(new Player("gail", "grant",team3.Id));
            db.Add(new Player("allen", "castillo",team3.Id));
            db.SaveChanges();
            //6
            db.Add(new Player("renee", "davidson",team4.Id));
            db.Add(new Player("ron", "simpson",team4.Id));
            db.Add(new Player("darren", "hall",team4.Id));
            db.Add(new Player("mary", "jordan",team4.Id));
            db.Add(new Player("jack", "marshall",team4.Id));
            db.Add(new Player("kent", "scott",team4.Id));
            db.SaveChanges();
            //6
            //db.Add(new Player("wallace", "mitchelle",team1.Id));
            //db.Add(new Player("lance", "rivera",team1.Id));
            //db.Add(new Player("melinda", "wagner",team1.Id));
        }
    }
}
