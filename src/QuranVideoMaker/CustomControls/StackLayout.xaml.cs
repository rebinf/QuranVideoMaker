using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for StackLayout.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class StackLayout : UserControl
    {
        public ObservableCollection<LayoutItem> Items { get; set; } = new ObservableCollection<LayoutItem>();

        public StackLayout()
        {
            InitializeComponent();

            this.Loaded += StackLayout_Loaded;
            this.Items.CollectionChanged += Items_CollectionChanged;
        }

        private void StackLayout_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Init();
            }
        }

        private void Init()
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            grid.Children.Clear();

            if (grid.ColumnDefinitions.Count == 0)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5, GridUnitType.Pixel) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            var count = 0;

            foreach (var item in Items)
            {
                item.VerticalAlignment = VerticalAlignment.Center;
                item.Margin = new Thickness(0, 3, 0, 3);

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

                var label = new TextBlock() { Text = item.Label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 3, 0, 3) };

                grid.Children.Add(label);
                grid.Children.Add(item);

                Grid.SetRow(label, count);
                Grid.SetColumn(label, 0);

                Grid.SetRow(item, count);
                Grid.SetColumn(item, 2);

                count++;
            }
        }
    }
}
