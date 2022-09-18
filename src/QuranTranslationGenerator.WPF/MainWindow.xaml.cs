﻿using QuranTranslationImageGenerator;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuranTranslationGenerator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackgroundColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((MainWindowViewModel)DataContext).Settings.BackgroundColor = color;
            }
        }

        private void TextColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((MainWindowViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextColor = color;
            }
        }

        private void TextShadowColor_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var colorPicker = new System.Windows.Forms.ColorDialog();

            if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SKColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B, colorPicker.Color.A);

                ((MainWindowViewModel)DataContext).Settings.ArabicScriptRenderSettings.TextShadowColor = color;
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
            ((MainWindowViewModel)DataContext).Settings.TranslationRenderSettings.Remove((sender as FrameworkElement).DataContext as VerseRenderSettings);
        }
    }
}
