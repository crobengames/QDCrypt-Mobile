using Microsoft.Maui.Storage;
using QDCrypt.Models;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace QDCrypt.Views;

// Incase an error occur during user data saving.
// A true boolean is pass from the EULA page to allow user access of the app.
// Otherwise a loop will occur where the user is stuck at the EULA page.
[QueryProperty(nameof(HasAgreed), "EULA")]
public partial class MainPage : ContentPage
{

    public bool HasAgreed { get; set; }
    private bool isEncrypted = false;

	public MainPage()
	{
		InitializeComponent();

	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Check the app data if the user has agreed to the EULA.
        // Also check if the user has pressed the agree button at EULA page.
        if (!QDAgreement.UserHasAgreed() && !HasAgreed)
        {
            Shell.Current.GoToAsync(nameof(EULAPage));
            return;
        }

        // To avoid null reference error make sure that text fields is empty and not null
        PasswordEntry.Text = String.Empty;
        CipherEditor.Text = String.Empty;

        SetCipherBehavior(false);
    }

    #region Control Events

    private void OnEncryptClicked(object sender, EventArgs e)
    {
        string hashKey = String.Empty;

        try
        {
            hashKey = QDCryptography.GetSHA256Hash(PasswordEntry.Text);
        }
        catch (Exception)
        {
            DisplayDialogue(QDDialogue.HashingError());
            return;
        }

        string key = hashKey.Substring(0, 32);
        string iv = hashKey.Substring(hashKey.Length - 16);
     
        try
        {
            CipherEditor.Text = QDCryptography.AesEcnrypt(CipherEditor.Text, key, iv);     
            UnfocusCipherEditor();
            SetCipherBehavior(true);
            DisplayDialogue(QDDialogue.EncryptionSuccess());
        }
        catch (Exception)
        {
            DisplayDialogue(QDDialogue.EncryptionError());
        }
    }

    private void OnDecryptClicked(object sender, EventArgs e)
    {
        string hashKey = String.Empty;

        try
        {
            hashKey = QDCryptography.GetSHA256Hash(PasswordEntry.Text);
        }
        catch (Exception)
        {
            DisplayDialogue(QDDialogue.HashingError());           
            return;
        }

        string key = hashKey.Substring(0, 32);
        string iv = hashKey.Substring(hashKey.Length - 16);

        try
        {
            CipherEditor.Text = QDCryptography.AesDecrypt(CipherEditor.Text.Trim(), key, iv);
            SetCipherBehavior(false);
            DisplayDialogue(QDDialogue.DecryptionSuccess());
        }
        catch (Exception)
        {
            DisplayDialogue(QDDialogue.DecryptionError());
        }    
    }

    private async void OnClearClicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(PasswordEntry.Text) && string.IsNullOrEmpty(CipherEditor.Text))
            return;

        var dialogue = QDDialogue.ClearWarning();
        bool answer = await DisplayAlert(dialogue.Item1, dialogue.Item2, dialogue.Item3, dialogue.Item4);

        if (answer) 
        {
            PasswordEntry.Text = String.Empty;
            CipherEditor.Text = String.Empty;
            SetCipherBehavior(false);
        }
        else
        {
            UnfocusCipherEditor();
        }    
    }

    private void PasswordTextChanged(object sender, EventArgs e)
    {
        if (PasswordEntry.Text.Length >= 250)
        {
            DisplayDialogue(QDDialogue.MaxPasswordNotice());
        }
    }

    private void TogglePasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        if (PasswordEntry.IsPassword)
        {
            ShowHideBtn.Source = "hide.png";
        }                
        else
        {
            ShowHideBtn.Source = "show.png";
        }
    }

    private async void CopyClicked(object sender, EventArgs e)
    {
        try
        {            
            await Clipboard.Default.SetTextAsync(CipherEditor.Text);
        }
        catch (Exception)
        {
            DisplayDialogue(QDDialogue.CopyError());
        }   
    }

    private async void CipherEditorFocused(object sender, FocusEventArgs e)
    {
        // Hide CopyControl each focus, so whether encrypting or decrypting it's not visible 
        CopyControl.IsVisible = false;

        if (!isEncrypted) return;

        var dialogue = QDDialogue.CipherEditWarning();
        bool answer = await DisplayAlert(dialogue.Item1, dialogue.Item2, dialogue.Item3, dialogue.Item4);

        if (!answer)    // When cancel is selected remove the focus and show/hide the copy button
        {
            UnfocusCipherEditor();
            SetCipherBehavior(isEncrypted);
        }
        else    // If the OK is selected assume that the cipher has change so change isEncrypted to false
        {
            isEncrypted = false;
        }
    }

    private void CipherEditorUnfocused(object sender, FocusEventArgs e)
    {
        CopyControl.IsVisible = (CipherEditor.Text.Length > 0);
    }

    #endregion /Control Events

    #region Other Methods

    private void UnfocusCipherEditor()
    {
        CipherEditor.IsEnabled = false;
        CipherEditor.IsEnabled = true;
    }

    private void SetCipherBehavior(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
        try
        {
            CopyControl.IsVisible = (CipherEditor.Text.Length > 0);
        }
        catch (Exception)
        {
            return;
        }        
    }

    private void DisplayDialogue((string, string, string) dialogue)
    {
        DisplayAlert(dialogue.Item1, dialogue.Item2, dialogue.Item3);
    }

    #endregion /Other Methods

    #region Button Effects

    private void OnEncryptPressed(object sender, EventArgs e) => EncryptBtn.Source = "encrypthover.png";

    private void OnEncryptReleased(object sender, EventArgs e) => EncryptBtn.Source = "encrypt.png";

    private void OnDecryptPressed(object sender, EventArgs e) => DecryptBtn.Source = "decrypthover.png";

    private void OnDecryptReleased(object sender, EventArgs e) => DecryptBtn.Source = "decrypt.png";

    private void OnClearPressed(object sender, EventArgs e) => ClearBtn.Source = "clearhover.png";

    private void OnClearReleased(object sender, EventArgs e) => ClearBtn.Source = "clear.png";

    private void OnCopyPressed(object sender, EventArgs e) => CopyControl.Opacity = 1.0;

    private void OnCopyReleased(object sender, EventArgs e) => CopyControl.Opacity = 0.35;

    #endregion /Button Effects

}

