using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Dialogs.ViewModels;
using System.Reflection;
using System.Windows;

namespace QuranVideoMaker
{
    public static class QuranVideoMakerUI
    {
        public static DialogResult ShowDialog(DialogType dialogType, params object[] parameters)
        {
            var view = Activator.CreateInstance(GetViewType(dialogType.ToString())) as Window;
            var vm = Activator.CreateInstance(GetViewModelType(dialogType.ToString()), parameters) as DialogViewModelBase;
            view.DataContext = vm;
            view.Owner = MainWindow.Instance;
            return new DialogResult(view.ShowDialog(), vm.Data);
        }

        private static Type GetViewType(string name)
        {
            return GetType($"{name}View");
        }

        private static Type GetViewModelType(string name)
        {
            return GetType($"{name}ViewModel");
        }

        private static Type GetType(string name)
        {
            return Assembly.GetExecutingAssembly().DefinedTypes.FirstOrDefault(x => x.Name == name).AsType();
        }
    }
}
