using QuranImageMaker;
using System.Windows;

namespace QuranVideoMaker.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for AddQuranView.xaml
    /// </summary>
    public partial class AddQuranView : DialogViewBase
    {
        public AddQuranView()
        {
            InitializeComponent();
        }

        private void RemoveTranslation_Click(object sender, RoutedEventArgs e)
        {
            ((AddQuranViewModel)DataContext).Translations.Remove((sender as FrameworkElement).DataContext as TranslationInfo);
        }
    }
}
