using System.Collections.Generic;
using System.Threading.Tasks;

using StoreDB.Models;
using StoreDB.Repos;

namespace StoreLib
{
    public class LocationService
    {
        private ILocationRepo repo;

        public LocationService(ILocationRepo repo)
        {
            this.repo = repo;
        }

        public List<string> GetLocationList() {
            List<string> locationList = new List<string>();
            Task<List<Location>> locationListTask = repo.GetAllLocationsAsync();
            foreach(Location location in locationListTask.Result) {
                locationList.Add($"[{location.LocationId}] {location.Name} ({location.Address})");
            }
            return locationList;
        }

        public Location GetLocationById(int locationId)
        {
            // add input validation
            return repo.GetLocationById(locationId);
        }

        public List<string> ViewLocationDetails(int locationId) {
            List<string> locationDetails = new List<string>();
            Location location = repo.GetLocationById(locationId);
            locationDetails.Add($"Location {location.LocationId}: {location.Name} ({location.Address})");
            locationDetails.Add($"Phone Number: {location.PhoneNumber}");
            return locationDetails;
        }
    }
}