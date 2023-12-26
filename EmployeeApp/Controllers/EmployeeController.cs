using DAL.Abstract;
using DAL.Entities;
using DAL.Services;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;


namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ILogger<EmployeeController> _logger;
        private IEmployee _EmployeeServices { get; set; }
        private ILocationServices _LocationServices { get; set; }
        public EmployeeController(ILogger<EmployeeController> logger, IEmployee employee, ILocationServices locationServices)
        {
            _logger = logger;
            _EmployeeServices = employee;
            _LocationServices = locationServices;
        }



        /// <summary>
        /// get employee details on first page view from database
        /// </summary>
        /// <returns>Get View of first page data</returns>
        public IActionResult MainIndex()
        {
            return View();
        }

        /// <summary>
        /// get  employee details on specific page view from database
        /// </summary>
        /// <param name="model"> in this search, column, sorting order, pagenumber, pagesize pass by this model </param>
        /// <returns>Get Json of specific page data</returns>
        [HttpPost]
        public IActionResult GetData([FromBody] AjaxPostModel model)
        {
            try
            {
                var search = model?.Search?.Value;
                var sortColumn = model.Order?[0]?.Column ?? 0;
                var sortDirection = model.Order?[0]?.Dir ?? "ASC";
                var pageNumber = model.Start / model.Length + 1;
                var pageSize = model.Length;

                var employees = _EmployeeServices.GetEmployeesDetailsPage(search, sortColumn, sortDirection, pageNumber, pageSize);
                var empTotalCount = _EmployeeServices.GetEmployeesCount();

                return Json(new
                {
                    recordsTotal = empTotalCount,
                    recordsFiltered = empTotalCount,
                    data = employees
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred on your request." });
            }
        }


        /// <summary>
        ///  it will goes to insert employee view for inserting
        /// </summary>
        /// <returns> Go to Insert employee view</returns>
        public IActionResult SaveEmployee()
        {
            ViewBag.Countries = _LocationServices.GetCountries();
            return View();
        }

        /// <summary>
        /// get Employee object with that value, it is check validation of email, mobile, pan, passport, if validations are true than 
        /// it will add new employyee to database and also selected profile image will save to employee folder inside root folder.
        /// </summary>
        /// <param name="employee" > get employee object with values</param>
        /// <param name="profileImage" > selected image values</param>
        /// <returns> index view it will return</returns>
        [HttpPost]
        public IActionResult SaveEmployee(Employee employee, IFormFile profileImage)
        {

            try
            {
                ViewBag.Countries = _LocationServices.GetCountries();
                ViewBag.States = _LocationServices.GetStatesByCountryId(employee.CountryId);
                ViewBag.Cities = _LocationServices.GetCitiesByStateId(employee.StateId);

                if (!_EmployeeServices.IsUniqueEmailAddress(employee.EmailAddress, "1"))
                {
                    TempData["errorMessage"] = "Email Address should be Unique";
                    return View();
                }
                if (!_EmployeeServices.IsUniqueMobileNumber(employee.MobileNumber, "1"))
                {
                    TempData["errorMessage"] = "Mobile Number should be Unique";
                    return View();
                }
                if (!_EmployeeServices.IsUniquePanNumber(employee.PanNumber, "1"))
                {
                    TempData["errorMessage"] = "Pan Number should be Unique";
                    return View();
                }
                if (!_EmployeeServices.IsUniquePassportNumber(employee.PassportNumber, "1"))
                {
                    TempData["errorMessage"] = "Passport Number should be Unique";
                    return View();
                }

                if (profileImage != null && profileImage.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(profileImage.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads\\", "Employee");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profileImage.CopyTo(stream);
                    }

                    employee.ProfileImage = Path.Combine("Employee", uniqueFileName);
                }
                else
                {
                    employee.ProfileImage = Request.Form["ProfileImage"];
                }

                if (ModelState.IsValid)
                {
                    _EmployeeServices.SaveEmployeeDetails(employee);

                    TempData["SuccessMessage"] = "Employee details saved successfully.";
                    return RedirectToAction("MainIndex");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving employee details.";
                return View(employee);
            }
            

        }


        /// <summary>
        /// will get employee details by the employeecode and redirect to view
        /// </summary>
        /// <param name="employeeCode" > employeeCode for employee details</param>
        /// <returns> go to Edit employee view</returns>
        [HttpGet]
        public IActionResult EditEmployee(string employeeCode)
        {         
            if (string.IsNullOrEmpty(employeeCode))
            {
                return RedirectToAction("MainIndex");
            }

            Employee employee = _EmployeeServices.GetEmployeesDetailsByEmployeeCode(employeeCode);

            if (employee == null)
            {
                TempData["errorMessage"] = $"Employee details not found with Employee code : {employeeCode}";
                return RedirectToAction("MainIndex");
            }

            ViewBag.Countries = _LocationServices.GetCountries();
            ViewBag.States = _LocationServices.GetStatesByCountryId(employee.CountryId);
            ViewBag.Cities = _LocationServices.GetCitiesByStateId(employee.StateId);
            ViewBag.ProfileImage = employee.ProfileImage;

            return View(employee);
            
        }

        /// <summary>
        /// get updated Employee object with that value, it is check validation of email, mobile, pan, passport, if validations are true than 
        /// it will add new employyee to database and also selected profile image will save to employee folder inside root folder.
        /// </summary>
        /// <param name="employee" > get updated employee object with values</param>
        /// <param name="profileImage" > updated selected image values</param>
        /// <returns> index view it will return</returns>
        [HttpPost]
        public IActionResult EditEmployee(Employee employee, IFormFile profileImage)
        {
            try
            {

                ViewBag.Countries = _LocationServices.GetCountries();
                ViewBag.States = _LocationServices.GetStatesByCountryId(employee.CountryId);
                ViewBag.Cities = _LocationServices.GetCitiesByStateId(employee.StateId);

                if (profileImage != null && profileImage.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(profileImage.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads\\", "Employee");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profileImage.CopyTo(stream);
                    }

                    employee.ProfileImage = Path.Combine("Employee", uniqueFileName);
                }
                else
                {
                    employee.ProfileImage = Request.Form["ProfileImage"];
                }

                if (!_EmployeeServices.IsUniqueEmailAddress(employee.EmailAddress ,employee.EmployeeCode))
                {
                    TempData["errorMessage"] = "Email Address should be Unique";
                    return View(employee);
                }
                if (!_EmployeeServices.IsUniqueMobileNumber(employee.MobileNumber, employee.EmployeeCode))
                {
                    TempData["errorMessage"] = "Mobile Number should be Unique";
                    return View(employee);
                }
                if (!_EmployeeServices.IsUniquePanNumber(employee.PanNumber, employee.EmployeeCode))
                {
                    TempData["errorMessage"] = "Pan Number should be Unique";
                    return View(employee);
                }
                if (!_EmployeeServices.IsUniquePassportNumber(employee.PassportNumber, employee.EmployeeCode))
                {
                    TempData["errorMessage"] = "Passport Number should be Unique";
                    return View(employee);
                }
                ModelState.Remove("ProfileImage");
                if (ModelState.IsValid)
                {
                    _EmployeeServices.UpdateEmployeeDetails(employee);

                    TempData["successMessage"] = "Employee details updated successfully";
                    return RedirectToAction("MainIndex");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(employee);
            }

        }

        /// <summary>
        /// for getting unique file name of selected profile image
        /// </summary>
        /// <param name="fileName" > selected profile image file name </param>
        /// <returns> Unique file name if profile image</returns>
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }


        /// <summary>
        /// will delete employee details by the employeecode in database and redirect to view
        /// </summary>
        /// <param name="employeeCode" > employeeCode for employee details</param>
        /// <returns> go to Index employee </returns>
        [HttpDelete]
        public IActionResult DeleteEmployee(string employeeCode)
        {
            var result = _EmployeeServices.SoftDeleteEmployee(employeeCode);

            if (result == 1)
            {
                return Json(new { success = true, message = "Employee deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete employee." });
            }
        }


        /// <summary>
        /// get list of state using country Id
        /// </summary>
        /// <param name="countryId"> id of selected country</param>
        /// <returns>list of state in json format</returns>
        [HttpPost]
        public IActionResult GetStatesByCountryId(int countryId)
        {
            var states = _LocationServices.GetStatesByCountryId(countryId);
            return Json(states);
        }

        /// <summary>
        /// get list of city using state Id
        /// </summary>
        /// <param name="stateId"> id of selected state</param>
        /// <returns>list of city in json format</returns>
        [HttpPost]
        public IActionResult GetCitiesByStateId(int stateId)
        {
            var cities = _LocationServices.GetCitiesByStateId(stateId);
            return Json(cities);
        }

        /// <summary>
        /// check Email address is unique or not
        /// </summary>
        /// <param name="emailAddress"> email address for checking</param>
        /// <param name="employeeCode"> employee code of employee</param>
        /// <returns>return true of false in json format</returns>
        public IActionResult IsUniqueEmailAddress(string emailAddress, string employeeCode)
        {
            bool isUnique = _EmployeeServices.IsUniqueEmailAddress(emailAddress, employeeCode);
            return Json(isUnique);
        }

        /// <summary>
        /// check Mobile number is unique or not
        /// </summary>
        /// <param name="mobileNumber"> mobile number for checking</param>
        /// <param name="employeeCode"> employee code of employee</param>
        /// <returns>return true of false in json format</returns>
        public IActionResult IsUniqueMobileNumber(string mobileNumber, string employeeCode)
        {
            bool isUnique = _EmployeeServices.IsUniqueMobileNumber(mobileNumber, employeeCode);
            return Json(isUnique);
        }
        
        /// <summary>
        /// check Pan number is unique or not
        /// </summary>
        /// <param name="panNumber"> pan number for checking</param>
        /// <param name="employeeCode"> employee code of employee</param>
        /// <returns>return true of false in json format</returns>
        public IActionResult IsUniquePanNumber(string panNumber, string employeeCode)
        {
            bool isUnique = _EmployeeServices.IsUniquePanNumber(panNumber, employeeCode);
            return Json(isUnique);
        }

        /// <summary>
        /// check Passport number is unique or not
        /// </summary>
        /// <param name="passportNumber"> passport number for checking</param>
        /// <param name="employeeCode"> employee code of employee</param>
        /// <returns>return true of false in json format</returns>
        public IActionResult IsUniquePassportNumber(string passportNumber, string employeeCode)
        {
            bool isUnique = _EmployeeServices.IsUniquePassportNumber(passportNumber, employeeCode);
            return Json(isUnique);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
