public sealed class ShiftsController
{
    private readonly IShiftsService _shiftsService;
    private readonly ShiftsView _view;

    public ShiftsController(IShiftsService shiftsService, ShiftsView view)
    {
        _shiftsService = shiftsService;
        _view = view;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            IReadOnlyCollection<ClientShift> shifts;
            try
            {
                shifts = await _shiftsService.GetAllAsync();
            }
            catch (ApiClientException exception)
            {
                _view.DisplayMessage(exception.Message);
                _view.WaitForInput();
                return;
            }

            _view.DisplayShifts(shifts);
            switch (_view.DisplayManagementMenu())
            {
                case ManagementMenuOption.Add:
                    await AddAsync();
                    break;

                case ManagementMenuOption.Update:
                    await UpdateAsync();
                    break;

                case ManagementMenuOption.Delete:
                    await DeleteAsync();
                    break;

                case ManagementMenuOption.Back:
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(ManagementMenuOption),
                        "Unknown shift menu option.");
            }
        }
    }

    private async Task AddAsync()
    {
        var request = _view.PromptShiftDetails();
        await RunOperationAsync(
            () => _shiftsService.CreateAsync(request),
            "Shift created.");
    }

    private async Task UpdateAsync()
    {
        var id = _view.PromptShiftId("update");
        ClientShift shift;
        try
        {
            shift = await _shiftsService.GetByIdAsync(id);
        }
        catch (ApiClientException exception)
        {
            _view.DisplayMessage(exception.Message);
            _view.WaitForInput();
            return;
        }

        var request = _view.PromptShiftDetails(shift);
        await RunOperationAsync(
            () => _shiftsService.UpdateAsync(id, request),
            "Shift updated.");
    }

    private async Task DeleteAsync()
    {
        var id = _view.PromptShiftId("delete");
        await RunOperationAsync(
            () => _shiftsService.DeleteAsync(id),
            "Shift deleted.");
    }

    private async Task RunOperationAsync(Func<Task> operation, string successMessage)
    {
        try
        {
            await operation();
            _view.DisplayMessage(successMessage);
        }
        catch (ApiClientException exception)
        {
            _view.DisplayMessage(exception.Message);
        }

        _view.WaitForInput();
    }
}
