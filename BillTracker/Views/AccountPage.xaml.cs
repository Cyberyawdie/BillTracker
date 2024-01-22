namespace BillTracker.Views;

public partial class AccountPage : ContentPage
{
    readonly DataService dataService;

    public AccountPage(DataService service)
    {
        InitializeComponent();
        dataService = service;
        InitializePicker();
    }

    private void InitializePicker()
    {
        // Set the default value for the picker based on current settings
        // Assuming you store this setting in local preferences or similar
        var currentSetting = GetDataStorageDuration(); // Get current setting
        DataStorageDurationPicker.SelectedIndex = currentSetting - 1; // Adjust based on how you store this setting
    }

    private async void OnDeleteHistoryClicked(object sender, EventArgs e)
    {
        bool confirmed = await DisplayAlert("Confirm", "Delete all bill history?", "Yes", "No");
        if (confirmed)
        {
            await dataService.DeleteAllBillsAsync(); // Implement this method in your database service
            // Additional logic if needed
        }
    }

    private void OnDurationChanged(object sender, EventArgs e)
    {
        var selectedDuration = DataStorageDurationPicker.SelectedIndex + 1; // Get selected duration
        // Save this setting to your preferences or settings
        SaveDataStorageDuration(selectedDuration);
    }

    private async void OnClearDataClicked(object sender, EventArgs e)
    {
        bool isConfirmed = await DisplayAlert("Confirm", "Are you sure you want to delete all bill data?", "Yes", "No");
        if (isConfirmed)
        {
            await dataService.DeleteAllBillsAsync();
            await Shell.Current.GoToAsync(nameof(MainPage));
            // Additional logic after deletion (e.g., updating UI)
        }
    }

    private void SaveDataStorageDuration(int years)
    {
        Preferences.Set("DataStorageDuration", years);
    }

    private int GetDataStorageDuration()
    {
        // If the preference is not set, default to 1 year
        return Preferences.Get("DataStorageDuration", 1);
    }


}