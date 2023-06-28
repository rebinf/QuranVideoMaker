using System.Windows;
using System.Windows.Controls;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for LayoutItem.xaml
    /// </summary>
    public partial class LayoutItem : Grid
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LayoutItem), new PropertyMetadata(string.Empty));

        public LayoutItem()
        {
            InitializeComponent();
        }
    }
}
