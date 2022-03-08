using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMotownFestival.Api.DAL;
using RMotownFestival.Api.Data;
using RMotownFestival.Api.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        private readonly MotownDbContext _ctx;
        private readonly TelemetryClient _temeletryClient;

        public FestivalController(MotownDbContext context, TelemetryClient telemetry )
        {
            _ctx = context;
            _temeletryClient = telemetry;
        }

        [HttpGet("LineUp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Schedule))]
        public async Task<ActionResult> GetLineUp()
        {
            var schedule = await _ctx.Schedules
                .Include(s => s.Festival).ThenInclude(f => f.Stages)
                .Include(f => f.Festival).ThenInclude(f => f.Artists)
                .Include(i => i.Items)
                .ToListAsync();
            return Ok(schedule);
        }

        [HttpGet("Artists")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Artist>))]
        public async Task<ActionResult> GetArtists(bool? withRatings)
        {
            if (withRatings.HasValue && withRatings.Value)
            {
                _temeletryClient.TrackEvent($"List of artists with ratings");
            }
            else
            {
                _temeletryClient.TrackEvent($"List of artists without ratings");

            }


            var artists = await _ctx.Artists.ToListAsync();
            return Ok(artists);
        }

        [HttpGet("Stages")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Stage>))]
        public async Task<ActionResult> GetStages()
        {
            var stages = await _ctx.Stages.ToListAsync();
            return Ok(stages);
        }

        [HttpPost("Favorite")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ScheduleItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult SetAsFavorite(int id)
        {
            var schedule = FestivalDataSource.Current.LineUp.Items
                .FirstOrDefault(si => si.Id == id);
            if (schedule != null)
            {
                schedule.IsFavorite = !schedule.IsFavorite;
                return Ok(schedule);
            }
            return NotFound();
        }
    }
}