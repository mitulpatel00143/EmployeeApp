using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface ILocationServices
    {
        List<Country> GetCountries();
        List<State> GetStatesByCountryId(int CountryId);
        List<City> GetCitiesByStateId(int StateId);
    }
}
