using Demo.Domain.Models;

namespace Demo.Domain.Validators;

// This can be transformed into instance class and give it a translation service to return the error messages in the user language. For simplicity, I am keeping it as static class and hardcoding the error messages in English.
public static class UserValidator
{
    public const int MinimunUserAge = 16;

    public static string[] Validate(User user, DateTime now)
    {
        var errors = new List<string>();
        errors.AddRange(ValidateUsername(user));
        errors.AddRange(ValidateName(user));
        errors.AddRange(ValidateDateOfBirth(user, now));
        return errors.ToArray();
    }

    public static string[] ValidateUsername(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            return ["The 'Username' is required"];
        }

        if (user.Username.Length < 5 || user.Username.Length > 50)
        {
            return ["The 'Username' has to be between 5 and 50 characters long"];
        }

        return [];
    }

    public static string[] ValidateName(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return ["The 'Name' is required"];
        }

        if (user.Name.Length < 2 || user.Name.Length > 80)
        {
            return ["The 'Name' has to be between 2 and 80 characters long"];
        }

        return [];
    }

    public static string[] ValidateDateOfBirth(User user, DateTime now)
    {
        // Check if the age is under the minimum age. finding the calendar date of 16 years ago
        var minDateOfBirth = DateOnly.FromDateTime(now.Date.AddYears(MinimunUserAge * -1));

        if (user.DateOfBirth > minDateOfBirth)
        {
            return [$"The user age must be greater or equal to {MinimunUserAge} years"];
        }

        return [];
    }
}