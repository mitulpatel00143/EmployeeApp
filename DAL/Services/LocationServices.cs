using DAL.Abstract;
using DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LocationServices: ILocationServices
    {
        private readonly ILogger Logger;
        private readonly IUnitOfWork _UnitOfWork;
        public LocationServices(ILogger<LocationServices> logger, IUnitOfWork unitofwork)
        {
            Logger = logger;
            _UnitOfWork = unitofwork;
        }

        /// <summary>
        /// get list of country details from database country table
        /// </summary>
        public List<Country> GetCountries()
        {
            
            DataSet dataSet = _UnitOfWork.GetDataSet("stp_emp_GetCountries");
            List<Country> countries = new List<Country>();

            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {

                Country objCountry = new Country()
                {
                    CountryId = Convert.ToInt32(dr["Row_Id"]),
                    CountryName = Convert.ToString(dr["CountryName"]), 
                };
                countries.Add(objCountry);
            }

            return countries;
        }

        /// <summary>
        /// get list of state details using countryId from database state table 
        /// </summary>
        public List<State> GetStatesByCountryId(int countryId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>() { 
                new SqlParameter("@CountryId",countryId)
            };

            DataSet dataSet = _UnitOfWork.GetDataSet("stp_emp_GetStatesByCountryId", sqlParameters);
            List<State> states = new List<State>();

            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                State state = new State() {
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    StateId = Convert.ToInt32(dr["Row_Id"]),
                    StateName = Convert.ToString(dr["StateName"]),
                };
                states.Add(state);
            }
            return states;
        }

        /// <summary>
        /// get list of city details using stateId from database city table
        /// </summary>
        public List<City> GetCitiesByStateId(int StateId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>() { 
                new SqlParameter("@StateId", StateId) 
            };

            DataSet data = _UnitOfWork.GetDataSet("stp_emp_GetCitiesByStateId", sqlParameters);
            List<City> cities = new List<City>();
            
            foreach(DataRow dr in data.Tables[0].Rows)
            {
                City city = new City() {
                    StateId = Convert.ToInt32(dr["StateId"]),
                    CityId = Convert.ToInt32(dr["Row_Id"]),
                    CityName= Convert.ToString(dr["CityName"]),
                };
                cities.Add(city);
            }

            return cities;
        }

    }
}
