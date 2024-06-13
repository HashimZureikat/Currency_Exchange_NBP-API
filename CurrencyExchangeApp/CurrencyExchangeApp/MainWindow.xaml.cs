using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using CurrencyExchangeApp.CurrencyExchangeServiceReference;

namespace CurrencyExchangeApp
{
    public partial class MainWindow : Window
    {
        private readonly CurrencyExchangeServiceClient _serviceClient;
        private User _loggedInUser;

        public MainWindow()
        {
            InitializeComponent();
            _serviceClient = new CurrencyExchangeServiceClient("BasicHttpBinding_ICurrencyExchangeService");
        }

        private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            try
            {
                var user = await _serviceClient.CreateUserAsync(username, password);
                MessageBox.Show($"User {user.Username} created with balance {user.Balance}");
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void LoginUserButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            try
            {
                _loggedInUser = await _serviceClient.LoginUserAsync(username, password);
                LoggedInAsLabel.Content = $"Logged in as: {_loggedInUser.Username}";
                EnableUserActions();
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void TopUpAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var amount = decimal.Parse(AmountTextBox.Text);

            try
            {
                var result = await _serviceClient.TopUpAccountAsync(_loggedInUser.Username, amount);
                MessageBox.Show(result ? "Top up successful" : "Top up failed");
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void GetUserBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var balance = await _serviceClient.GetUserBalanceAsync(_loggedInUser.Username);
                UserBalanceLabel.Content = $"User Balance: {balance}";
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CalculateExchangeButton_Click(object sender, RoutedEventArgs e)
        {
            var fromCurrency = FromCurrencyTextBox.Text;
            var toCurrency = ToCurrencyTextBox.Text;
            var amount = decimal.Parse(CalcAmountTextBox.Text);

            try
            {
                var calculatedAmount = await _serviceClient.CalculateExchangeAmountAsync(fromCurrency, toCurrency, amount);
                CalculatedAmountLabel.Content = $"Calculated Amount: {calculatedAmount}";
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void GetExchangeRateButton_Click(object sender, RoutedEventArgs e)
        {
            var currencyCode = CurrencyCodeTextBox.Text;

            try
            {
                var rate = await _serviceClient.GetExchangeRateAsync(currencyCode);
                ExchangeRateLabel.Content = $"Exchange Rate: {rate}";
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void GetArchivedRatesButton_Click(object sender, RoutedEventArgs e)
        {
            var startDate = DateTime.Parse(StartDatePicker.Text);
            var endDate = DateTime.Parse(EndDatePicker.Text);

            try
            {
                var rates = await _serviceClient.GetArchivedExchangeRatesAsync(startDate, endDate);
                ArchivedRatesLabel.Content = "Archived Rates:\n" + string.Join("\n", rates.Select(r => $"{r.Date.ToShortDateString()}: {r.Rate}"));
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void EnableUserActions()
        {
            TopUpAccountButton.IsEnabled = true;
            GetUserBalanceButton.IsEnabled = true;
            FromCurrencyTextBox.IsEnabled = true;
            ToCurrencyTextBox.IsEnabled = true;
            CalcAmountTextBox.IsEnabled = true;
            CalculateExchangeButton.IsEnabled = true;
            CurrencyCodeTextBox.IsEnabled = true;
            GetExchangeRateButton.IsEnabled = true;
            StartDatePicker.IsEnabled = true;
            EndDatePicker.IsEnabled = true;
            GetArchivedRatesButton.IsEnabled = true;
            AmountTextBox.IsEnabled = true;
        }
    }
}
