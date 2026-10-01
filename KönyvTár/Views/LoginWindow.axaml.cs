using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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

        //ThemeButton.Click += OnThemeButtonClick;
        //UpdateThemeButton();
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
    }

    private void OnPasswordResetFinished()
    {
        ShowLoginPage();
        _loginPage.ShowStatus("A jelszó-visszaállítás jelenleg csak bemutató.");
    }

    //private void OnThemeButtonClick(object? sender, RoutedEventArgs e)
    //{
    //    ThemeManager.Toggle();
    //    UpdateThemeButton();
    //}

    //private void UpdateThemeButton() => ThemeIcon.Kind = ThemeManager.IsDark
    //    ? MaterialIconKind.MoonWaningCrescent
    //    : MaterialIconKind.WhiteBalanceSunny;
    
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
