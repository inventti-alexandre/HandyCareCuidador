using System.Collections.Generic;
using PropertyChanged;

namespace HandyCareCuidador.Helper
{
    [ImplementPropertyChanged]
    public class Geoname
    {
        public string adminCode1 { get; set; }
        public string lng { get; set; }
        public int geonameId { get; set; }
        public string toponymName { get; set; }
        public string countryId { get; set; }
        public string fcl { get; set; }
        public int population { get; set; }
        public int numberOfChildren { get; set; }
        public string countryCode { get; set; }
        public string name { get; set; }
        public string fclName { get; set; }
        public string countryName { get; set; }
        public string fcodeName { get; set; }
        public string adminName1 { get; set; }
        public string lat { get; set; }
        public string fcode { get; set; }
    }

    public class RootObject
    {
        public int totalResultsCount { get; set; }
        public List<Geoname> geonames { get; set; }
    }
}
