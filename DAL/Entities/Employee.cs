using DAL.Entities.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entities
{
    public class Employee
    {
        public string? EmployeeCode { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(100, ErrorMessage = "Email Address cannot exceed 30 characters.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9-]{2,}(\\.[a-zA-Z]{2,5})+$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [MaxLength(12, ErrorMessage = "Mobile Number cannot exceed 12 characters.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNumber { get; set; }

        [DisplayName("Pan Number")]
        [Required(ErrorMessage = "Pan Number is required.")]
        [MaxLength(12, ErrorMessage = "Pan Number cannot exceed 12 characters.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid Pan Number.")]
        //[Remote("IsUniquePanNumber", "Employee", ErrorMessage = "Pan Number must be unique.")]
        public string PanNumber { get; set; }

        [DisplayName("Passport Number")]
        [Required(ErrorMessage = "Passport Number is required.")]
        [MaxLength(20, ErrorMessage = "Passport Number cannot exceed 12 characters.")]
        [RegularExpression(@"^[A-PR-WY][1-9]\d\s?\d{4}[1-9]$", ErrorMessage = "Invalid Passport Number.")]
        //[Remote("IsUniquePassportNumber", "Employee", ErrorMessage = "Passport Number must be unique.")]
        public string PassportNumber { get; set; }

        [DisplayName("Profile Picture")]
        public string? ProfileImage { get; set; }

        public int Gender { get; set; }

        [DisplayName("Active")]
        public bool ActiveStatus { get; set; }

        [DisplayName("Date Of Birth")]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateOfBirthValidation(ErrorMessage = "Age must be above 18.")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Date Of Joinee")]
        [Required(ErrorMessage = "Date of Joinee is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateValidation(ErrorMessage = "Date of Joinee should be less than today's date.")]
        [AgeAndDateOfJoining(18, ErrorMessage = "Employee must be above 18")]
        public DateTime DateOfJoinee { get; set; }
    }

    public class AjaxPostModel
    {
        public int Draw { get; set; }
        public AjaxRequestColumns[] Columns { get; set; }
        public AjaxRequestOrder[] Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataSearch Search { get; set; }
    }

    public class AjaxRequestColumns
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataSearch Search { get; set; }
    }

    public class AjaxRequestOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class DataSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

}

