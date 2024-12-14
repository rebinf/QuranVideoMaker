using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuranImageMaker
{
    /// <summary>
    /// RenderedTranslation
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("RenderedTranslation")]
    [DisplayName("RenderedTranslation")]
    [DebuggerDisplay("RenderedTranslation")]
    public class RenderedTranslation : INotifyPropertyChanged
    {
        private string _name;
        private Guid _guid;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the translation.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier for the translation.
        /// </summary>
        public Guid Guid
        {
            get { return _guid; }
            set
            {
                if (_guid != value)
                {
                    _guid = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderedTranslation"/> class.
        /// </summary>
        public RenderedTranslation()
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
