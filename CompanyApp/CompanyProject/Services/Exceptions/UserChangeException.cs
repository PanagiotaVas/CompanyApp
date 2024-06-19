namespace CompanyProject.Services.Exceptions
{
    public class UserChangeException : Exception
    {
        public UserChangeException(string? message) : base(message)
        { }
    }
}
