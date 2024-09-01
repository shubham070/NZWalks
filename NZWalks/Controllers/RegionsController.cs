using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        public RegionsController(NZWalksDbContext dbContext)
        {
           this._context = dbContext;
        }

        [HttpGet]
        [Route("GetAllRegions")]
        public IActionResult GetAll()
        {
            var regions = _context.Regions.ToList();
            return Ok(regions.Any() ? regions : GetDummyRegions());
        }

        [HttpGet]
        [Route("GetRegionById/{Id:Guid}")]
        public IActionResult GetRegionByGuid([FromRoute] Guid Id)
        {
            var regions = _context.Regions.FirstOrDefault(_=>_.Id.Equals(Id));
            return Ok(regions ?? GetDummyRegions().FirstOrDefault());
        }

        private List<Region> GetDummyRegions()
        {
            return new List<Region>()
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "MH",
                    Name = "Maharashtra",
                    RegionImageUrl = "https://en.m.wikipedia.org/wiki/File:Mumbai_03-2016_30_Gateway_of_India.jpg"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "KA",
                    Name = "Karnataka",
                    RegionImageUrl = "https://en.wikipedia.org/wiki/Bangalore_Palace#/media/File:Bangalore_Mysore_Maharaja_Palace.jpg"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "TN",
                    Name = "Tamil Nadu",
                    RegionImageUrl = "https://en.wikipedia.org/wiki/Meenakshi_Temple#/media/File:An_aerial_view_of_Madurai_city_from_atop_of_Meenakshi_Amman_temple.jpg"
                }
            };
        }
    }
}
