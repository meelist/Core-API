using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
   public interface IWorldRepository
   {
      IEnumerable<Trip> GetAllTrips();

      void AddTrip(Trip trip);

      Task<bool> SaveChangesAsync();
      Trip GetTripByName(string tripName, string name);
      void AddStop(string tripName, string username, Stop newStop);
      IEnumerable<Trip> GetUserTripsWithStops(string name);
   }
}