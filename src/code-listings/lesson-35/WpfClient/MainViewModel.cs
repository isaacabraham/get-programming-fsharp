using System;
using System.Collections.ObjectModel;
using Capstone6.Domain;

namespace Capstone6
{
    public class MainViewModel : ViewModelBase
    {
        public Customer Owner { get; private set; }
        public Command<int> DepositCommand { get; private set; }
        public Command<int> WithdrawCommand { get; private set; }
        private int balance;

        public int Balance
        {
            get => balance;
            private set => SetProperty(ref balance, value);
        }

        public ObservableCollection<Transaction> Transactions { get; private set; }
        private RatedAccount account;

        private Tuple<bool, int> TryParseInt(object value)
        {
            var parsed = Int32.TryParse(value as string, out int output);
            return Tuple.Create(parsed, output);
        }

        private void UpdateAccount(RatedAccount newAccount)
        {
            this.account = newAccount;
            this.LoadTransactions();
            this.Balance = (int)account.Balance;
        }

        private void LoadTransactions()
        {
            Transactions.Clear();
            foreach (var txn in Api.LoadTransactionHistory(Owner))
                Transactions.Add(txn);
        }

        public MainViewModel()
        {
            Owner = new Customer("isaac");
            Transactions = new ObservableCollection<Transaction>();
            this.LoadTransactions();
            UpdateAccount(Api.LoadAccount(Owner));
            DepositCommand = new Command<int>(
                amount =>
                {
                    UpdateAccount(Api.Deposit(amount, Owner));
                    WithdrawCommand.Refresh();
                }, TryParseInt);
            WithdrawCommand = new Command<int>(
                amount => UpdateAccount(Api.Withdraw(amount, Owner)),
                TryParseInt,
                () => account.IsInCredit);
        }
    }
}
