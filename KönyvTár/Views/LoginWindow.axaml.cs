using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Material.Icons;
using KönyvTár.ViewModels;

namespace KönyvTár.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        LoginButton.Click += OnLoginClick;
        ThemeButton.Click += OnThemeButtonClick;
        UpdateThemeButton();
    }

    private void OnThemeButtonClick(object? sender, RoutedEventArgs e)
    {
        ThemeManager.Toggle();
        UpdateThemeButton();
    }

    private void UpdateThemeButton() => ThemeIcon.Kind = ThemeManager.IsDark
        ? MaterialIconKind.MoonWaningCrescent
        : MaterialIconKind.WhiteBalanceSunny;

    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            StatusText.Text = "Add meg e-mail címed és jelszavad.";
            return;
        }

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
    }
}
