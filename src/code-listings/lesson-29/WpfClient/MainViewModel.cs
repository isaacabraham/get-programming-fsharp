using Capstone5.Domain;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;

namespace Capstone5
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        public Customer Owner { get; private set; }
        public Command<int> DepositCommand { get; private set; }
        public Command<int> WithdrawCommand { get; private set; }
        public int Balance { get; private set; }
        public ObservableCollection<Transaction> Transactions { get; private set; }
        private RatedAccount account;
        private Tuple<bool, int> TryParseInt(object value)
        {
            int output = 0;
            var parsed = Int32.TryParse(value as string, out output);
            return Tuple.Create(parsed, output);
        }

        private void UpdateAccount(RatedAccount newAccount)
        {
            this.account = newAccount;
            this.LoadTransactions();
            //this.Balance = (int)account.Balance;
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
