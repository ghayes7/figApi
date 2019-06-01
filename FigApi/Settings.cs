using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FigApi.Models;

namespace FigApi
{
    public static class Settings
    {
        public static int MaxPlayersPerTeam { get; } = 8; //static in case we want to load from a file or something, otherwise const
        /// <summary>
        /// return 401 if query by is specified but invalid
        /// </summary>
        public static readonly bool QueryByCritOnInvalid = true;
        public static readonly OrderTeamsByField QueryByDefaultField = OrderTeamsByField.Name;
        /// <summary>
        /// Seed the database with some mock data
        /// </summary>
        public static readonly bool SeedDatabase = false;
    }
}
