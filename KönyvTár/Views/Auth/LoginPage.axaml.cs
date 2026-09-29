using Avalonia.Controls;
using Avalonia.Interactivity;

namespace KönyvTár.Views.Auth;

public partial class LoginPage : UserControl
{
    public event System.Action? LoginRequested;
    public event System.Action? RegisterRequested;
    public event System.Action? CodeLoginRequested;
    public event System.Action? ForgotPasswordRequested;

    public string? Email => EmailTextBox.Text;

    public LoginPage()
    {
        InitializeComponent();
        LoginButton.Click += OnLoginClick;
        RegisterButton.Click += (_, _) => RegisterRequested?.Invoke();
        CodeLoginButton.Click += (_, _) => CodeLoginRequested?.Invoke();
        ForgotPasswordButton.Click += (_, _) => ForgotPasswordRequested?.Invoke();
    }

    public void ShowStatus(string message) => StatusText.Text = message;

    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            ShowStatus("Add meg e-mail címed és jelszavad.");
            return;
        }

        LoginRequested?.Invoke();
    }
}
