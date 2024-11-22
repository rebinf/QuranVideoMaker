using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// DialogViewModelBase
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("DialogViewModelBase")]
    [DisplayName("DialogViewModelBase")]
    [DebuggerDisplay("DialogViewModelBase")]
    public partial class DialogViewModelBase : INotifyPropertyChanged
    {
        private object _data;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the close window.
        /// </summary>
        public Action<bool> CloseWindow { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModelBase"/> class.
        /// </summary>
        public DialogViewModelBase()
        {
        }

        [RelayCommand]
        public virtual void OnOK()
        {
            CloseWindow?.Invoke(true);
        }

        [RelayCommand]
        public void OnCancel()
        {
            CloseWindow?.Invoke(false);
        }

        public virtual void OnClosed()
        {

        }

        /// <summary>
        /// Called when public properties changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
