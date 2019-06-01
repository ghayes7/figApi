using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FigApi.Models;

namespace FigApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FigApi.Models.Team> Teams { get; set; }

        public DbSet<FigApi.Models.Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
            builder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Team>(e => e.Team)
                    .WithMany(e => e.Players)
                    .HasForeignKey(e => e.TeamId);
            });
        }
    }
}
