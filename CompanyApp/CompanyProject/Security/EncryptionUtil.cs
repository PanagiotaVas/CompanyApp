namespace CompanyProject.Security
{
    public static class EncryptionUtil
    {
        public static string Encrypt(string clearText)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(clearText);
            return encryptedPassword;
        }

        public static bool IsValidPassword(string clearText, string encryptedText)
        {
            var isValid = BCrypt.Net.BCrypt.Verify(clearText, encryptedText);
            return isValid;
        }
    }

}
