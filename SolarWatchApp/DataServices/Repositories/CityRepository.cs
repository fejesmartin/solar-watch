using Microsoft.EntityFrameworkCore;
using SolarWatchApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarWatchApp.DataServices.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly SolarWatchContext _solarWatchContext;

        public CityRepository(SolarWatchContext solarWatchContext)
        {
            _solarWatchContext = solarWatchContext ?? throw new ArgumentNullException(nameof(solarWatchContext));
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            try
            {
                return await _solarWatchContext.Cities.ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle or log the exception, depending on your application's requirements
                Console.WriteLine($"Error in GetAllCitiesAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            try
            {
                return await _solarWatchContext.Cities.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCityByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<City> GetCityByNameAsync(string name)
        {
            try
            {
                return await _solarWatchContext.Cities.FirstOrDefaultAsync(c => c.Name == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCityByNameAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<City> CreateCityAsync(City city)
        {
            try
            {
                _solarWatchContext.Cities.Add(city);
                await _solarWatchContext.SaveChangesAsync();
                return city;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCityAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateCityAsync(City city)
        {
            try
            {
                _solarWatchContext.Entry(city).State = EntityState.Modified;
                await _solarWatchContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCityAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCityAsync(int id)
        {
            try
            {
                var city = await _solarWatchContext.Cities.FindAsync(id);
                if (city != null)
                {
                    _solarWatchContext.Cities.Remove(city);
                    await _solarWatchContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCityAsync: {ex.Message}");
                throw;
            }
        }
    }
}
