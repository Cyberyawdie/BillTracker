
namespace BillTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bills.db");
        builder.Services.AddSingleton(new DataService(dbPath));

        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<BillDetailPage>();
		builder.Services.AddTransient<AddBillPage>();

        builder.Services.AddSingleton<BillsPage>();
        builder.Services.AddSingleton<BillHistoryPage>();
        builder.Services.AddSingleton<AccountPage>();

        return builder.Build();
	}
}
