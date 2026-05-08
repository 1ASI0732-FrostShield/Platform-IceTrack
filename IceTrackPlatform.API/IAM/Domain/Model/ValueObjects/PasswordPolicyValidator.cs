using System.Text.RegularExpressions;

namespace IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;

public static class PasswordPolicyValidator
{
    private static readonly Regex HasUppercase    = new(@"[A-Z]");
    private static readonly Regex HasLowercase    = new(@"[a-z]");
    private static readonly Regex HasDigit        = new(@"\d");
    private static readonly Regex HasSpecialChar  = new(@"[!@#$%^&*(),.?""':{}|<>]");

    public static void Validate(string password)
    {
        var errors = new List<string>();

        if (password.Length < 8)
            errors.Add("at least 8 characters");

        if (!HasUppercase.IsMatch(password))
            errors.Add("at least one uppercase letter");

        if (!HasLowercase.IsMatch(password))
            errors.Add("at least one lowercase letter");

        if (!HasDigit.IsMatch(password))
            errors.Add("at least one number");

        if (!HasSpecialChar.IsMatch(password))
            errors.Add("at least one special character (!@#$%^&*...)");

        if (errors.Any())
            throw new Exception($"Password must contain: {string.Join(", ", errors)}");
    }
}