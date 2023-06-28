using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Helpers;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for TimelineControl.xaml
    /// </summary>
    public partial class TimelineControl : Grid
    {
        private bool _tracksCanvasLeftMouseButtonDown;
        private TrackItemControl _mouseDownElement;
        private Ellipse _mouseDownFadeControl;
        private double _mouseDownX;
        private double _trackMouseDownX;
        private TrackItem _mouseDownTrackItem;
        private bool _resizingLeft;
        private bool _resizingRight;

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Project.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(Project), typeof(TimelineControl), new PropertyMetadata(null, OnProjectChangedCallback));

        public TimelineControl()
        {
            InitializeComponent();
        }

        [RelayCommand]
        private void OnToolSelected(TimelineSelectedTool tool)
        {
            Project.SelectedTool = tool;

            if (Project.SelectedTool == TimelineSelectedTool.AutoVerse)
            {
                if (Project.Tracks.First(x => x.Type == TimelineTrackType.Quran).Items.Count == 0)
                {
                    QuranVideoMakerUI.ShowDialog(DialogType.AddQuran, Project, true);
                }
            }
        }

        private static void OnProjectChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is Project oldProject)
            {
                oldProject.PropertyChanged -= (d as TimelineControl).Project_PropertyChanged;
            }

            if (e.NewValue is Project newProject)
            {
                newProject.PropertyChanged -= (d as TimelineControl).Project_PropertyChanged;
                newProject.PropertyChanged += (d as TimelineControl).Project_PropertyChanged;

                //(d as TimelineControl).InitializeHeader();
            }
        }

        private void Project_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Project.TimelineZoom) || e.PropertyName == nameof(Project.NeedlePositionTime))
            {
                Canvas.SetLeft(needle, Project.NeedlePositionTime.TotalFrames * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[Project.TimelineZoom]);

                needle.BringIntoView();
            }
            else if (e.PropertyName == nameof(Project.SelectedTool))
            {
                OnToolSelected(Project.SelectedTool);
            }
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tracksHScrollView.ScrollToHorizontalOffset(e.NewValue);
        }

        private void TracksCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            if (e.ClickCount > 1)
            {
                return;
            }

            _trackMouseDownX = e.GetPosition(tracksControl).X;

            Project.Tracks.SelectMany(x => x.Items).ToList().ForEach(x => x.IsSelected = false);

            if (e.OriginalSource is FrameworkElement fe && fe.DataContext is TrackItem item)
            {
                var trackItemControl = fe is TrackItemControl tic ? tic : VisualHelper.GetAncestor<TrackItemControl>(fe);
                var mouseX = e.GetPosition(tracksControl).X;
                var itemX = trackItemControl.TranslatePoint(new Point(), tracksControl).X;
                var itemRight = itemX + trackItemControl.Width;
                var frame = Math.Round(mouseX / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);
                var itemFrame = Math.Round(itemX / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                _mouseDownElement = trackItemControl;
                _mouseDownX = e.GetPosition(_mouseDownElement).X;
                _mouseDownTrackItem = item;

                if (Project.SelectedTool == TimelineSelectedTool.SelectionTool)
                {
                    //if left border clicked, else if right border clicked
                    if (Math.Abs(mouseX - itemX) <= 3)
                    {
                        _resizingLeft = true;
                    }
                    else if (Math.Abs(mouseX - itemRight) <= 3)
                    {
                        _resizingRight = true;
                    }
                    else
                    {
                        _resizingLeft = false;
                        _resizingRight = false;
                        _mouseDownTrackItem.IsSelected = true;
                    }
                }
                else if (Project.SelectedTool == TimelineSelectedTool.CuttingTool)
                {
                    Project.CutItem(item, frame);
                }
                else if (Project.SelectedTool == TimelineSelectedTool.VerseResizer)
                {
                    Project.ResizeQuranItem(item, frame);
                }
            }
            else if (e.OriginalSource is Ellipse fadeControl && fadeControl.DataContext is TrackItem trackItem)
            {
                _mouseDownTrackItem = trackItem;
                _mouseDownFadeControl = fadeControl;
                _mouseDownElement = VisualHelper.GetAncestor<TrackItemControl>(fadeControl);
            }
            else
            {
                UpdateNeedlePosition(e.GetPosition(header).X);
            }

            _tracksCanvasLeftMouseButtonDown = true;
        }

        private void TimelineControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDownElement = null;
            _mouseDownTrackItem = null;
            _resizingLeft = false;
            _resizingRight = false;
            _tracksCanvasLeftMouseButtonDown = false;
        }

        private void TimelineControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_tracksCanvasLeftMouseButtonDown && _mouseDownTrackItem != null && !_resizingLeft && !_resizingRight && !_mouseDownTrackItem.IsChangingFadeIn && !_mouseDownTrackItem.IsChangingFadeOut)
            {
                var x = Math.Round(e.GetPosition(tracksControl).X - _mouseDownX);

                if (x <= 0)
                {
                    x = 0;
                }

                var destinationStartFrame = Convert.ToInt32(Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero));
                var destinationEndFrame = Convert.ToInt32(destinationStartFrame + _mouseDownTrackItem.Duration.TotalFrames);
                var resultFrame = destinationStartFrame;

                if (destinationStartFrame <= 0)
                {
                    resultFrame = 0;
                }

                //check for overlaps
                var track = Project.Tracks.First(x => x.Items.Contains(_mouseDownTrackItem));

                TimelineTrack destinationTrack = null;

                if (e.OriginalSource is FrameworkElement element && element.DataContext is TimelineTrack t)
                {
                    destinationTrack = t;
                }

                var trackToCheck = destinationTrack ?? track;
                var itemsToCheck = trackToCheck.Items.Where(x => x != _mouseDownTrackItem).ToArray();

                foreach (var item in itemsToCheck)
                {
                    if (item.Position.TotalFrames < destinationEndFrame && destinationStartFrame <= (item.Position.TotalFrames + item.Duration.TotalFrames))
                    {
                        resultFrame = Convert.ToInt32(item.Position.TotalFrames + item.Duration.TotalFrames);
                    }

                    if (destinationEndFrame >= item.Position.TotalFrames && destinationStartFrame < item.Position.TotalFrames)
                    {
                        resultFrame = Convert.ToInt32(item.Position.TotalFrames - _mouseDownTrackItem.Duration.TotalFrames);
                    }
                }

                _mouseDownTrackItem.Position = new TimeCode(resultFrame, Project.FPS);

                if (destinationTrack != null && destinationTrack != track && _mouseDownTrackItem.IsCompatibleWith(destinationTrack.Type))
                {
                    track.Items.Remove(_mouseDownTrackItem);
                    destinationTrack.Items.Add(_mouseDownTrackItem);
                }
            }
            else if (_tracksCanvasLeftMouseButtonDown && _mouseDownTrackItem != null && _resizingLeft)
            {
                var x = e.GetPosition(tracksControl).X;
                var itemX = _mouseDownElement.TranslatePoint(new Point(), tracksControl).X;
                var itemFrame = Math.Round(itemX / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                var currentMouseFrame = Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                var frameDifference = itemFrame - currentMouseFrame;
                var oldStart = _mouseDownTrackItem.Start;

                _mouseDownTrackItem.Start = new TimeCode(_mouseDownTrackItem.Start.TotalFrames - frameDifference, Project.FPS);

                if (oldStart != _mouseDownTrackItem.Start)
                {
                    _mouseDownTrackItem.Position = new TimeCode(_mouseDownTrackItem.Position.TotalFrames - frameDifference, Project.FPS);
                }
                else if (oldStart == new TimeCode() && _mouseDownTrackItem.Start == new TimeCode())
                {
                    _mouseDownTrackItem.Position = new TimeCode(_mouseDownTrackItem.Position.TotalFrames - frameDifference, Project.FPS);

                    if (_mouseDownTrackItem.UnlimitedSourceLength)
                    {
                        _mouseDownTrackItem.End = new TimeCode(_mouseDownTrackItem.End.TotalFrames + frameDifference, Project.FPS);
                    }
                }

                //if (_mouseDownTrackItem is QuranTrackItem)
                //{
                //    if (Keyboard.IsKeyDown(Key.LeftShift))
                //    {
                //        var leftItem = Project.Tracks.First(x => x.Type == TrackType.Quran).Items.FirstOrDefault(x => x != _mouseDownTrackItem && x.GetRightTime().TotalFrames - 10 <= _mouseDownTrackItem.Position.TotalFrames + 10);

                //        if (leftItem != null)
                //        {
                //            leftItem.End = new TimeCode(_mouseDownTrackItem.Position.TotalFrames - leftItem.Position.TotalFrames, Project.FPS);
                //        }
                //    }
                //}
            }
            else if (_tracksCanvasLeftMouseButtonDown && _mouseDownTrackItem != null && _resizingRight)
            {
                var x = e.GetPosition(tracksControl).X;
                var itemX = _mouseDownElement.TranslatePoint(new Point(), tracksControl).X;
                var itemFrame = Math.Round(itemX / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                var currentMouseFrame = Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                var frameDifference = itemFrame - currentMouseFrame;
                var oldEnd = _mouseDownTrackItem.End;

                _mouseDownTrackItem.End = new TimeCode(_mouseDownTrackItem.Start.TotalFrames - frameDifference, Project.FPS);
            }
            else if (_mouseDownTrackItem != null && (_mouseDownTrackItem.IsChangingFadeIn || _mouseDownTrackItem.IsChangingFadeOut))
            {
                var x = e.GetPosition(_mouseDownElement).X;

                if (_mouseDownTrackItem.IsChangingFadeOut)
                {
                    x -= _mouseDownElement.Width;
                }

                x -= 5;

                var destinationStartFrame = Convert.ToInt32(Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero));

                if (_mouseDownTrackItem.IsChangingFadeIn)
                {
                    if (destinationStartFrame < 0)
                    {
                        destinationStartFrame = 0;
                    }

                    _mouseDownTrackItem.FadeInFrame = destinationStartFrame;
                }
                else
                {
                    if (destinationStartFrame > 0)
                    {
                        destinationStartFrame = 0;
                    }

                    _mouseDownTrackItem.FadeOutFrame = Math.Abs(destinationStartFrame);
                }

                Debug.WriteLine($"changing fade... ({x}, {destinationStartFrame})");
            }
            else if (_tracksCanvasLeftMouseButtonDown && _mouseDownElement == null)
            {
                UpdateNeedlePosition(e.GetPosition(header).X);
            }
        }

        private void TracksScrollView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                {
                    Project.TimelineZoom++;
                }
                else
                {
                    Project.TimelineZoom--;
                }
            }
            else
            {
                tracksHScrollView.ScrollToHorizontalOffset(tracksHScrollView.HorizontalOffset - e.Delta);
            }
        }

        private void UpdateNeedlePosition(double needleXPosition)
        {
            var x = Math.Round(needleXPosition);

            if (x <= 0)
            {
                x = 0;
            }

            var sections = Math.Round(x / 60);

            var frame = Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

            var seconds = Math.Round(frame / Project.FPS, MidpointRounding.ToZero);
            var time = TimeSpan.FromSeconds(seconds);
            var newX = frame * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[Project.TimelineZoom];

            Canvas.SetLeft(needle, newX);
            Debug.WriteLine($"X: {x}, Sections: {sections}, Frame: {frame}, Seconds: {seconds}, ({time})");

            Project.NeedlePositionTime = new TimeCode(frame, Project.FPS);
            Project.RaiseNeedlePositionTimeChanged(Project.NeedlePositionTime);
        }

        private void Timeline_PreviewDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(ProjectClip));

            if (data != null)
            {
                var clip = data as ProjectClip;

                if (e.OriginalSource is FrameworkElement element && element.DataContext is TimelineTrack track && clip.IsCompatibleWith(track.Type))
                {
                    var x = e.GetPosition(header).X;
                    var frame = Math.Round(x / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom], MidpointRounding.ToZero);

                    var position = new TimeCode(frame, Project.FPS);

                    var trackItem = new TrackItem(clip, position, TimeCode.Zero, clip.Length);

                    track.Items.Add(trackItem);
                }
            }
        }

        private void Timeline_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ProjectClip)) is not ProjectClip clip || (e.OriginalSource is FrameworkElement element && element.DataContext is TimelineTrack track && !clip.IsCompatibleWith(track.Type)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void Timeline_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.Delete)
                {
                    foreach (var track in Project.Tracks)
                    {
                        foreach (var item in track.Items.Where(x => x.IsSelected).ToArray())
                        {
                            track.Items.Remove(item);
                        }
                    }
                }
            }
        }
    }
}
