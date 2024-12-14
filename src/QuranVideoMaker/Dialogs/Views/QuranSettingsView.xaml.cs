using QuranImageMaker;
using QuranVideoMaker.Dialogs.ViewModels;
using SkiaSharp;
using System.Windows;
using System.Windows.Input;

namespace QuranVideoMaker.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for QuranSettingsView.xaml
    /// </summary>
    public partial class QuranSettingsView : DialogViewBase
    {
        public QuranSettingsView()
        {
            InitializeComponent();
        }

        private void BackgroundColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.BackgroundColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();
            }
        }

        private void TextColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();

            }
        }

        private void TextShadowColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextShadowColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();
            }
        }

        private void TranslationTextColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((sender as FrameworkElement).DataContext as VerseRenderSettings).TextColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();
            }
        }

        private void TranslationTextShadowColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((sender as FrameworkElement).DataContext as VerseRenderSettings).TextShadowColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();
            }
        }

        private void TextBackgroundColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = GetColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainWindowViewModel.Instance.CurrentProject.CustomColors = colorPicker.CustomColors;
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);
                ((QuranSettingsViewModel)DataContext).Settings.TextBackgroundColor = color;
                ((QuranSettingsViewModel)DataContext).UpdatePreview();
            }
        }

        private void RemoveTranslation_Click(object sender, RoutedEventArgs e)
        {
            var settings = (sender as FrameworkElement).DataContext as VerseRenderSettings;
            ((QuranSettingsViewModel)DataContext).Settings.TranslationRenderSettings.Remove(settings);

            // need to remove it from the project as well
            foreach (var verse in MainWindowViewModel.Instance.CurrentProject.GetVerses())
            {
                foreach (var item in verse.Translations.ToArray())
                {
                    if (item.TypeId == settings.Id)
                    {
                        verse.Translations.Remove(item);
                    }
                }
            }

            ((QuranSettingsViewModel)DataContext).UpdatePreview();
        }

        private static System.Windows.Forms.ColorDialog GetColorDialog()
        {
            return new System.Windows.Forms.ColorDialog() { CustomColors = MainWindowViewModel.Instance.CurrentProject.CustomColors };
        }
    }
}
