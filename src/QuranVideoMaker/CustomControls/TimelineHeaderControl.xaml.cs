using QuranVideoMaker.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuranVideoMaker.CustomControls
{
    /// <summary>
    /// Interaction logic for TimelineHeader.xaml
    /// </summary>
    public partial class TimelineHeaderControl : Canvas
    {
        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Project.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(Project), typeof(TimelineHeaderControl), new PropertyMetadata(null, OnProjectChangedCallback));

        public TimelineHeaderControl()
        {
            InitializeComponent();
        }

        private void InitializeHeader()
        {
            if (DataContext == null)
            {
                return;
            }

            this.Children.Clear();

            var max = TimeSpan.FromMinutes(10);
            var totalFrames = max.TotalSeconds * Project.FPS;

            var width = (totalFrames * Constants.TimelinePixelsInSeparator);

            var curX = 0d;
            var spike = true;
            var count = 1;

            while (curX < width)
            {
                this.Children.Add(new Line()
                {
                    X1 = curX,
                    X2 = curX,
                    Y1 = spike ? 24 : 26,
                    Y2 = this.ActualHeight,
                    Stroke = Brushes.WhiteSmoke,
                    StrokeThickness = 0.5
                });

                if (spike)
                {
                    var currentFrame = curX / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom];

                    var txt = new TextBlock()
                    {
                        Text = new TimeCode(currentFrame, Project.FPS).ToString(),
                        Foreground = Brushes.WhiteSmoke,
                        Tag = new TimeLabelInfo() { Number = count, X = curX },
                    };

                    this.Children.Add(txt);

                    Canvas.SetTop(txt, 8);
                    Canvas.SetLeft(txt, curX);
                }

                curX += Constants.TimelinePixelsInSeparator;
                spike = !spike;
            }

            ApplyCurrentZoom();
        }

        private void ApplyCurrentZoom()
        {
            var items = this.Children.OfType<TextBlock>().OrderBy(x => ((TimeLabelInfo)x.Tag).Number);

            foreach (var item in items)
            {
                var info = item.Tag as TimeLabelInfo;
                var currentFrame = info.X / Constants.TimelinePixelsInSeparator * Constants.TimelineZooms[Project.TimelineZoom];
                var currentSecond = Math.Round(currentFrame / Project.FPS, MidpointRounding.ToZero);
                var t = TimeSpan.FromSeconds(currentSecond);
                var currentFrameStr = currentFrame - (currentSecond * Project.FPS);
                item.Text = $"{t:hh\\:mm\\:ss}:{currentFrameStr:00}";
            }

            var frames = Project.GetTotalFrames() + (TimeSpan.FromMinutes(30).TotalSeconds * Project.FPS);

            this.Width = (frames * Constants.TimelinePixelsInSeparator) / Constants.TimelineZooms[Project.TimelineZoom];
        }

        private static void OnProjectChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is Project oldProject)
            {
                oldProject.PropertyChanged -= (d as TimelineHeaderControl).Project_PropertyChanged;
            }

            if (e.NewValue is Project newProject)
            {
                newProject.PropertyChanged -= (d as TimelineHeaderControl).Project_PropertyChanged;
                newProject.PropertyChanged += (d as TimelineHeaderControl).Project_PropertyChanged;

                (d as TimelineHeaderControl).InitializeHeader();
            }
        }

        private void Project_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Project.TimelineZoom))
            {
                ApplyCurrentZoom();
                //InitializeHeader();
            }
        }
    }
}
