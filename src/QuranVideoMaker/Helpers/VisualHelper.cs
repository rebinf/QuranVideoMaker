using System.Windows;
using System.Windows.Media;

namespace QuranVideoMaker.Helpers
{
    public static class VisualHelper
    {
        public static T GetAncestor<T>(FrameworkElement element)
        {
            var parent = VisualTreeHelper.GetParent(element);

            while (parent != null)
            {
                if (parent is T found)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            return default;
        }
    }
}
