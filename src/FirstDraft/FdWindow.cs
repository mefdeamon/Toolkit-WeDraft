using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FirstDraft
{
    /// <summary>
    /// First Draft自定义窗口
    /// </summary>
    public class FdWindow : Window
    {
        #region Private Member

        /// <summary>
        /// The window resizer helper that keeps the window size correct in various states
        /// </summary>
        private WindowResizer mWindowResizer;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        /// <summary>
        /// The default no corner value
        /// </summary>
        private static readonly CornerRadius zeroWindowCornerRadius = new CornerRadius(0);
        private static readonly Thickness zeroThickness = new Thickness(0);

        /// <summary>
        /// The first setted window corner radius
        /// </summary>
        private readonly CornerRadius normalWindowCornerRadius = new CornerRadius(0);

        /// <summary>
        /// The margin size when window in normal state
        /// </summary>
        private readonly Thickness normalMarginSize = new Thickness(10);

        #endregion

        #region Dependency Properties

        /// <summary>
        /// True if the window is currently being moved/dragged
        /// </summary>
        public bool BeingMoved
        {
            get { return (bool)GetValue(BeingMovedProperty); }
            private set { SetValue(BeingMovedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BeingMoved.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BeingMovedProperty =
            DependencyProperty.Register("BeingMoved", typeof(bool), typeof(FdWindow), new PropertyMetadata(false));


        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public Boolean Borderless
        {
            get { return (Boolean)GetValue(BorderlessProperty); }
            private set { SetValue(BorderlessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Borderless.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderlessProperty =
            DependencyProperty.Register("Borderless", typeof(Boolean), typeof(FdWindow), new PropertyMetadata(false));

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; private set; } = 0;


        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get { return (Thickness)GetValue(ResizeBorderThicknessProperty); }
            private set { SetValue(ResizeBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResizeBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResizeBorderThicknessProperty =
            DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(FdWindow), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding
        {
            get { return (Thickness)GetValue(InnerContentPaddingProperty); }
            private set { SetValue(InnerContentPaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InnerContentPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerContentPaddingProperty =
            DependencyProperty.Register("InnerContentPadding", typeof(Thickness), typeof(FdWindow), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        ///  The drop shadow BlurRadius
        /// </summary>
        public int OuterMarginBlurRadius
        {
            get { return (int)GetValue(OuterMarginBlurRadiusProperty); }
            private set { SetValue(OuterMarginBlurRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OuterMarginBlurRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OuterMarginBlurRadiusProperty =
            DependencyProperty.Register("OuterMarginBlurRadius", typeof(int), typeof(FdWindow), new PropertyMetadata(10));

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSize
        {
            get => (Thickness)GetValue(OuterMarginSizeProperty);
            private set { SetValue(OuterMarginSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OuterMarginSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OuterMarginSizeProperty =
            DependencyProperty.Register("OuterMarginSize", typeof(Thickness), typeof(FdWindow), new PropertyMetadata(new Thickness(10), (s, e) =>
            {
                if (s is FdWindow fdWindow && e.NewValue is Thickness thickness)
                {
                    fdWindow.OuterMarginBlurRadius = (int)thickness.Top;
                }
            }));

        /// <summary>
        /// The rectangle border around the window when docked
        /// </summary>
        public int FlatBorderThickness
        {
            get { return (int)GetValue(FlatBorderThicknessProperty); }
            private set { SetValue(FlatBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FlatBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlatBorderThicknessProperty =
            DependencyProperty.Register("FlatBorderThickness", typeof(int), typeof(FdWindow), new PropertyMetadata(0));

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius
        {
            get { return (CornerRadius)GetValue(WindowCornerRadiusProperty); }
            private set { SetValue(WindowCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowCornerRadiusProperty =
            DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(FdWindow), new PropertyMetadata(new CornerRadius(0), (s, e) =>
            {

            }));

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int CaptionHeight
        {
            get { return (int)GetValue(CaptionHeightProperty); }
            set { SetValue(CaptionHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptionHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(int), typeof(FdWindow), new PropertyMetadata(32, (d, e) => ((FdWindow)d).WindowResized()));

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength CaptionHeightGridLength
        {
            get { return (GridLength)GetValue(CaptionHeightGridLengthProperty); }
            private set { SetValue(CaptionHeightGridLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptionHeightGridLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionHeightGridLengthProperty =
            DependencyProperty.Register("CaptionHeightGridLength", typeof(GridLength), typeof(FdWindow), new PropertyMetadata(new GridLength(0)));


        #endregion

        #region Constructor

        static FdWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FdWindow), new FrameworkPropertyMetadata(typeof(FdWindow)));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FdWindow()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;

            // Listen out for the window resizing
            this.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => this.Close()));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (s, e) => this.WindowState = WindowState.Maximized, (s, e) => e.CanExecute = true));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (s, e) => this.WindowState = WindowState.Minimized, (s, e) => e.CanExecute = true));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (s, e) => this.WindowState = WindowState.Normal, (s, e) => e.CanExecute = true));


            // Fix window resize issue
            mWindowResizer = new WindowResizer(this);

            // Listen out for dock changes
            mWindowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // On window being moved/dragged
            mWindowResizer.WindowStartedMove += () =>
            {
                // Update being moved flag
                BeingMoved = true;
            };

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            mWindowResizer.WindowFinishedMove += () =>
            {
                // Update being moved flag
                BeingMoved = false;

                // Check for moved to top of window and not at an edge
                if (mDockPosition == WindowDockPosition.Undocked && this.Top == mWindowResizer.CurrentScreenSize.Top)
                    // If so, move it to the true top (the border size)
                    this.Top = -OuterMarginSize.Top;
            };

            WindowResized();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            Borderless = (this.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

            WindowCornerRadius = Borderless ? zeroWindowCornerRadius : normalWindowCornerRadius;

            if (this.WindowState == WindowState.Maximized)
            {
                OuterMarginSize = mWindowResizer.CurrentMonitorMargin;
            }
            else if (Borderless)
            {
                OuterMarginSize = zeroThickness;
            }
            else
            {
                OuterMarginSize = normalMarginSize;
            }

            FlatBorderThickness = Borderless && this.WindowState != WindowState.Maximized ? 1 : 0;
            ResizeBorder = this.WindowState == WindowState.Maximized ? 0 : 4;
            ResizeBorderThickness = new Thickness(OuterMarginSize.Left + ResizeBorder,
                                                                OuterMarginSize.Top + ResizeBorder,
                                                                OuterMarginSize.Right + ResizeBorder,
                                                                OuterMarginSize.Bottom + ResizeBorder);

            CaptionHeightGridLength = new GridLength(CaptionHeight);
        }

        #endregion
    }

    #region DLL Helper Structures & WindowResizer

    /// <summary>
    /// Fixes the issue with Windows of Style <see cref="WindowStyle.None"/> covering the taskbar
    /// </summary>
    public class WindowResizer
    {
        #region Private Members

        /// <summary>
        /// The window to handle the resizing for
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The last calculated available screen size
        /// </summary>
        private Rect mScreenSize = new Rect();

        /// <summary>
        /// How close to the edge the window has to be to be detected as at the edge of the screen
        /// </summary>
        private int mEdgeTolerance = 1;

        /// <summary>
        /// The transform matrix used to convert WPF sizes to screen pixels
        /// </summary>
        private DpiScale? mMonitorDpi;

        /// <summary>
        /// The last screen the window was on
        /// </summary>
        private IntPtr mLastScreen;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mLastDock = WindowDockPosition.Undocked;

        /// <summary>
        /// A flag indicating if the window is currently being moved/dragged
        /// </summary>
        private bool mBeingMoved = false;

        #endregion

        #region DLL Imports

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorOptions dwFlags);

        #endregion

        #region Public Events

        /// <summary>
        /// Called when the window dock position changes
        /// </summary>
        public event Action<WindowDockPosition> WindowDockChanged = (dock) => { };

        /// <summary>
        /// Called when the window starts being moved/dragged
        /// </summary>
        public event Action WindowStartedMove = () => { };

        /// <summary>
        /// Called when the window has been moved/dragged and then finished
        /// </summary>
        public event Action WindowFinishedMove = () => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// The size and position of the current monitor the window is on
        /// </summary>
        public Rectangle CurrentMonitorSize { get; set; } = new Rectangle();

        /// <summary>
        /// The margin around the window for the current window to compensate for any non-usable area
        /// such as the task bar
        /// </summary>
        public Thickness CurrentMonitorMargin { get; private set; } = new Thickness();

        /// <summary>
        /// The size and position of the current screen in relation to the multi-screen desktop
        /// For example a second monitor on the right will have a Left position of
        /// the X resolution of the screens on the left
        /// </summary>
        public Rect CurrentScreenSize => mScreenSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window">The window to monitor and correctly maximize</param>
        /// <param name="adjustSize">The callback for the host to adjust the maximum available size if needed</param>
        public WindowResizer(Window window)
        {
            mWindow = window;

            // Listen out for source initialized to setup
            mWindow.SourceInitialized += Window_SourceInitialized;

            // Monitor for edge docking
            mWindow.SizeChanged += Window_SizeChanged;
            mWindow.LocationChanged += Window_LocationChanged;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize and hook into the windows message pump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            // Get the handle of this window
            var handle = (new WindowInteropHelper(mWindow)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            // If not found, end
            if (handleSource == null)
                return;

            // Hook into it's Windows messages
            handleSource.AddHook(WindowProc);
        }

        #endregion

        #region Edge Docking

        /// <summary>
        /// Monitor for moving of the window and constantly check for docked positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Window_SizeChanged(null, null);
        }

        /// <summary>
        /// Monitors for size changes and detects if the window has been docked (Aero snap) to an edge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Make sure our monitor info is up-to-date
            WmGetMinMaxInfo(IntPtr.Zero, IntPtr.Zero);

            // Get the monitor transform for the current position
            mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);

            // Cannot calculate size until we know monitor scale
            if (mMonitorDpi == null)
                return;

            // Get window rectangle
            var top = mWindow.Top;
            var left = mWindow.Left;
            var bottom = top + mWindow.Height;
            var right = left + mWindow.Width;

            // Get window position/size in device pixels
            var windowTopLeft = new Point(left * mMonitorDpi.Value.DpiScaleX, top * mMonitorDpi.Value.DpiScaleX);
            var windowBottomRight = new Point(right * mMonitorDpi.Value.DpiScaleX, bottom * mMonitorDpi.Value.DpiScaleX);

            // Check for edges docked
            var edgedTop = windowTopLeft.Y <= (mScreenSize.Top + mEdgeTolerance) && windowTopLeft.Y >= (mScreenSize.Top - mEdgeTolerance);
            var edgedLeft = windowTopLeft.X <= (mScreenSize.Left + mEdgeTolerance) && windowTopLeft.X >= (mScreenSize.Left - mEdgeTolerance);
            var edgedBottom = windowBottomRight.Y >= (mScreenSize.Bottom - mEdgeTolerance) && windowBottomRight.Y <= (mScreenSize.Bottom + mEdgeTolerance);
            var edgedRight = windowBottomRight.X >= (mScreenSize.Right - mEdgeTolerance) && windowBottomRight.X <= (mScreenSize.Right + mEdgeTolerance);

            // Get docked position
            var dock = WindowDockPosition.Undocked;

            // Left docking
            if (edgedTop && edgedBottom && edgedLeft)
                dock = WindowDockPosition.Left;
            // Right docking
            else if (edgedTop && edgedBottom && edgedRight)
                dock = WindowDockPosition.Right;
            // Top/bottom
            else if (edgedTop && edgedBottom)
                dock = WindowDockPosition.TopBottom;
            // Top-left
            else if (edgedTop && edgedLeft)
                dock = WindowDockPosition.TopLeft;
            // Top-right
            else if (edgedTop && edgedRight)
                dock = WindowDockPosition.TopRight;
            // Bottom-left
            else if (edgedBottom && edgedLeft)
                dock = WindowDockPosition.BottomLeft;
            // Bottom-right
            else if (edgedBottom && edgedRight)
                dock = WindowDockPosition.BottomRight;

            // None
            else
                dock = WindowDockPosition.Undocked;

            // If dock has changed
            if (dock != mLastDock)
                // Inform listeners
                WindowDockChanged(dock);

            // Save last dock position
            mLastDock = dock;
        }

        #endregion

        #region Windows Message Pump

        /// <summary>
        /// Listens out for all windows messages for this window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Handle the GetMinMaxInfo of the Window
                case 0x0024: // WM_GETMINMAXINFO
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;

                // Once the window starts being moved
                case 0x0231: // WM_ENTERSIZEMOVE
                    mBeingMoved = true;
                    WindowStartedMove();
                    break;

                // Once the window has finished being moved
                case 0x0232: // WM_EXITSIZEMOVE
                    mBeingMoved = false;
                    WindowFinishedMove();
                    break;
            }

            return (IntPtr)0;
        }

        #endregion

        /// <summary>
        /// Get the min/max window size for this window
        /// Correctly accounting for the task bar size and position
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // Get the point position to determine what screen we are on
            GetCursorPos(out var lMousePosition);

            // Now get the current screen
            var lCurrentScreen = mBeingMoved ?
                // If being dragged get it from the mouse position
                MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONULL) :
                // Otherwise get it from the window position (for example being moved via Win + Arrow)
                // in case the mouse is on another monitor
                MonitorFromWindow(hwnd, MonitorOptions.MONITOR_DEFAULTTONULL);

            var lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            // Try and get the current screen information
            var lCurrentScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
                return;

            // Try and get the primary screen information
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
                return;

            // NOTE: Always update it
            // If this has changed from the last one, update the transform
            //if (lCurrentScreen != mLastScreen || mMonitorDpi == null)
            mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);

            // Store last know screen
            mLastScreen = lCurrentScreen;

            // Get work area sizes and rations
            var currentX = lCurrentScreenInfo.RCWork.Left - lCurrentScreenInfo.RCMonitor.Left;
            var currentY = lCurrentScreenInfo.RCWork.Top - lCurrentScreenInfo.RCMonitor.Top;
            var currentWidth = (lCurrentScreenInfo.RCWork.Right - lCurrentScreenInfo.RCWork.Left);
            var currentHeight = (lCurrentScreenInfo.RCWork.Bottom - lCurrentScreenInfo.RCWork.Top);
            var currentRatio = (float)currentWidth / (float)currentHeight;

            var primaryX = lPrimaryScreenInfo.RCWork.Left - lPrimaryScreenInfo.RCMonitor.Left;
            var primaryY = lPrimaryScreenInfo.RCWork.Top - lPrimaryScreenInfo.RCMonitor.Top;
            var primaryWidth = (lPrimaryScreenInfo.RCWork.Right - lPrimaryScreenInfo.RCWork.Left);
            var primaryHeight = (lPrimaryScreenInfo.RCWork.Bottom - lPrimaryScreenInfo.RCWork.Top);
            var primaryRatio = (float)primaryWidth / (float)primaryHeight;

            if (lParam != IntPtr.Zero)
            {
                // Get min/max structure to fill with information
                var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

                //
                //   NOTE: The below setting of max sizes we no longer do
                //         as through observations, it appears Windows works
                //         correctly only when the max window size is set to
                //         EXACTLY the size of the primary window
                // 
                //         Anything else and the behavior is wrong and the max
                //         window width on a secondary monitor if larger than the
                //         primary then goes too large
                //
                //          lMmi.PointMaxPosition.X = 0;
                //          lMmi.PointMaxPosition.Y = 0;
                //          lMmi.PointMaxSize.X = lCurrentScreenInfo.RCMonitor.Right - lCurrentScreenInfo.RCMonitor.Left;
                //          lMmi.PointMaxSize.Y = lCurrentScreenInfo.RCMonitor.Bottom - lCurrentScreenInfo.RCMonitor.Top;
                //
                //         Instead we now just add a margin to the window itself
                //         to compensate when maximized
                // 
                //
                // NOTE: rcMonitor is the monitor size
                //       rcWork is the available screen size (so the area inside the task bar start menu for example)

                // Size limits (used by Windows when maximized)
                // relative to 0,0 being the current screens top-left corner

                // Set to primary monitor size
                lMmi.PointMaxPosition.X = lPrimaryScreenInfo.RCMonitor.Left;
                lMmi.PointMaxPosition.Y = lPrimaryScreenInfo.RCMonitor.Top;
                lMmi.PointMaxSize.X = lPrimaryScreenInfo.RCMonitor.Right;
                lMmi.PointMaxSize.Y = lPrimaryScreenInfo.RCMonitor.Bottom;

                // Set min size
                var minSize = new Point(mWindow.MinWidth * mMonitorDpi.Value.DpiScaleX, mWindow.MinHeight * mMonitorDpi.Value.DpiScaleX);
                lMmi.PointMinTrackSize.X = (int)minSize.X;
                lMmi.PointMinTrackSize.Y = (int)minSize.Y;

                // Now we have the max size, allow the host to tweak as needed
                Marshal.StructureToPtr(lMmi, lParam, true);
            }

            // Set monitor size
            CurrentMonitorSize = new Rectangle(currentX, currentY, currentWidth + currentX, currentHeight + currentY);

            // Get margin around window
            CurrentMonitorMargin = new Thickness(
                (lCurrentScreenInfo.RCWork.Left - lCurrentScreenInfo.RCMonitor.Left) / mMonitorDpi.Value.DpiScaleX,
                (lCurrentScreenInfo.RCWork.Top - lCurrentScreenInfo.RCMonitor.Top) / mMonitorDpi.Value.DpiScaleY,
                (lCurrentScreenInfo.RCMonitor.Right - lCurrentScreenInfo.RCWork.Right) / mMonitorDpi.Value.DpiScaleX,
                (lCurrentScreenInfo.RCMonitor.Bottom - lCurrentScreenInfo.RCWork.Bottom) / mMonitorDpi.Value.DpiScaleY
                );

            // Store new size
            mScreenSize = new Rect(lCurrentScreenInfo.RCWork.Left, lCurrentScreenInfo.RCWork.Top, currentWidth, currentHeight);
        }

        /// <summary>
        /// Gets the current cursor position in screen coordinates relative to an entire multi-desktop position
        /// </summary>
        /// <returns></returns>
        public Point GetCursorPosition()
        {
            // Get mouse position
            GetCursorPos(out var lMousePosition);

            // Apply DPI scaling
            return new Point(lMousePosition.X / mMonitorDpi.Value.DpiScaleX, lMousePosition.Y / mMonitorDpi.Value.DpiScaleY);
        }
    }

    /// <summary>
    /// The dock position of the window
    /// </summary>
    public enum WindowDockPosition
    {
        /// <summary>
        /// Not docked
        /// </summary>
        Undocked = 0,
        /// <summary>
        /// Docked to the left of the screen
        /// </summary>
        Left = 1,
        /// <summary>
        /// Docked to the right of the screen
        /// </summary>
        Right = 2,
        /// <summary>
        /// Docked to the top/bottom of the screen
        /// </summary>
        TopBottom = 3,
        /// <summary>
        /// Docked to the top-left of the screen
        /// </summary>
        TopLeft = 4,
        /// <summary>
        /// Docked to the top-right of the screen
        /// </summary>
        TopRight = 5,
        /// <summary>
        /// Docked to the bottom-left of the screen
        /// </summary>
        BottomLeft = 6,
        /// <summary>
        /// Docked to the bottom-right of the screen
        /// </summary>
        BottomRight = 7,
    }

    public enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
#pragma warning disable IDE1006 // Naming Styles
        public int CBSize = Marshal.SizeOf(typeof(MONITORINFO));
        public Rectangle RCMonitor = new Rectangle();
        public Rectangle RCWork = new Rectangle();
        public int DWFlags = 0;
#pragma warning restore IDE1006 // Naming Styles
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
#pragma warning disable IDE1006 // Naming Styles
        public int Left, Top, Right, Bottom;
#pragma warning restore IDE1006 // Naming Styles

        public Rectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
#pragma warning disable IDE1006 // Naming Styles
        public POINT PointReserved;
        public POINT PointMaxSize;
        public POINT PointMaxPosition;
        public POINT PointMinTrackSize;
        public POINT PointMaxTrackSize;
#pragma warning restore IDE1006 // Naming Styles
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>
        /// x coordinate of point.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public int X;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// y coordinate of point.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public int Y;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Construct a point of coordinates (x,y).
        /// </summary>
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }

    #endregion
}
