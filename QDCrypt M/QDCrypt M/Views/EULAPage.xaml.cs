using QDCrypt.Models;

namespace QDCrypt.Views;

public partial class EULAPage : ContentPage
{
    public EULAPage()
    {
        InitializeComponent();
    }

    private void OnLicenseClicked(object sender, EventArgs e)
        => OpenURL("https://github.com/crobengames/QDCrypt-Mobile/blob/main/LICENSE");

    private void OnPrivacyClicked(object sender, EventArgs e)
        => OpenURL("https://github.com/crobengames/QDCrypt-Mobile/blob/main/Privacy%20Policy.txt");

    private void OnCheckBoxChanged(object sender, EventArgs e)
        => UpdateAgreeButton();

    private async void OpenURL(string url)
    {
        try
        {
            Uri uri = new(url);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private void UpdateAgreeButton ()
    {
        if (!LicenseCB.IsChecked || !PrivacyCB.IsChecked)
        {
            AgreeBtn.IsVisible = false;
            return;
        }
        AgreeBtn.IsVisible = true;
    }


    private void OnAgreeClicked(object sender, EventArgs e)
    {

        if (!LicenseCB.IsChecked || !PrivacyCB.IsChecked)
            return;

        QDAgreement.Save();
        // Incase an error occur during saving.
        // A true boolean is pass to allow the user to use the app.
        // Otherwise a loop will occur where the user is stuck at the EULA page.
        Shell.Current.GoToAsync($"//{nameof(MainPage)}?EULA={true}");
    }
}