namespace BillTracker.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class BillDetailPage : ContentPage
{
	Bill item;

	public Bill Item
	{
		get { return item; }
		set
		{
			item = value;

			DisplayedTitle.Text = item.Name;
			DisplayedDescription.Text = item.Description;
		}
	}

	public BillDetailPage()
	{
		InitializeComponent();
	}
}
