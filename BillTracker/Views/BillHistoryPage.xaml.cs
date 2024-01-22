namespace BillTracker.Views;

public partial class BillHistoryPage : ContentPage
{
    readonly DataService dataService;

    public BillHistoryPage(DataService service)
    {
        InitializeComponent();
        dataService = service;
        LoadBills();
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        LoadBills();
    }
    private async void LoadBills()
    {
        var bills = await dataService.GetItemsAsync(); // Fetch bills
        var currentYear = DateTime.Now.Year;

        // Grouping by year and then by month
        var groupedBills = bills.Where(b => b.CurrentDueDate.Year <= currentYear)
                                .GroupBy(b => b.CurrentDueDate.Year)
                                .Select(yearGroup => new YearGroup
                                {
                                    Year = yearGroup.Key,
                                    Months = yearGroup.GroupBy(b => b.CurrentDueDate.Month)
                                                      .Select(monthGroup => new MonthGroup
                                                      {
                                                          MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                                                          Bills = new ObservableCollection<Bill>(monthGroup.OrderBy(b => b.CurrentDueDate))
                                                      })
                                                      .OrderBy(mg => mg.MonthName)
                                                      .ToList()
                                })
                                .OrderByDescending(yg => yg.Year)
                                .ToList();

        BillsCollectionView.ItemsSource = groupedBills;
    }

    // YearGroup and MonthGroup class definitions
    public class YearGroup
    {
        public int Year { get; set; }
        public List<MonthGroup> Months { get; set; }
    }

    public class MonthGroup
    {
        public string MonthName { get; set; }
        public ObservableCollection<Bill> Bills { get; set; }
    }
}