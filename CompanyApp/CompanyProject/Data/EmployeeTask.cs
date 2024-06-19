namespace CompanyProject.Data
{
    public class EmployeeTask
    {
        public int EmployeeId { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        public virtual Task Task { get; set; } = null!;

        public virtual User User { get; set; } = null!;

    }
}
