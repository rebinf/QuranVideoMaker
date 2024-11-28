using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace QuranImageMaker
{
    /// <summary>
    /// Represents a collection of <see cref="VerseInfo"/> objects with additional rendering settings.
    /// </summary>
    public class VerseInfoCollection : ObservableCollection<VerseInfo>
    {
        /// <summary>
        /// Gets or sets the render settings for the verses.
        /// </summary>
        public VerseRenderSettings RenderSettings { get; set; } = new VerseRenderSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfoCollection"/> class.
        /// </summary>
        public VerseInfoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfoCollection"/> class
        /// with the specified collection of <see cref="VerseInfo"/> objects.
        /// </summary>
        /// <param name="collection">The collection of <see cref="VerseInfo"/> objects to include.</param>
        public VerseInfoCollection(IEnumerable<VerseInfo> collection) : base(collection)
        {
            UpdateVerseParts();
        }

        /// <summary>
        /// Updates the parts of the verses in the collection.
        /// </summary>
        public void UpdateVerseParts()
        {
            this.UpdateVerseParts();
        }

        /// <summary>
        /// Called when the collection changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            UpdateVerseParts();
            base.OnCollectionChanged(e);
        }
    }
}
