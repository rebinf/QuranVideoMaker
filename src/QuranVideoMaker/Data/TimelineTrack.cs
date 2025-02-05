﻿using QuranImageMaker;
using QuranVideoMaker.Extensions;
using QuranVideoMaker.Undo;
using QuranVideoMaker.Undo.UndoUnits;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// TimelineTrack
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("TimelineTrack")]
    [DisplayName("TimelineTrack")]
    [DebuggerDisplay("{Type} ({Name})")]
    public class TimelineTrack : ITimelineTrack, IJsonOnDeserialized
    {
        private string _id = Guid.NewGuid().ToString().Replace("-", string.Empty);
        private TimelineTrackType _type;
        private string _name;
        private int _height = 50;
        private ObservableCollection<ITrackItem> _items;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public event EventHandler Changed;

        /// <inheritdoc/>
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public TimelineTrackType Type
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
        public ObservableCollection<ITrackItem> Items
        {
            get { return _items ??= new ObservableCollection<ITrackItem>(); }
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
            HookEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineTrack" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public TimelineTrack(TimelineTrackType type, string name) : this()
        {
            Type = type;
            Name = name;
        }

        /// <inheritdoc/>
        public void AddTrackItem(ITrackItem item)
        {
            Items.Add(item);
            UndoEngine.Instance.AddUndoUnit(new TrackItemAddUndoUnit(this, item));
        }

        /// <inheritdoc/>
        public void AddTrackItems(IEnumerable<ITrackItem> items)
        {
            var undoUnit = new TrackItemAddUndoUnit(items.Select(x => new TrackAndItemData(this, x)));

            foreach (var item in items)
            {
                Items.Add(item);
            }

            UndoEngine.Instance.AddUndoUnit(undoUnit);
        }

        /// <inheritdoc/>
        public void RemoveTrackItem(ITrackItem item)
        {
            Items.Remove(item);
            UndoEngine.Instance.AddUndoUnit(new TrackItemRemoveUndoUnit(this, item));
        }

        /// <inheritdoc/>
        public void RemoveTrackItems(IEnumerable<ITrackItem> items)
        {
            var undoUnit = new TrackItemRemoveUndoUnit(items.Select(x => new TrackAndItemData(this, x)));

            foreach (var item in items)
            {
                Items.Remove(item);
            }

            UndoEngine.Instance.AddUndoUnit(undoUnit);
        }

        /// <inheritdoc/>
        public TimeCode GetDuration()
        {
            if (Items.Count == 0)
            {
                return new TimeCode();
            }

            var lastItem = Items.OrderByDescending(x => x.GetRightTime()).FirstOrDefault();
            return lastItem.GetRightTime();
        }

        /// <inheritdoc/>
        public void CutItem(ITrackItem item, double timelineFrame)
        {
            if (!Items.Contains(item))
            {
                throw new Exception("Item not found in track.");
            }

            var copy = item.Clone();

            var oldEnd = item.End;

            var localFrame = item.GetLocalFrame(timelineFrame);
            item.End = new TimeCode(localFrame, item.End.FPS);
            copy.End = new TimeCode(oldEnd.TotalFrames, oldEnd.FPS);
            copy.Start = new TimeCode(localFrame, item.Start.FPS);
            copy.Position = new TimeCode(item.GetRightTime().TotalFrames, item.Position.FPS);

            AddTrackItem(copy);
        }

        /// <inheritdoc/>
        public ITrackItem GetItemAtTimelineFrame(double timelineFrame)
        {
            return Items.FirstOrDefault(x => x.Position.TotalSeconds <= timelineFrame && x.GetRightTime().TotalSeconds >= timelineFrame);
        }

        /// <inheritdoc/>
        public string GenerateNewId()
        {
            Id = Guid.NewGuid().ToString().Replace("-", string.Empty);
            return Id;
        }

        public void FixVerseNumbers()
        {
            var items = Items.Cast<QuranTrackItem>().OrderBy(x => x.Position.TotalFrames).ToList();
            items.Select(x => x.Verse).UpdateVerseParts();
            items.ForEach(x => x.UpdateName());
        }

        public void OnDeserialized()
        {
            HookEvents();
        }

        private void HookEvents()
        {
            this.Items.CollectionChanged -= Items_CollectionChanged;
            this.Items.CollectionChanged += Items_CollectionChanged;

            foreach (var item in Items)
            {
                item.PropertyChanged -= Item_PropertyChanged;
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed?.Invoke(sender, EventArgs.Empty);
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Type == TimelineTrackType.Quran)
            {
                FixVerseNumbers();
            }

            HookEvents();

            Changed?.Invoke(this, EventArgs.Empty);
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