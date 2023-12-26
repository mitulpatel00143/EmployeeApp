using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DAL.Entities.Attributes
{

    public class DateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfJoinee)
            {   
                if (dateOfJoinee > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "Date of Joining cannot be in the future.");
                }

            }

            return ValidationResult.Success;
        }
    }

    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                int age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                {
                    age--;
                }
                if (age < 18)
                {
                    return new ValidationResult(ErrorMessage ?? "Date of Birth must 18+ Age.");
                }

                if (dateOfBirth > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "Date of Birth cannot be in the future.");
                }

            }

            return ValidationResult.Success;
        }
    }



    public class AgeAndDateOfJoiningAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public AgeAndDateOfJoiningAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                var dateOfBirthProperty = validationContext.ObjectType.GetProperty("DateOfBirth");
                var dateOfBirthValue = dateOfBirthProperty.GetValue(validationContext.ObjectInstance, null);

                if (dateOfBirthValue is DateTime dateOfBirth)
                {
                    var age = CalculateAge(dateOfBirth, date);

                    if (age < _minAge)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }

        private int CalculateAge(DateTime dateOfBirth, DateTime currentDate)
        {
            int age = currentDate.Year - dateOfBirth.Year;

            if (currentDate < dateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }

}
