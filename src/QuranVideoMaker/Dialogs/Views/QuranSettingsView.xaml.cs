using QuranTranslationImageGenerator;
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
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.BackgroundColor = color;
            }
        }

        private void TextColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextColor = color;
            }
        }

        private void TextShadowColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((QuranSettingsViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextShadowColor = color;
            }
        }

        private void TranslationTextColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((sender as FrameworkElement).DataContext as VerseRenderSettings).TextColor = color;
            }
        }

        private void TranslationTextShadowColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((sender as FrameworkElement).DataContext as VerseRenderSettings).TextShadowColor = color;
            }
        }

        private void RemoveTranslation_Click(object sender, RoutedEventArgs e)
        {
            ((QuranSettingsViewModel)DataContext).Settings.TranslationRenderSettings.Remove((sender as FrameworkElement).DataContext as VerseRenderSettings);
        }
    }
}
