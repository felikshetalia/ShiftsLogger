
using Microsoft.AspNetCore.Http.HttpResults;

public class ShiftsService : IShiftsService
{
    private readonly ShiftsLoggerDbContext _shiftsCtx;
    public ShiftsService(ShiftsLoggerDbContext ctx)
    {
        _shiftsCtx = ctx;
    }
    public Shift CreateAsync(Shift dto)
    {
        var saved = _shiftsCtx.Shifts.Add(dto);
        _shiftsCtx.SaveChanges();
        return saved.Entity;
    }

    public void DeleteAsync(int id)
    {
        var saved = _shiftsCtx.Shifts.Find(id);
        if (saved == null)
        {
            // throw new NotFound
            return;
        }

        _shiftsCtx.Shifts.Remove(saved);
        _shiftsCtx.SaveChanges();
    }

    public ICollection<Shift> GetAllAsync()
        => _shiftsCtx.Shifts.ToList();

    public Shift? GetByIdAsync(int id)
        => _shiftsCtx.Shifts.Find(id);

    public Shift UpdateAsync(int id, Shift dto)
    {
        var saved = _shiftsCtx.Shifts.Find(id);
        if (saved == null)
            return null!;

        _shiftsCtx.Entry(saved).CurrentValues.SetValues(dto);
        _shiftsCtx.SaveChanges();

        return saved;
    }
}