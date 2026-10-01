using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Material.Icons;
using KönyvTár.ViewModels;
using KönyvTár.Views.Auth;
using System;

namespace KönyvTár.Views;

public partial class LoginWindow : Window
{
    private LoginPage _loginPage = null!;
    private RegisterPage _registerPage = null!;
    //private CodeLoginPage _codeLoginPage = null!;
    private ForgotEmailPage _forgotEmailPage = null!;
    private ForgotCodePage _forgotCodePage = null!;
    private ResetPasswordPage _resetPasswordPage = null!;
    private bool _registerPageActive;

    public LoginWindow()
    {
        InitializeComponent();
        PositionChanged += OnPositionChanged;

        _loginPage = new LoginPage();
        _registerPage = new RegisterPage();
        //_codeLoginPage = new CodeLoginPage();
        _forgotEmailPage = new ForgotEmailPage();
        _forgotCodePage = new ForgotCodePage();
        _resetPasswordPage = new ResetPasswordPage();

        //_loginPage.LoginRequested += OpenMainWindow;
        _loginPage.RegisterRequested += () => ShowPage(_registerPage, "KönyvTár — Regisztráció");
       
        _loginPage.ForgotPasswordRequested += () => ShowPage(_forgotEmailPage, "KönyvTár — Jelszó visszaállítása");
        _registerPage.BackRequested += ShowLoginPage;
        //_codeLoginPage.BackRequested += ShowLoginPage;
        _forgotEmailPage.BackRequested += ShowLoginPage;
        _forgotEmailPage.CodeRequested += ShowForgotCodePage;
        _forgotCodePage.BackRequested += () => ShowPage(_forgotEmailPage, "KönyvTár — Jelszó visszaállítása");
        _forgotCodePage.ContinueRequested += () => ShowPage(_resetPasswordPage, "KönyvTár — Új jelszó");
        _resetPasswordPage.BackRequested += () => ShowPage(_forgotCodePage, "KönyvTár — E-mail ellenőrzése");
        _resetPasswordPage.Finished += OnPasswordResetFinished;

        ThemeButton.Click += OnThemeButtonClick;
        UpdateThemeButton();
        ShowLoginPage();
    }

    private void ShowLoginPage()
    {
        
        ShowPage(_loginPage, "KönyvTár — Belépés");
    }

    private void ShowForgotCodePage(string email)
    {
        _forgotCodePage.SetEmail(email);
        ShowPage(_forgotCodePage, "KönyvTár — E-mail ellenőrzése");
    }

    private void ShowPage(Control page, string title)
    {
        PageHost.Content = page;
        Title = title;
        
        SizeToContent = SizeToContent.Height;
        var screen = Screens.ScreenFromWindow(this);
        Height = screen is null
            ? 850
            : Math.Max(MinHeight, screen.WorkingArea.Height / RenderScaling - 32);

        Dispatcher.UIThread.Post(CenterOnScreen, DispatcherPriority.Loaded);
    }

    private bool _isCentering;

    private void OnPositionChanged(object? sender, PixelPointEventArgs e)
    {
        if (!_isCentering)
            CenterOnScreen();
    }

    private void CenterOnScreen()
    {
        var screen = Screens.ScreenFromWindow(this) ?? Screens.Primary;
        if (screen is null)
            return;

        // Size in DIPs (including title bar/borders when available)
        var size = FrameSize ?? ClientSize;
        if (size.Width <= 0 || size.Height <= 0)
            return; // not measured yet, try again later

        // Convert DIPs -> physical pixels using the TARGET screen's scaling
        var scale = screen.Scaling;
        var pixelWidth = (int)Math.Round(size.Width * scale);
        var pixelHeight = (int)Math.Round(size.Height * scale);

        var area = screen.WorkingArea; // already in physical pixels
        var centeredPosition = new PixelPoint(
            area.X + (area.Width - pixelWidth) / 2,
            area.Y + (area.Height - pixelHeight) / 2);

        if (Position == centeredPosition)
            return;

        _isCentering = true;
        Position = centeredPosition;
        _isCentering = false;
    }

    private void OnPasswordResetFinished()
    {
        ShowLoginPage();
        _loginPage.ShowStatus("A jelszó-visszaállítás jelenleg csak bemutató.");
    }

    private void OnThemeButtonClick(object? sender, RoutedEventArgs e)
    {
        ThemeManager.Toggle();
        UpdateThemeButton();
    }

    private void UpdateThemeButton() => ThemeIcon.Kind = ThemeManager.IsDark
        ? MaterialIconKind.MoonWaningCrescent
        : MaterialIconKind.WhiteBalanceSunny;
    
    private void OpenMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        var mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        desktop.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
    }
}
