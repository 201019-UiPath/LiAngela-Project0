using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public class LocationRepo : ILocationRepo
    {
        private StoreContext context;
        
        public LocationRepo(StoreContext context)
        {
            this.context = context;
        }

        public Task<List<Location>> GetAllLocationsAsync()
        {
            return context.Locations.Select(x => x).ToListAsync();
        }

        public Location GetLocationById(int locationId)
        {
            return context.Locations.Where(x => x.LocationId == locationId).SingleOrDefault();
        }
    }
}