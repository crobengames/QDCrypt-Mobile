using QDCrypt.Views;

namespace QDCrypt;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(EULAPage), typeof(EULAPage));
	}
}
