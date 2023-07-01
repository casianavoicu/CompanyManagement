namespace CompanyManagement.Common.Dto
{
    public class EmployeeDto : EmployeeLightDto
    {
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class EmployeeLightDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Salary { get; set; }

        public string? Email { get; set; }

        public DepartamentEnum Departament { get; set; }
    }


    public enum DepartamentEnum
    {
        IT,
        Marketing
    }

    public enum EquipmentEnum
    {
        Laptop,
        PC
    }
}
