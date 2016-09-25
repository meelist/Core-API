using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
   [Route("api/trips")]
   [Authorize]
   public class TripsController : Controller
   {
      private readonly IWorldRepository _repo;
      private readonly ILogger<TripsController> _logger;

      public TripsController(IWorldRepository repo, ILogger<TripsController> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      [HttpGet("")]
      public IActionResult Get()
      {
         try
         {
            var results = _repo.GetUserTripsWithStops(User.Identity.Name);
            return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
         }
         catch (Exception ex)
         {
            _logger.LogError($"Failed to get All Trips: {ex}");
            return BadRequest("Error occurred");
         }
      }

      [HttpPost("")]
      public async Task<IActionResult> Post([FromBody] TripViewModel theTrip)
      {
         if (ModelState.IsValid)
         {
            //Save to the DataBase

            var newTrip = Mapper.Map<Trip>(theTrip);

            newTrip.UserName = User.Identity.Name;
            _repo.AddTrip(newTrip);

            if (await _repo.SaveChangesAsync())
            {
               return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
            }
         }

         return BadRequest("Failed to save the trip"); //if private api then return BadRequest(ModelState);
      }
   }
}