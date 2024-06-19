namespace CompanyProject.Services.Exceptions
{
    public class TaskAlreadyExistsException : Exception
    {
        public TaskAlreadyExistsException(string? message) : base(message)
        { }
    }
}
