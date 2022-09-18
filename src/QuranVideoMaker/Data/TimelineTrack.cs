using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// TimelineTrack
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("TimelineTrack")]
    [DisplayName("TimelineTrack")]
    [DebuggerDisplay("{Type} ({Name})")]
    public class TimelineTrack : INotifyPropertyChanged
    {
        private TrackType _type;
        private string _name;
        private int _height = 50;
        private ObservableCollection<TrackItemBase> _items;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TrackType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
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
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ObservableCollection<TrackItemBase> Items
        {
            get { return _items ??= new ObservableCollection<TrackItemBase>(); }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineTrack"/> class.
        /// </summary>
        public TimelineTrack()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineTrack" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public TimelineTrack(TrackType type, string name)
        {
            Type = type;
            Name = name;
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