using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
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
            if (!decimal.TryParse(AmountTextBox.Text, out var amount))
            {
                MessageBox.Show("Please enter a valid decimal number for the amount.");
                return;
            }

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
            var fromCurrency = (FromCurrencyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var toCurrency = (ToCurrencyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency))
            {
                MessageBox.Show("Please select both From and To currencies.");
                return;
            }

            if (!decimal.TryParse(CalcAmountTextBox.Text, out var amount))
            {
                MessageBox.Show("Please enter a valid decimal number for the amount.");
                return;
            }

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
            var currencyCode = (CurrencyCodeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                MessageBox.Show("Please select a currency code.");
                return;
            }

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
            if (!DateTime.TryParse(StartDatePicker.Text, out var startDate))
            {
                MessageBox.Show("Please enter a valid start date.");
                return;
            }

            if (!DateTime.TryParse(EndDatePicker.Text, out var endDate))
            {
                MessageBox.Show("Please enter a valid end date.");
                return;
            }

            try
            {
                var rates = await _serviceClient.GetArchivedExchangeRatesAsync(startDate, endDate);
                ArchivedRatesLabel.Content = "Archived Rates:\n" + string.Join("\n", rates.Take(15).Select(r => $"{r.Date.ToShortDateString()}: {r.Rate}"));
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
            FromCurrencyComboBox.IsEnabled = true;
            ToCurrencyComboBox.IsEnabled = true;
            CalcAmountTextBox.IsEnabled = true;
            CalculateExchangeButton.IsEnabled = true;
            CurrencyCodeComboBox.IsEnabled = true;
            GetExchangeRateButton.IsEnabled = true;
            StartDatePicker.IsEnabled = true;
            EndDatePicker.IsEnabled = true;
            GetArchivedRatesButton.IsEnabled = true;
            AmountTextBox.IsEnabled = true;
        }
    }
}
