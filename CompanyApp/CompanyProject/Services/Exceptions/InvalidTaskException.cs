namespace CompanyProject.Services.Exceptions
{
    public class InvalidTaskException : Exception
    {
        public InvalidTaskException(string? error) : base(error)
        {
        }
    }
}
