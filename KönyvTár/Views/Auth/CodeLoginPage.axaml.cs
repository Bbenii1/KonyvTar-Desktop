using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;

namespace KönyvTár.Views.Auth;

public partial class CodeLoginPage : UserControl
{
    public event System.Action? BackRequested;

    public CodeLoginPage()
    {
        InitializeComponent();
        BackButton.Click += (_, _) => BackRequested?.Invoke();
        RequestCodeButton.Click += OnRequestCodeClick;
        ChangeEmailButton.Click += OnChangeEmailClick;
        ResendCodeButton.Click += (_, _) => StatusText.Text = "Kódküldés jelenleg csak bemutató; e-mail nem ment ki.";
        SubmitCodeButton.Click += OnSubmitCodeClick;
    }

    public void SetEmail(string? email)
    {
        EmailTextBox.Text = email?.Trim() ?? string.Empty;
        EmailEntryPanel.IsVisible = true;
        CodeSentPanel.IsVisible = false;
        CodeEntryPanel.IsVisible = false;
        CodeTextBox.Text = string.Empty;
        StatusText.Text = string.Empty;
    }

    private void OnRequestCodeClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
        {
            StatusText.Text = "Add meg az e-mail címed.";
            return;
        }

        EmailSummary.Text = EmailTextBox.Text.Trim();
        EmailEntryPanel.IsVisible = false;
        CodeSentPanel.IsVisible = true;
        CodeEntryPanel.IsVisible = true;
        StatusText.Text = "Kódküldés jelenleg csak bemutató; e-mail nem ment ki.";
        CodeTextBox.Focus();
    }

    private void OnChangeEmailClick(object? sender, RoutedEventArgs e)
    {
        CodeSentPanel.IsVisible = false;
        CodeEntryPanel.IsVisible = false;
        EmailEntryPanel.IsVisible = true;
        CodeTextBox.Text = string.Empty;
        StatusText.Text = string.Empty;
        EmailTextBox.Focus();
    }

    private void OnSubmitCodeClick(object? sender, RoutedEventArgs e)
    {
        var code = CodeTextBox.Text;
        if (code is null || code.Length != 6 || !code.All(char.IsDigit))
        {
            StatusText.Text = "Add meg a 6 jegyű kódot.";
            return;
        }

        StatusText.Text = "Kódellenőrzés jelenleg csak bemutató.";
    }
}
