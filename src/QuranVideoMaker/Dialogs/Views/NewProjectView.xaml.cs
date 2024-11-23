using QuranImageMaker;
using System.Windows;

namespace QuranVideoMaker.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : DialogViewBase
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void RemoveTranslation_Click(object sender, RoutedEventArgs e)
        {
            ((AddQuranViewModel)DataContext).Translations.Remove((sender as FrameworkElement).DataContext as TranslationInfo);
        }

    }
}
