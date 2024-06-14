
# CurrencyExchangeApp

## Overview

CurrencyExchangeApp is a WPF application that interfaces with a WCF service to provide functionality for creating user accounts, topping up accounts, checking exchange rates, and retrieving archived exchange rates using the National Bank of Poland's API.

## Features

- **User Account Management**
  - Create a new user account
  - Login to an existing account

- **Account Operations**
  - Top up user account
  - Get user account balance

- **Currency Exchange Operations**
  - Calculate exchange amount between two currencies
  - Get current exchange rate for a specific currency
  - Retrieve archived exchange rates for a specific time period

## Technology Stack

- **Frontend:** WPF (Windows Presentation Foundation)
- **Backend:** WCF (Windows Communication Foundation)
- **Data Source:** National Bank of Poland API

## Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.8 or later

## How to Run

1. **Clone the Repository:**

    ```bash
    git clone https://github.com/HashimZureikat/CurrecnyExchange_NBP-api.git
    cd CurrecnyExchange_NBP-api
    ```

2. **Open the Solution:**

    Open `CurrencyExchangeSolution.sln` in Visual Studio.

3. **Build the Solution:**

    Build the entire solution to restore all the required NuGet packages and build the projects.

4. **Run the Service:**

    Set `CurrencyExchangeService` as the startup project and run it to start the WCF service.

5. **Run the Client Application:**

    Set `CurrencyExchangeApp` as the startup project and run it to start the WPF client application.

## Project Structure

```
CurrencyExchangeSolution
├── CurrencyExchangeApp                # WPF Client Application
│   ├── Connected Services
│   ├── Properties
│   ├── References
│   ├── App.config
│   ├── App.xaml
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── packages.config
│
├── CurrencyExchangeService            # WCF Service
│   ├── Properties
│   ├── References
│   ├── Data
│   │   ├── ApplicationDbContext.cs
│   ├── Models
│   │   ├── ExchangeRate.cs
│   │   ├── Transaction.cs
│   │   ├── User.cs
│   │   ├── UserCurrency.cs
│   ├── App.config
│   ├── CurrencyExchangeService.cs
│   ├── ICurrencyExchangeService.cs
│   ├── NbpApiClient.cs
│   ├── packages.config
```

## Usage

1. **Creating a User:**
   - Enter a username and password.
   - Click "Create User".

2. **Logging In:**
   - Enter the username and password.
   - Click "Login".
   - Once logged in, you can perform account operations.

3. **Topping Up Account:**
   - Enter the amount to top up.
   - Click "Top Up Account".

4. **Getting Account Balance:**
   - Click "Get Balance".

5. **Calculating Exchange Amount:**
   - Select the from and to currencies.
   - Enter the amount.
   - Click "Calculate".

6. **Getting Current Exchange Rate:**
   - Select the currency code.
   - Click "Get Exchange Rate".

7. **Getting Archived Exchange Rates:**
   - Select the start and end dates.
   - Click "Get Archived Rates".

![image](https://github.com/HashimZureikat/Currency_Exchange_NBP-API/assets/87613242/2e6af547-b608-428e-9df7-eea9abaf8617)





## License

This project is licensed under the MIT License. See the LICENSE file for more details.

