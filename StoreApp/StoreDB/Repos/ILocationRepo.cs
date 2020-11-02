using System.Threading.Tasks;
using System.Collections.Generic;

using StoreDB.Models;

namespace StoreDB.Repos
{
    public interface ILocationRepo
    {
         Task<List<Location>> GetAllLocationsAsync();

         Location GetLocationById(int locationId);
    }
}