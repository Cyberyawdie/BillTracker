using SQLite;

namespace BillTracker.Services;

public class DataService
{
    private readonly SQLiteAsyncConnection _database;

    public DataService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Bill>();
        SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        var billsCount = await _database.Table<Bill>().CountAsync();
        if (billsCount == 0)
        {
            // List of seed bills
            var seedBills = new List<Bill>
            {
                new Bill { Name = "Electricity", Amount = 150.0M, OriginalDueDate = new DateTime(2023, 1, 15), CurrentDueDate = new DateTime(2023, 1, 15), IsPaid = false },
                new Bill { Name = "Water", Amount = 50.0M, OriginalDueDate = new DateTime(2023, 1, 20), CurrentDueDate = new DateTime(2023, 1, 20), IsPaid = false },
                // ... other seed bills ...
            };

            foreach (var bill in seedBills)
            {
                await _database.InsertAsync(bill);
            }
        }
    }

    public async Task<IEnumerable<Bill>> GetItemsAsync()
    {
        return await _database.Table<Bill>().ToListAsync();

    }
    public Task<int> AddBillAsync(Bill bill)
    {
        return _database.InsertAsync(bill);
    }
    public Task<int> UpdateBillAsync(Bill bill)
    {
        return _database.UpdateAsync(bill);
    }
    public Task<int> DeleteBillByIdAsync(int id)
    {
        return _database.Table<Bill>().DeleteAsync(b => b.Id == id);
    }
    public async Task DeleteBillsOlderThanAsync(DateTime cutoffDate)
    {
        var oldBills = await _database.Table<Bill>()
                                      .Where(b => b.CurrentDueDate < cutoffDate && b.IsPaid)
                                      .ToListAsync();

        foreach (var bill in oldBills)
        {
            await _database.DeleteAsync(bill);
        }
    }
    public async Task DeleteAllBillsAsync()
    {
        await _database.DeleteAllAsync<Bill>();
    }
    public async Task<List<string>> GetDistinctBillNamesAsync()
    {
        // Assuming you have a DbContext or similar data access setup
        // This is an example and might differ based on your database setup
        var bills = await _database.Table<Bill>().ToListAsync();

        return bills.Select(b => b.Name).Distinct().ToList();
    }

}
