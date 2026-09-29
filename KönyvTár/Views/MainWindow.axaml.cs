using Avalonia.Controls;
using Avalonia.Interactivity;
using Material.Icons;

namespace KönyvTár.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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

}
