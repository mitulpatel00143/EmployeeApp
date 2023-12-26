using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface IEmployee
    {
        int GetEmployeesCount();
        List<EmployeeDetailsViewModel> GetEmployeesDetails();
        List<EmployeeDetailsViewModel> GetEmployeesDetailsPage(string search, int sortColumn, string sortDirection, int pageNumber, int pageSize);

        Employee GetEmployeesDetailsByEmployeeCode(string employeeCode);


        void SaveEmployeeDetails(Employee employee);
        void UpdateEmployeeDetails(Employee employee);
        int SoftDeleteEmployee(string employeeCode);


        bool IsUniqueEmailAddress(string emailAddress, string employeeCode);
        bool IsUniqueMobileNumber(string mobileNumber, string employeeCode);
        bool IsUniquePanNumber(string panNumber, string employeeCode);
        bool IsUniquePassportNumber(string passportNumber, string employeeCode);
    }
}
