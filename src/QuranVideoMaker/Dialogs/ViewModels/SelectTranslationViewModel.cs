using QuranTranslationImageGenerator;
using System.ComponentModel;
using System.Diagnostics;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// Select Translation ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Select Translation ViewModel")]
    [DisplayName("SelectTranslationViewModel")]
    [DebuggerDisplay("SelectTranslationViewModel")]
    public class SelectTranslationViewModel : DialogViewModelBase
    {
        /// <summary>
        /// Gets or sets the selected translation.
        /// </summary>
        public QuranTranslation SelectedTranslation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectTranslationViewModel"/> class.
        /// </summary>
        public SelectTranslationViewModel()
        {
        }
    }
}
