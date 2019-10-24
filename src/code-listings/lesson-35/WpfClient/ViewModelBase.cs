using System.ComponentModel;
using System.Runtime.CompilerServices;

// based on https://gist.github.com/milbrandt/a56ed3e44d265e2a8204e290d6abb435
namespace Capstone6
{
    /// <summary>
    /// abstract base class to supply interfaces INotifyPropertyChanged
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Generic method, stores new property value in an intended private  field and fires event PropertyChanged
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="Value"></param>
        /// <param name="property"></param>
        protected void SetProperty<T>(
            ref T storage,
            T Value,
            [CallerMemberName] string property = null)
        {
            if (Equals(storage, Value))
            {
                return;
            }

            storage = Value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);
    }
}
