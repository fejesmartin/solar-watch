using Microsoft.EntityFrameworkCore;
using SolarWatchApp.Models;

namespace SolarWatchApp.DataServices.Repositories;

public class CityRepository: ICityRepository
{
    private readonly SolarWatchContext _solarWatchContext;

    public CityRepository(SolarWatchContext solarWatchContext)
    {
        _solarWatchContext = solarWatchContext;
    }


    public async Task<IEnumerable<City?>> GetAllCitiesAsync()
    {
       return await _solarWatchContext.Cities.ToListAsync();
    }

    public async Task<City?> GetCityByIdAsync(int id)
    {
        return await _solarWatchContext.Cities.FindAsync(id);
    }
    public async Task<City> GetCityByNameAsync(string name)
    {
        return await _solarWatchContext.Cities.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<City> CreateCityAsync(City city)
    {
        _solarWatchContext.Cities.Add(city);
        await _solarWatchContext.SaveChangesAsync();
        return city;
    }

    public async Task UpdateCityAsync(City city)
    {
        _solarWatchContext.Entry(city).State = EntityState.Modified;
       await _solarWatchContext.SaveChangesAsync();
    }

    public async Task DeleteCityAsync(int id)
    {
        var city = await _solarWatchContext.Cities.FindAsync(id);
        if (city != null)
        {
            _solarWatchContext.Cities.Remove(city);
            await _solarWatchContext.SaveChangesAsync();
        }
    }
}