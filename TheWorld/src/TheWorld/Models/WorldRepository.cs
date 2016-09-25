using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
   public class WorldRepository : IWorldRepository
   {
      private readonly WorldContext _context;
      private readonly ILogger<WorldContext> _logger;

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

      public void AddTrip(Trip trip)
      {
         _context.Add(trip);
      }

      public Trip GetTripByName(string tripName, string name)
      {
         return _context.Trips
            .Include(t => t.Stops)
            .FirstOrDefault(t => t.Name == tripName && t.UserName == name);
      }

      public void AddStop(string tripName, string username, Stop newStop)
      {
         var trip = GetTripByName(tripName, username);

         if (trip != null)
         {
            trip.Stops.Add(newStop);
            _context.Stops.Add(newStop);
         }
      }

      public IEnumerable<Trip> GetUserTripsWithStops(string name)
      {
         try
         {
            return _context.Trips
               .Include(t => t.Stops)
               .OrderBy(t => t.Name)
               .Where(t => t.UserName == name)
               .ToList();
         }
         catch (Exception ex)
         {
            _logger.LogError($"Could not get trips with stops for user: {name}, ex: {ex}");
            return null;
         }
      }

      public async Task<bool> SaveChangesAsync()
      {
         return (await _context.SaveChangesAsync()) > 0;
      }
   }
}
