using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftsService _shiftsService;

    public ShiftsController(IShiftsService shiftsService)
    {
        _shiftsService = shiftsService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Shift>>> GetAll()
        => Ok(await _shiftsService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Shift>> GetById(int id)
    {
        var shift = await _shiftsService.GetByIdAsync(id);
        return shift is null ? NotFound() : Ok(shift);
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> Create(CreateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var createdShift = await _shiftsService.CreateAsync(shift);
        if (createdShift is null)
            return BadRequest("The employee does not exist or is not active.");

        return CreatedAtAction(nameof(GetById), new { id = createdShift.Id }, createdShift);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Shift>> Update(int id, UpdateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var updatedShift = await _shiftsService.UpdateAsync(id, shift);
        return updatedShift is null ? NotFound() : Ok(updatedShift);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _shiftsService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
