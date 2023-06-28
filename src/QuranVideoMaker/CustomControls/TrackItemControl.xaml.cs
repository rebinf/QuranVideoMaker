using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Helpers;
using QuranVideoMaker.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for TrackItemControl.xaml
    /// </summary>
    public partial class TrackItemControl : Border
    {
        private TimelineControl _timelineControl;

        public TrackItemControl()
        {
            InitializeComponent();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            _timelineControl ??= VisualHelper.GetAncestor<TimelineControl>(this);

            var project = _timelineControl.Project;

            if (project.SelectedTool == TimelineSelectedTool.SelectionTool && this.DataContext is TrackItem trackItem && !trackItem.IsChangingFadeIn && !trackItem.IsChangingFadeOut)
            {
                var mouseX = e.GetPosition(this).X;

                //if hover left border, else if hover right border
                if (Math.Abs(mouseX - 0) <= 3)
                {
                    this.Cursor = Cursors.SizeWE;
                    resizeBorder.BorderThickness = new Thickness(2, 0, 0, 0);
                }
                else if (Math.Abs(mouseX - this.Width) <= 3)
                {
                    this.Cursor = Cursors.SizeWE;
                    resizeBorder.BorderThickness = new Thickness(0, 0, 2, 0);
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    resizeBorder.BorderThickness = new Thickness(0);
                }
            }
            else if (project.SelectedTool == TimelineSelectedTool.CuttingTool || project.SelectedTool == TimelineSelectedTool.VerseResizer)
            {
                this.Cursor = Cursors.IBeam;
            }
            else
            {
                resizeBorder.BorderThickness = new Thickness(0);
            }

            base.OnPreviewMouseMove(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            resizeBorder.BorderThickness = new Thickness(0);
            base.OnMouseLeave(e);
        }

        [RelayCommand]
        private void OnDoubleClick()
        {
            if (DataContext is QuranTrackItem quranTrackItem)
            {
                QuranVideoMakerUI.ShowDialog(DialogType.QuranTrackItemSettings, quranTrackItem, _timelineControl.Project);
            }
        }
    }
}
