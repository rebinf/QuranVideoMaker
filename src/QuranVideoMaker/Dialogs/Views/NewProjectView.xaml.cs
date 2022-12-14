using QuranTranslationImageGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
