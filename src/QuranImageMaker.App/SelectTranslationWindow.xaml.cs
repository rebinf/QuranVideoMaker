using QuranImageMaker;
using System.Windows;

namespace QuranImageMaker.App
{
    /// <summary>
    /// Interaction logic for SelectTranslationWindow.xaml
    /// </summary>
    public partial class SelectTranslationWindow : Window
    {
        public QuranTranslation SelectedTranslation { get; set; }

        public SelectTranslationWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
