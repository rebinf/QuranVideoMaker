using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (_timelineControl == null)
            {
                _timelineControl = VisualHelper.GetAncestor<TimelineControl>(this);
            }

            if (_timelineControl.SelectedTool == TimelineSelectedTool.SelectionTool && this.DataContext is TrackItemBase trackItem && !trackItem.IsChangingFadeIn && !trackItem.IsChangingFadeOut)
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
            else if (_timelineControl.SelectedTool == TimelineSelectedTool.CuttingTool || _timelineControl.SelectedTool == TimelineSelectedTool.VerseResizer)
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
