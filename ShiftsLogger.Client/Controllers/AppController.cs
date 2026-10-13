namespace ShiftsLogger.Client;

public sealed class AppController
{
    private readonly IAppView _appView;
    public AppController(IAppView view)
    {
        _appView = view;
    }

    public async Task Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            MainMenuOption selectedOption = _appView.DisplayMainMenu();

            switch (selectedOption)
            {
                case MainMenuOption.EnterShift:
                    break;

                case MainMenuOption.Exit:
                    isRunning = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(selectedOption),
                        selectedOption,
                        "Unknown menu option.");
            }
        }
        _appView.DisplayGoodbye();
    }
}