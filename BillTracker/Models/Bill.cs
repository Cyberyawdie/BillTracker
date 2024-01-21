using SQLite;

namespace BillTracker.Models;

public class Bill
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Amount { get; set; }

    public DateTime OriginalDueDate { get; set; }

    public DateTime CurrentDueDate { get; set; }

    public bool IsPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Description { get; set; }
}
