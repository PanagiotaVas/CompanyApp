namespace CompanyProject.Services.Exceptions
{
    public class EmployeeAlreadyExistsException : Exception
    {
        public EmployeeAlreadyExistsException(string s) : base(s)
        { }
    }
}
