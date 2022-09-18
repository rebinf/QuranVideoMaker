using QuranTranslationImageGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public QuranTranslation SelectedTranslation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectTranslationViewModel"/> class.
        /// </summary>
        public SelectTranslationViewModel()
        {
        }
    }
}
