using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuranImageMaker
{
    public class VerseInfoCollection : ObservableCollection<VerseInfo>
    {
        public VerseRenderSettings RenderSettings { get; set; } = new VerseRenderSettings();

        public VerseInfoCollection()
        {
        }

        public VerseInfoCollection(IEnumerable<VerseInfo> collection) : base(collection)
        {
            UpdateVerseParts();
        }

        public void UpdateVerseParts()
        {
            this.UpdateVerseParts();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            UpdateVerseParts();
            base.OnCollectionChanged(e);
        }
    }
}
