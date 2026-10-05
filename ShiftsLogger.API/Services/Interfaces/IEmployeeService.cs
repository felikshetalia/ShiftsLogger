public interface IEmployeeService
{
    ICollection<Employee> GetAllAsync();
    Employee? GetByIdAsync(int id);
    Employee CreateAsync(Employee dto);
    Employee UpdateAsync(int id, Employee dto);
    void DeleteAsync(int id);
}