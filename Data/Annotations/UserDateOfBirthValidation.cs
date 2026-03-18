using System.ComponentModel.DataAnnotations;

namespace InterviewApiDemo.Data.Annotations;

public class UserDateOfBirthValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var valueString = value != null ? value.ToString() : null;

        // If value is empty, show Success
        if (string.IsNullOrWhiteSpace(valueString))
        {
            return ValidationResult.Success;
        }

        // If dob cannot be parsed in Date then it is invalid
        if (!DateTime.TryParse(valueString, out DateTime dob))
        {
            return new ValidationResult("Please provide the date of birth in a valid format");
        }

        // Check if the age is under the minimum age. finding the calendar date of 16 years ago
        var minDateOfBirth = DateTime.Now.Date.AddYears(Constants.MinimunUserAge * -1);

        if (dob > minDateOfBirth)
        {
            return new ValidationResult($"The user age must be greater or equal to {Constants.MinimunUserAge} years");
        }

        return ValidationResult.Success;
    }
}
