using Capstone6.Domain;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Capstone6.Converters
{
    public class BankOperationConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var bankOperation = value as BankOperation;
            if (bankOperation == null)
                return "Unknown";
            else if (bankOperation.IsWithdraw)
                return "Withdraw";
            else
                return "Deposit";
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
