namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employee) 
            : base($"The employee with id: {employee} doesn't exist in the database.") 
        { 
        }
    }
}
