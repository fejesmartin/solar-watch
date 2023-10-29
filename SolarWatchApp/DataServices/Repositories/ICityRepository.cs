using SolarWatchApp.Models;

namespace SolarWatchApp.DataServices.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<City?>> GetAllCitiesAsync();
    Task<City?> GetCityByIdAsync(int id);
    Task<City> GetCityByNameAsync(string name);
    Task<City> CreateCityAsync(City city);
    Task UpdateCityAsync(City city);
    Task DeleteCityAsync(int id);
}