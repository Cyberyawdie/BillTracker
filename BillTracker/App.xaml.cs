namespace BillTracker;

public partial class App : Application
{
    readonly DataService dataService;

    public App(DataService service)
	{
		InitializeComponent();

        MainPage = new AppShell();
        dataService = service;
        PerformDataCleanup();

    }

    private int GetDataStorageDuration()
    {
        // If the preference is not set, default to 1 year
        return Preferences.Get("DataStorageDuration", 1);
    }

    private async void PerformDataCleanup()
    {
        var dataStorageDurationInYears = GetDataStorageDuration();
        var cutoffDate = DateTime.Now.AddYears(-dataStorageDurationInYears);

        await dataService.DeleteBillsOlderThanAsync(cutoffDate);
    }

}
