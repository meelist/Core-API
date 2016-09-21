using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
       private readonly WorldContext _context;
       private ILogger<WorldContext> _logger;

       public WorldRepository(WorldContext context, ILogger<WorldContext> logger)
       {
          _context = context;
          _logger = logger;
       }

       public IEnumerable<Trip> GetAllTrips()
       {
         _logger.LogInformation("Getting All Trips from the Database");

          return _context.Trips.ToList();
       }
    }
}
