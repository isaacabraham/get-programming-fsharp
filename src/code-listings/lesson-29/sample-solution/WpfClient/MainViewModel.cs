using Capstone5.Domain;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Capstone5
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        public ICollectionView TransactionsView { get { return cvs.View; } }
        public string Owner { get; private set; }
        public Command<int> DepositCommand { get; private set; }
        public Command<int> WithdrawCommand { get; private set; }
        public int Balance { get; private set; }

        private RatedAccount account;
        private readonly ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();
        private readonly CollectionViewSource cvs;
        private Tuple<bool, int> TryParseInt(object value)
        {
            int output = 0;
            var parsed = Int32.TryParse(value as string, out output);
            return Tuple.Create(parsed, output);
        }

        private void UpdateAccount(RatedAccount account)
        {
            this.account = account;
            this.LoadTransactions();
            this.Balance = (int)account.Balance;
        }

        private void LoadTransactions()
        {
            transactions.Clear();
            foreach (var txn in Api.LoadTransactionHistory(Owner))
                transactions.Add(txn);
        }

        public MainViewModel()
        {
            Owner = "isaac";
            cvs = new CollectionViewSource { Source = transactions };
            cvs.SortDescriptions.Add(new SortDescription("Timestamp", ListSortDirection.Descending));
            this.LoadTransactions();
            UpdateAccount(Api.LoadAccount(Owner));

            DepositCommand = new Command<int>(
                amount =>
                {
                    UpdateAccount(Api.Deposit(amount, account));
                    WithdrawCommand.Refresh();
                }, TryParseInt);
            WithdrawCommand = new Command<int>(
                amount =>
                {
                    var creditAccount = (RatedAccount.InCredit)account;
                    UpdateAccount(Api.Withdraw(amount, creditAccount.Account));
                },
                TryParseInt,
                () => account.IsInCredit);
        }
    }
}
