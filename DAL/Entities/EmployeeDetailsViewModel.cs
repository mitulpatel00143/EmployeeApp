using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EmployeeDetailsViewModel
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoinee { get; set; }

    }

}
