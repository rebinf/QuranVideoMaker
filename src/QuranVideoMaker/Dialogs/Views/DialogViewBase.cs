using QuranVideoMaker.Dialogs.ViewModels;
using System.Windows;

namespace QuranVideoMaker.Dialogs.Views
{
    public class DialogViewBase : Window
    {
        public DialogViewBase()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            this.Loaded += DialogViewBase_Loaded;
        }

        private void DialogViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DialogViewModelBase vm)
            {
                vm.CloseWindow += (result) =>
                {
                    this.DialogResult = result;
                    vm.OnClosed();
                    this.Close();
                };
            }
        }
    }
}
