using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace AspNetDating.Services
{
    [ServiceContract]
    public interface ICountriesService
    {
        [OperationContract]
        string GetCountryByCode(string code);

        [OperationContract]
        string[] GetCountries();

        [OperationContract]
        string[] GetRegions(string country);

        [OperationContract]
        string[] GetCities(string country, string region);
    }
}