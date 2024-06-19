namespace CompanyProject.Services.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string? message) : base(message)
        { }
    }
}
