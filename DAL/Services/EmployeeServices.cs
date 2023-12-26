using DAL.Abstract;
using DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class EmployeeServices : IEmployee
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _UnitOfWork;
        public EmployeeServices(ILogger<EmployeeServices> logger, IUnitOfWork unitofwork)
        {
            _logger = logger;
            _UnitOfWork = unitofwork;
        }


        /// <summary>
        /// Get number of total employees from database EmployeeMaster table
        /// </summary>
        public int GetEmployeesCount()
        {

            var result = _UnitOfWork.GetDataSet("stp_emp_GetEmployeeCount");
            return Convert.ToInt32(result.Tables[0].Rows[0]["EmpCount"]);
        }
        /// <summary>
        /// Get list of all employees from database EmployeeMaster table
        /// </summary>
        public List<EmployeeDetailsViewModel> GetEmployeesDetails()
        {

            DataSet dataSet = _UnitOfWork.GetDataSet("stp_emp_GetAllEmployees");
            List<EmployeeDetailsViewModel> employees = new List<EmployeeDetailsViewModel>();

            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {

                EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel()
                {
                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                    FirstName = Convert.ToString(dr["FirstName"]),
                    LastName = Convert.ToString(dr["LastName"]),
                    CountryName = Convert.ToString(dr["CountryName"]),
                    StateName = Convert.ToString(dr["StateName"]),
                    CityName = Convert.ToString(dr["CityName"]),
                    EmailAddress = Convert.ToString(dr["EmailAddress"]),
                    MobileNumber = Convert.ToString(dr["MobileNumber"]),
                    PanNumber = Convert.ToString(dr["PanNumber"]),
                    PassportNumber = Convert.ToString(dr["PassportNumber"]),
                    ProfileImage = Convert.ToString(dr["ProfileImage"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    ActiveStatus = Convert.ToString(dr["ActiveStatus"]),
                };
                employees.Add(employeeDetails);
            }

            return employees;
        }
        /// <summary>
        /// Get list of employees on one page from database EmployeeMaster table
        /// </summary>
        public List<EmployeeDetailsViewModel> GetEmployeesDetailsPage(string search, int sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@SearchTerm", (object)search ?? DBNull.Value),
                new SqlParameter("@SortColumn", GetSortColumnName(sortColumn)),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize)
            };

            List<EmployeeDetailsViewModel> employees = new List<EmployeeDetailsViewModel>();
            try
            {
                DataSet dataSet = _UnitOfWork.GetDataSet("stp_emp_GetAllEmployees", parameters);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel()
                    {
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                        CountryName = Convert.ToString(dr["CountryName"]),
                        StateName = Convert.ToString(dr["StateName"]),
                        CityName = Convert.ToString(dr["CityName"]),
                        EmailAddress = Convert.ToString(dr["EmailAddress"]),
                        MobileNumber = Convert.ToString(dr["MobileNumber"]),
                        PanNumber = Convert.ToString(dr["PanNumber"]),
                        PassportNumber = Convert.ToString(dr["PassportNumber"]),
                        ProfileImage = Convert.ToString(dr["ProfileImage"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        ActiveStatus = Convert.ToString(dr["ActiveStatus"]),
                    };

                    employees.Add(employeeDetails);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetEmployeesDetailsPage");
            }

            return employees;
        }
        /// <summary>
        /// for select column name for sorting
        /// </summary>
        private string GetSortColumnName(int sortColumn)
        {
            switch (sortColumn)
            {
                case 0: return "FirstName";
                case 1: return "LastName";
                case 2: return "EmailAddress";
                case 3: return "CountryName";
                case 4: return "StateName";
                case 5: return "CityName";
                case 6: return "PanNumber";
                case 7: return "PassportNumber";
                case 8: return "Gender";
                case 9: return "ActiveStatus";
                case 10: return "ProfileImage";
                default: return "FirstName";
            }
        }

        /// <summary>
        /// Get details  employee by employee code from database EmployeeMaster table
        /// </summary>
        public Employee GetEmployeesDetailsByEmployeeCode(string employeeCode)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeCode", employeeCode)
            };

            DataSet dataSet = _UnitOfWork.GetDataSet("stp_emp_GetEmployeeByEmployeeCode", sqlParameters);
            Employee employeeDetails = new Employee();

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dataSet.Tables[0].Rows[0]; 

                employeeDetails.EmployeeCode = Convert.ToString(dr["EmployeeCode"]);
                employeeDetails.FirstName = Convert.ToString(dr["FirstName"]);
                employeeDetails.LastName = Convert.ToString(dr["LastName"]);
                employeeDetails.CountryId = Convert.ToInt32(dr["CountryId"]);
                employeeDetails.StateId = Convert.ToInt32(dr["StateId"]);
                employeeDetails.CityId = Convert.ToInt32(dr["CityId"]);
                employeeDetails.EmailAddress = Convert.ToString(dr["EmailAddress"]);
                employeeDetails.MobileNumber = Convert.ToString(dr["MobileNumber"]);
                employeeDetails.PanNumber = Convert.ToString(dr["PanNumber"]);
                employeeDetails.PassportNumber = Convert.ToString(dr["PassportNumber"]);
                employeeDetails.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).Date;
                employeeDetails.DateOfJoinee = Convert.ToDateTime(dr["DateOfJoinee"]).Date;
                employeeDetails.ProfileImage = Convert.ToString(dr["ProfileImage"]);
                employeeDetails.Gender = Convert.ToInt32(dr["Gender"]);
                employeeDetails.ActiveStatus = Convert.ToBoolean(dr["IsActive"]);
            }

            return employeeDetails;
        }

        /// <summary>
        /// save employee details into database EmployeeMaster table
        /// </summary>
        public void SaveEmployeeDetails(Employee employee)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@CountryId", employee.CountryId),
                new SqlParameter("@StateId", employee.StateId),
                new SqlParameter("@CityId", employee.CityId),
                new SqlParameter("@EmailAddress", employee.EmailAddress),
                new SqlParameter("@MobileNumber", employee.MobileNumber),
                new SqlParameter("@PanNumber", employee.PanNumber.ToUpper()),
                new SqlParameter("@PassportNumber", employee.PassportNumber.ToUpper()),
                new SqlParameter("@DateOfBirth", employee.DateOfBirth),
                new SqlParameter("@DateOfJoinee", employee.DateOfJoinee),
                new SqlParameter("@ProfileImage", employee.ProfileImage),
                new SqlParameter("@Gender", employee.Gender),
                new SqlParameter("@IsActive", employee.ActiveStatus)
            };
            _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_emp_InsertEmployee", parameters, false);
        }

        /// <summary>
        /// update employee details  using employyecode into database EmployeeMaster table
        /// </summary>
        public void UpdateEmployeeDetails(Employee employee)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeCode", employee.EmployeeCode),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@CountryId", employee.CountryId),
                new SqlParameter("@StateId", employee.StateId),
                new SqlParameter("@CityId", employee.CityId),
                new SqlParameter("@EmailAddress", employee.EmailAddress),
                new SqlParameter("@MobileNumber", employee.MobileNumber),
                new SqlParameter("@PanNumber", employee.PanNumber.ToUpper()),
                new SqlParameter("@PassportNumber", employee.PassportNumber.ToUpper()),
                new SqlParameter("@DateOfBirth", employee.DateOfBirth),
                new SqlParameter("@DateOfJoinee", employee.DateOfJoinee),
                new SqlParameter("@ProfileImage", employee.ProfileImage),
                new SqlParameter("@Gender", employee.Gender),
                new SqlParameter("@IsActive", employee.ActiveStatus)
            };
            _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_emp_UpdateEmployee", parameters, false);
        }

        /// <summary>
        /// delete employee details using employyecode from database EmployeeMaster table
        /// </summary>
        public int SoftDeleteEmployee(string employeeCode)
        {
            int flag = 0;

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@EmployeeCode", employeeCode)
                };

                _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_emp_DeleteEmployee", sqlParameters, true);
                flag = 1;
            }
            catch (Exception ex)
            {
                flag = 0;
            }

            return flag;
        }


        /// <summary>
        /// check given email address is available in database EmployeeMaster table for checking uniqueness
        /// </summary>
        public bool IsUniqueEmailAddress(string emailAddress, string employeeCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", emailAddress),
                new SqlParameter("@EmployeeCode", employeeCode)
            };

            var result = _UnitOfWork.GetDataSet("stp_emp_IsUniqueEmailAddress", parameters);
            return Convert.ToBoolean(result.Tables[0].Rows[0]["IsUnique"]);
        }
        /// <summary>
        /// check given mobile number is available in database EmployeeMaster table for checking uniqueness
        /// </summary>
        public bool IsUniqueMobileNumber(string mobileNumber, string employeeCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MobileNumber", mobileNumber),
                new SqlParameter("@EmployeeCode", employeeCode)
            };

            var result = _UnitOfWork.GetDataSet("stp_emp_IsUniqueMobileNumber", parameters);
            return Convert.ToBoolean(result.Tables[0].Rows[0]["IsUnique"]);
        }
        /// <summary>
        /// check given pan number is available in database EmployeeMaster table for checking uniqueness
        /// </summary>
        public bool IsUniquePanNumber(string panNumber, string employeeCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PanNumber", panNumber),
                new SqlParameter("@EmployeeCode", employeeCode)
            };                                                          

            var result = _UnitOfWork.GetDataSet("stp_emp_IsUniquePanNumber", parameters);
            return Convert.ToBoolean(result.Tables[0].Rows[0]["IsUnique"]);
        }
        /// <summary>
        /// check given passport number is available in database EmployeeMaster table for checking uniqueness
        /// </summary>
        public bool IsUniquePassportNumber(string passportNumber, string employeeCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PassportNumber", passportNumber),
                new SqlParameter("@EmployeeCode", employeeCode)
            };

            var result = _UnitOfWork.GetDataSet("stp_emp_IsUniquePassportNumber", parameters);
            return Convert.ToBoolean(result.Tables[0].Rows[0]["IsUnique"]);
        }       


    }
}
