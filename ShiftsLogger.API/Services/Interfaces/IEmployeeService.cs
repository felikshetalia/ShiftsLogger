public interface IEmployeeService
{
    Task<ICollection<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee dto);
    Task<Employee> UpdateAsync(int id, Employee dto);
    Task DeleteAsync(int id);
}