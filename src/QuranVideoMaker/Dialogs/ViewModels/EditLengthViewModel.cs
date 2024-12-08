using QuranVideoMaker.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// Edit Length ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Edit Length ViewModel")]
    [DisplayName("EditLengthViewModel")]
    [DebuggerDisplay("EditLengthViewModel")]
    public class EditLengthViewModel : DialogViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditLengthViewModel"/> class.
        /// </summary>
        public EditLengthViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditLengthViewModel"/> class.
        /// </summary>
        public EditLengthViewModel(TimeCode length)
        {
            Data = length;
        }

        public override void OnOK()
        {
            var tryParse = TimeCode.TryParse(Data?.ToString(), out var timeCode);

            if (tryParse)
            {
                Data = timeCode;
            }
            else
            {
                MessageBox.Show("Invalid time code format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            base.OnOK();
        }
    }
}
