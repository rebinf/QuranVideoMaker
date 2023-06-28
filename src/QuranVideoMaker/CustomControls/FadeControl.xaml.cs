using QuranVideoMaker.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for FadeControl.xaml
    /// </summary>
    public partial class FadeControl : Grid
    {
        public FadeControlType ControlType
        {
            get { return (FadeControlType)GetValue(ControlTypeProperty); }
            set { SetValue(ControlTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlTypeProperty =
            DependencyProperty.Register("ControlType", typeof(FadeControlType), typeof(FadeControl), new PropertyMetadata(FadeControlType.Left));

        public FadeControl()
        {
            InitializeComponent();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (ControlType == FadeControlType.Left)
            {
                (DataContext as ITrackItem).IsChangingFadeIn = true;
            }
            else
            {
                (DataContext as ITrackItem).IsChangingFadeOut = true;
            }

            this.CaptureMouse();

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (ControlType == FadeControlType.Left)
            {
                (DataContext as ITrackItem).IsChangingFadeIn = false;
            }
            else
            {
                (DataContext as ITrackItem).IsChangingFadeOut = false;
            }

            this.ReleaseMouseCapture();

            base.OnPreviewMouseLeftButtonUp(e);
        }
    }
}
