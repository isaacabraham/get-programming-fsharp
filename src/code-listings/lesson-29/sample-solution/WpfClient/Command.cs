using System;
using System.Windows.Input;

namespace Capstone5
{
    public class Command<T> : ICommand
    {
        private readonly Action<T> command;
        private readonly Func<Boolean> canExecute;
        private readonly Func<Object, Tuple<Boolean, T>> tryParse;

        public event EventHandler CanExecuteChanged;

        public Command(Action<T> command, Func<object, Tuple<bool, T>> tryParse, Func<bool> canExecute = null)
        {
            this.command = command;
            this.tryParse = tryParse;
            this.canExecute = canExecute ?? (() => true);
        }

        public void Refresh()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public Boolean CanExecute(Object parameter)
        {
            return canExecute();
        }

        public void Execute(Object parameter)
        {
            var result = tryParse(parameter);
            if (result.Item1)
            {
                command(result.Item2);
                this.Refresh();
            }
        }
    }
}
