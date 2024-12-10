using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for ProjectClipsControl.xaml
    /// </summary>
    public partial class ProjectClipsControl : ListBox
    {
        public ProjectClipsControl()
        {
            InitializeComponent();
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            var element = e.OriginalSource as FrameworkElement ?? (e.OriginalSource as Run)?.Parent as TextBlock;

            if (element != null && element.DataContext is ProjectClip clipInfo)
            {
                this.SelectedItem = clipInfo;
                DragDrop.DoDragDrop(this, clipInfo, DragDropEffects.Copy);
            }
        }

        private void ListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    if (FileFormats.SupportedFileFormats.Contains(System.IO.Path.GetExtension(item).ToLower()))
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }

                    e.Handled = true;
                }
            }
        }

        private void ListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    if (FileFormats.SupportedFileFormats.Contains(System.IO.Path.GetExtension(item).ToLower()))
                    {
                        if (this.DataContext is MainWindowViewModel vm)
                        {
                            if (!vm.CurrentProject.Clips.Any(x => x.FilePath.Equals(item, StringComparison.OrdinalIgnoreCase)))
                            {
                                var clip = new ProjectClip(item);
                                vm.CurrentProject.Clips.Add(clip);
                                QuranVideoMakerUI.ShowDialog(Dialogs.DialogType.ClipImport, clip);
                            }
                        }
                    }
                }
            }
        }

        private void ListBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.Delete)
                {
                    if (SelectedItem != null)
                    {
                        (ItemsSource as ObservableCollection<IProjectClip>).Remove((ProjectClip)SelectedItem);
                    }
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var clip = (sender as FrameworkElement).DataContext as ProjectClip;

            MainWindowViewModel.Instance.CurrentProject.DeleteClips([clip]);
        }

        private void EditLength_Click(object sender, RoutedEventArgs e)
        {
            var clip = (sender as FrameworkElement).DataContext as ProjectClip;

            var dlgResult = QuranVideoMakerUI.ShowDialog(DialogType.EditLength, clip.Length);

            if (dlgResult.Result == true && dlgResult.Data is TimeCode tc && tc != TimeCode.Zero)
            {
                clip.Length = tc;
            }
        }
    }
}
