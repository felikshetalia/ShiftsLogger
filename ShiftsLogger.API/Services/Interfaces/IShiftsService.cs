public interface IShiftsService
{
    ICollection<Shift> GetAllAsync();
    Shift? GetByIdAsync(int id);
    Shift CreateAsync(Shift dto);
    Shift UpdateAsync(int id, Shift dto);
    void DeleteAsync(int id);
}