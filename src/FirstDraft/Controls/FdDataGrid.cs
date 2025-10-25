using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FirstDraft.Controls
{

    [TemplatePart(Name = "PART_DataGrid", Type = typeof(DataGrid))]
    [TemplatePart(Name = "PART_FirstPageButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PreviousPageButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextPageButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_LastPageButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PageSizeComboBox", Type = typeof(ComboBox))]
    [TemplatePart(Name = "PART_PageInfoTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_GoToPageTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_GoToPageButton", Type = typeof(Button))]
    public class FdDataGrid : Control
    {
        #region 依赖属性

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(FdDataGrid),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(ObservableCollection<DataGridColumn>), typeof(FdDataGrid),
                new PropertyMetadata(new ObservableCollection<DataGridColumn>(), OnColumnsChanged));

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(FdDataGrid),
                new PropertyMetadata(1, OnCurrentPageChanged));

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(FdDataGrid),
                new PropertyMetadata(20, OnPageSizeChanged));

        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register("TotalPages", typeof(int), typeof(FdDataGrid),
                new PropertyMetadata(1));

        public static readonly DependencyProperty TotalItemsProperty =
            DependencyProperty.Register("TotalItems", typeof(int), typeof(FdDataGrid),
                new PropertyMetadata(0));

        public static readonly DependencyProperty PageInfoProperty =
            DependencyProperty.Register("PageInfo", typeof(string), typeof(FdDataGrid),
                new PropertyMetadata("第 1 页 / 共 1 页"));

        public static readonly DependencyProperty ShowPaginationProperty =
            DependencyProperty.Register("ShowPagination", typeof(bool), typeof(FdDataGrid),
                new PropertyMetadata(true));

        public static readonly DependencyProperty AutoGenerateColumnsProperty =
            DependencyProperty.Register("AutoGenerateColumns", typeof(bool), typeof(FdDataGrid),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CanUserAddRowsProperty =
            DependencyProperty.Register("CanUserAddRows", typeof(bool), typeof(FdDataGrid),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(FdDataGrid),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CanUserSortColumnsProperty =
            DependencyProperty.Register("CanUserSortColumns", typeof(bool), typeof(FdDataGrid),
                new PropertyMetadata(true));

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(DataGridSelectionMode), typeof(FdDataGrid),
                new PropertyMetadata(DataGridSelectionMode.Extended));

        public static readonly DependencyProperty SelectionUnitProperty =
            DependencyProperty.Register("SelectionUnit", typeof(DataGridSelectionUnit), typeof(FdDataGrid),
                new PropertyMetadata(DataGridSelectionUnit.FullRow));

        // 延迟刷新属性（毫秒）
        public static readonly DependencyProperty RefreshDelayProperty =
            DependencyProperty.Register("RefreshDelay", typeof(int), typeof(FdDataGrid),
                new PropertyMetadata(100));

        // SelectedItem 依赖属性 - 启用双向绑定
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(FdDataGrid),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        // SelectedItems 依赖属性
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(FdDataGrid),
                new PropertyMetadata(null));

        // SelectedIndex 依赖属性
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(FdDataGrid),
                new FrameworkPropertyMetadata(-1,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedIndexChanged));

        #endregion

        #region 路由事件

        // SelectionChanged 路由事件
        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble,
                typeof(SelectionChangedEventHandler), typeof(FdDataGrid));

        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        #endregion

        #region 公共属性

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public ObservableCollection<DataGridColumn> Columns
        {
            get => (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            private set => SetValue(TotalPagesProperty, value);
        }

        public int TotalItems
        {
            get => (int)GetValue(TotalItemsProperty);
            private set => SetValue(TotalItemsProperty, value);
        }

        public string PageInfo
        {
            get => (string)GetValue(PageInfoProperty);
            private set => SetValue(PageInfoProperty, value);
        }

        public bool ShowPagination
        {
            get => (bool)GetValue(ShowPaginationProperty);
            set => SetValue(ShowPaginationProperty, value);
        }

        public bool AutoGenerateColumns
        {
            get => (bool)GetValue(AutoGenerateColumnsProperty);
            set => SetValue(AutoGenerateColumnsProperty, value);
        }

        public bool CanUserAddRows
        {
            get => (bool)GetValue(CanUserAddRowsProperty);
            set => SetValue(CanUserAddRowsProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public bool CanUserSortColumns
        {
            get => (bool)GetValue(CanUserSortColumnsProperty);
            set => SetValue(CanUserSortColumnsProperty, value);
        }

        public DataGridSelectionMode SelectionMode
        {
            get => (DataGridSelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public DataGridSelectionUnit SelectionUnit
        {
            get => (DataGridSelectionUnit)GetValue(SelectionUnitProperty);
            set => SetValue(SelectionUnitProperty, value);
        }

        public int RefreshDelay
        {
            get => (int)GetValue(RefreshDelayProperty);
            set => SetValue(RefreshDelayProperty, value);
        }

        // SelectedItem 公共属性
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // SelectedItems 公共属性
        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            private set => SetValue(SelectedItemsProperty, value);
        }

        // SelectedIndex 公共属性
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        #endregion

        #region 私有字段

        private DataGrid _dataGrid;
        private Button _firstPageButton;
        private Button _previousPageButton;
        private Button _nextPageButton;
        private Button _lastPageButton;
        private ComboBox _pageSizeComboBox;
        private TextBlock _pageInfoTextBlock;
        private TextBox _goToPageTextBox;
        private Button _goToPageButton;

        private INotifyCollectionChanged _notifyCollection;
        private DispatcherTimer _refreshTimer;
        private bool _needsRefresh;
        private IEnumerable _currentPageData;
        private List<int> pageSizes = new List<int>() { 10, 20, 50, 100, 200, 500, 1000 };

        // 选择同步控制字段
        private bool _isInternalSelectionChange;

        #endregion

        #region 构造函数

        static FdDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FdDataGrid),
                new FrameworkPropertyMetadata(typeof(FdDataGrid)));
        }

        public FdDataGrid()
        {
            Columns = new ObservableCollection<DataGridColumn>();
            SelectedItems = new ArrayList(); // 初始化 SelectedItems
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            // 初始化延迟刷新定时器
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Tick += OnRefreshTimerTick;
        }

        #endregion

        #region 模板重写

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 获取模板部件
            _dataGrid = GetTemplateChild("PART_DataGrid") as DataGrid;
            _firstPageButton = GetTemplateChild("PART_FirstPageButton") as Button;
            _previousPageButton = GetTemplateChild("PART_PreviousPageButton") as Button;
            _nextPageButton = GetTemplateChild("PART_NextPageButton") as Button;
            _lastPageButton = GetTemplateChild("PART_LastPageButton") as Button;
            _pageSizeComboBox = GetTemplateChild("PART_PageSizeComboBox") as ComboBox;
            _pageInfoTextBlock = GetTemplateChild("PART_PageInfoTextBlock") as TextBlock;
            _goToPageTextBox = GetTemplateChild("PART_GoToPageTextBox") as TextBox;
            _goToPageButton = GetTemplateChild("PART_GoToPageButton") as Button;

            // 绑定事件
            if (_firstPageButton != null)
                _firstPageButton.Click += OnFirstPageClick;
            if (_previousPageButton != null)
                _previousPageButton.Click += OnPreviousPageClick;
            if (_nextPageButton != null)
                _nextPageButton.Click += OnNextPageClick;
            if (_lastPageButton != null)
                _lastPageButton.Click += OnLastPageClick;
            if (_pageSizeComboBox != null)
            {
                _pageSizeComboBox.Items.Clear();
                _pageSizeComboBox.ItemsSource = pageSizes;
                _pageSizeComboBox.SelectedItem = PageSize;
                _pageSizeComboBox.SelectionChanged += OnPageSizeChanged;
                _pageSizeComboBox.PreviewTextInput += OnPageSizePreviewTextInput;
                _pageSizeComboBox.KeyDown += OnPageSizeKeyDown;
            }
            if (_goToPageTextBox != null)
            {
                _goToPageTextBox.PreviewTextInput += OnGoToPagePreviewTextInput;
                _goToPageTextBox.KeyDown += OnGoToPageKeyDown;
            }
            if (_goToPageButton != null)
            {
                _goToPageButton.Click += OnGoToPageClick;
            }

            // 初始化DataGrid
            if (_dataGrid != null)
            {
                _dataGrid.AutoGenerateColumns = AutoGenerateColumns;
                _dataGrid.CanUserAddRows = CanUserAddRows;
                _dataGrid.IsReadOnly = IsReadOnly;
                _dataGrid.CanUserSortColumns = CanUserSortColumns;
                _dataGrid.SelectionMode = SelectionMode;
                _dataGrid.SelectionUnit = SelectionUnit;

                // 绑定选择事件
                _dataGrid.SelectionChanged += OnDataGridSelectionChanged;

                UpdateDataGridColumns();

                // 同步初始选择状态
                SyncSelectionToDataGrid();
            }

            // 如果ItemsSource已经设置，初始化分页
            if (ItemsSource != null)
            {
                InitializePagination();
            }

            UpdatePaginationControls();
        }

        #endregion

        #region 事件处理

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializePagination();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            CleanupPagination();
            _refreshTimer.Stop();
        }

        private void OnFirstPageClick(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
        }

        private void OnPreviousPageClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        private void OnNextPageClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }

        private void OnLastPageClick(object sender, RoutedEventArgs e)
        {
            CurrentPage = TotalPages;
        }

        private void OnPageSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_pageSizeComboBox?.SelectedItem != null)
            {
                if (int.TryParse(_pageSizeComboBox?.SelectedItem.ToString(), out int newSize))
                {
                    if (PageSize != newSize)
                        PageSize = newSize;
                }
            }
        }

        private void AddCustomPageSizeItem()
        {
            if (_pageSizeComboBox?.Text != null)
            {
                if (int.TryParse(_pageSizeComboBox?.Text.ToString(), out int newSize))
                {
                    if (!pageSizes.Contains(newSize))
                    {
                        pageSizes.Add(newSize);
                        _pageSizeComboBox.ItemsSource = null;
                        _pageSizeComboBox.ItemsSource = pageSizes;

                        // 强制UI更新
                        _pageSizeComboBox.InvalidateVisual();
                        _pageSizeComboBox.UpdateLayout();
                    }

                    if (PageSize != newSize)
                        PageSize = newSize;
                    var a = _pageSizeComboBox?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    if (a != true)
                        FocusManager.SetFocusedElement(FocusManager.GetFocusScope(_pageSizeComboBox), null);
                }
            }
        }

        private void OnPageSizeKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddCustomPageSizeItem();
                e.Handled = true;
            }
        }

        private void OnPageSizePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #region 转到指定页功能

        private void OnGoToPagePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnGoToPageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleGoToPage();
                e.Handled = true;
            }
        }

        private void OnGoToPageClick(object sender, RoutedEventArgs e)
        {
            HandleGoToPage();
        }

        private void HandleGoToPage()
        {
            var textBox = _goToPageTextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
            {
                if (int.TryParse(textBox.Text, out int pageNumber))
                {
                    if (pageNumber >= 1 && pageNumber <= TotalPages)
                    {
                        CurrentPage = pageNumber;
                    }
                    else
                    {
                        MessageBox.Show($"请输入 1 到 {TotalPages} 之间的页码", "提示",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

            textBox?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        #endregion

        // 延迟刷新定时器
        private void OnRefreshTimerTick(object sender, EventArgs e)
        {
            _refreshTimer.Stop();
            if (_needsRefresh)
            {
                RefreshData();
                _needsRefresh = false;
            }
        }

        // 监听集合变化 - 使用延迟刷新
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 集合变化时立即计算总页数
            CalculateTotalPages();

            _needsRefresh = true;
            _refreshTimer.Interval = TimeSpan.FromMilliseconds(RefreshDelay);
            _refreshTimer.Stop();
            _refreshTimer.Start();
        }

        #endregion

        #region 选择功能实现

        // DataGrid 选择变化事件处理
        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SyncSelectionFromDataGrid(e);
        }

        // 从 DataGrid 同步选择状态到 FdDataGrid 属性
        private void SyncSelectionFromDataGrid(SelectionChangedEventArgs e)
        {
            if (_dataGrid == null) return;

            if (_isInternalSelectionChange) return;

            _isInternalSelectionChange = true;

            try
            {
                // 更新 SelectedItem - 使用 SetCurrentValue 保持绑定
                SetCurrentValue(SelectedItemProperty, _dataGrid.SelectedItem);

                // 更新 SelectedIndex
                SetCurrentValue(SelectedIndexProperty, _dataGrid.SelectedIndex);

                // 更新 SelectedItems
                if (_dataGrid.SelectedItems != null)
                {
                    var newSelectedItems = new ArrayList();
                    foreach (var item in _dataGrid.SelectedItems)
                    {
                        newSelectedItems.Add(item);
                    }
                    SetCurrentValue(SelectedItemsProperty, newSelectedItems);
                }

                // 触发选择改变事件
                var selectionChangedArgs = new SelectionChangedEventArgs(SelectionChangedEvent,
                                        e?.RemovedItems?.Cast<object>().ToList() ?? new List<object>(),
                                        e?.AddedItems?.Cast<object>().ToList() ?? new List<object>());
                OnSelectionChanged(selectionChangedArgs);
            }
            finally
            {
                _isInternalSelectionChange = false;
            }
        }

        // 从 FdDataGrid 属性同步选择状态到 DataGrid
        private void SyncSelectionToDataGrid()
        {
            if (_dataGrid == null) return;

            if (_isInternalSelectionChange) return;

            _isInternalSelectionChange = true;

            try
            {
                _dataGrid.SelectionChanged -= OnDataGridSelectionChanged;

                // 同步 SelectedItem
                _dataGrid.SelectedItem = SelectedItem;

                // 同步 SelectedIndex
                _dataGrid.SelectedIndex = SelectedIndex;

                // 同步 SelectedItems (对于多选模式)
                if ((SelectionMode == DataGridSelectionMode.Extended) && SelectedItems != null)
                {
                    _dataGrid.SelectedItems.Clear();
                    foreach (var item in SelectedItems)
                    {
                        _dataGrid.SelectedItems.Add(item);
                    }
                }
            }
            finally
            {
                _dataGrid.SelectionChanged += OnDataGridSelectionChanged;
                _isInternalSelectionChange = false;
            }
        }

        // SelectedItem 属性变更回调
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;
            if (!control._isInternalSelectionChange)
            {
                control.SyncSelectionToDataGrid();
            }
        }

        // SelectedIndex 属性变更回调
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;
            if (!control._isInternalSelectionChange)
            {
                control.SyncSelectionToDataGrid();
            }
        }

        #endregion

        #region 私有方法 - 高性能实现

        private void InitializePagination()
        {
            CleanupPagination();

            if (ItemsSource != null)
            {
                // 监听集合变化
                _notifyCollection = ItemsSource as INotifyCollectionChanged;
                if (_notifyCollection != null)
                {
                    _notifyCollection.CollectionChanged += OnCollectionChanged;
                }

                CalculateTotalPages();
                UpdatePageInfo();
                RefreshData();
            }
        }

        private void CleanupPagination()
        {
            if (_notifyCollection != null)
            {
                _notifyCollection.CollectionChanged -= OnCollectionChanged;
                _notifyCollection = null;
            }
        }

        private void UpdateDataGridColumns()
        {
            if (_dataGrid == null) return;

            _dataGrid.Columns.Clear();
            foreach (var column in Columns)
            {
                _dataGrid.Columns.Add(column);
            }
        }

        // 高性能数据切片
        private IEnumerable GetCurrentPageData()
        {
            if (ItemsSource == null) return null;

            // 确保总页数是最新的
            CalculateTotalPages();

            var totalCount = GetTotalItemCount();
            if (totalCount == 0) return new List<object>();

            int startIndex = (CurrentPage - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, totalCount);

            // 使用LINQ Skip和Take进行高效分页
            if (ItemsSource is IList list)
            {
                // 对于IList，直接使用索引
                var pageData = new List<object>();
                for (int i = startIndex; i < endIndex; i++)
                {
                    pageData.Add(list[i]);
                }
                return pageData;
            }
            else
            {
                // 对于IEnumerable，使用LINQ
                return ItemsSource.Cast<object>().Skip(startIndex).Take(PageSize).ToList();
            }
        }

        private int GetTotalItemCount()
        {
            if (ItemsSource is ICollection collection)
                return collection.Count;

            if (ItemsSource != null)
                return ItemsSource.Cast<object>().Count();

            return 0;
        }

        private void CalculateTotalPages()
        {
            try
            {
                int totalItems = GetTotalItemCount();
                SetTotalItems(totalItems);

                int totalPages = totalItems == 0 ? 1 : (int)Math.Ceiling((double)totalItems / PageSize);
                SetTotalPages(totalPages);

                // 确保当前页在有效范围内
                int currentPage = CurrentPage;
                if (currentPage > totalPages && totalPages > 0)
                {
                    CurrentPage = totalPages;
                }
                else if (currentPage < 1 && totalPages > 0)
                {
                    CurrentPage = 1;
                }
                else if (totalPages == 0)
                {
                    CurrentPage = 1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CalculateTotalPages error: {ex.Message}");
                // 设置默认值
                SetTotalItems(0);
                SetTotalPages(1);
                CurrentPage = 1;
            }
        }

        // 辅助方法，避免直接设置属性导致的递归调用
        private void SetTotalItems(int value)
        {
            if (TotalItems != value)
                SetValue(TotalItemsProperty, value);
        }

        private void SetTotalPages(int value)
        {
            if (TotalPages != value)
                SetValue(TotalPagesProperty, value);
        }

        private void UpdatePageInfo()
        {
            PageInfo = $"第 {CurrentPage} 页 / 共 {TotalPages} 页";
        }

        private void UpdatePaginationControls()
        {
            if (_firstPageButton != null)
                _firstPageButton.IsEnabled = CurrentPage > 1;
            if (_previousPageButton != null)
                _previousPageButton.IsEnabled = CurrentPage > 1;
            if (_nextPageButton != null)
                _nextPageButton.IsEnabled = CurrentPage < TotalPages;
            if (_lastPageButton != null)
                _lastPageButton.IsEnabled = CurrentPage < TotalPages;
        }

        // 高性能数据刷新
        private void RefreshData()
        {
            try
            {
                CalculateTotalPages();

                // 保存当前选择项
                var currentSelectedItem = SelectedItem;
                var currentSelectedItems = SelectedItems?.Cast<object>().ToList() ?? new List<object>();

                // 只获取当前页的数据
                var newPageData = GetCurrentPageData();

                // 只有当数据真正变化时才更新
                if (!AreCollectionsEqual(_currentPageData, newPageData))
                {
                    _currentPageData = newPageData;
                    if (_dataGrid != null)
                        _dataGrid.ItemsSource = _currentPageData;

                    // 延迟恢复选择状态
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (currentSelectedItem != null && _dataGrid != null)
                        {
                            // 检查当前选择项是否仍然存在
                            var itemExists = _dataGrid.Items.Cast<object>()
                                .Any(item => item == currentSelectedItem || item.Equals(currentSelectedItem));

                            if (itemExists)
                            {
                                // 恢复选择
                                _isInternalSelectionChange = true;
                                try
                                {
                                    _dataGrid.SelectedItem = currentSelectedItem;
                                    SetCurrentValue(SelectedItemProperty, currentSelectedItem);
                                }
                                finally
                                {
                                    _isInternalSelectionChange = false;
                                }
                            }
                            else
                            {
                                // 选择项不存在，清空选择
                                SetCurrentValue(SelectedItemProperty, null);
                            }
                        }
                    }), System.Windows.Threading.DispatcherPriority.DataBind);
                }

                UpdatePaginationControls();
                UpdatePageInfo();
            }
            catch (Exception ex)
            {
                // 记录错误但不崩溃
                System.Diagnostics.Debug.WriteLine($"RefreshData error: {ex.Message}");
            }
        }

        // 简单比较两个集合是否相等
        private bool AreCollectionsEqual(IEnumerable collection1, IEnumerable collection2)
        {
            if (collection1 == null && collection2 == null) return true;
            if (collection1 == null || collection2 == null) return false;

            var list1 = collection1.Cast<object>().ToList();
            var list2 = collection2.Cast<object>().ToList();

            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!Equals(list1[i], list2[i]))
                    return false;
            }

            return true;
        }

        #endregion

        #region 属性变更回调

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= control.OnCollectionChanged;
            }

            // 立即计算总页数
            control.CalculateTotalPages();
            control.InitializePagination();
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;

            if (e.OldValue is ObservableCollection<DataGridColumn> oldColumns)
            {
                oldColumns.CollectionChanged -= control.OnColumnsCollectionChanged;
            }

            if (e.NewValue is ObservableCollection<DataGridColumn> newColumns)
            {
                newColumns.CollectionChanged += control.OnColumnsCollectionChanged;
            }

            control.UpdateDataGridColumns();
        }

        private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateDataGridColumns();
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;
            control.ScheduleRefresh();
        }

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FdDataGrid)d;
            control.CalculateTotalPages();
            control.ScheduleRefresh();
        }

        // 延迟刷新调度
        private void ScheduleRefresh()
        {
            // 在调度刷新前先计算总页数
            CalculateTotalPages();

            _needsRefresh = true;
            _refreshTimer.Interval = TimeSpan.FromMilliseconds(RefreshDelay);
            _refreshTimer.Stop();
            _refreshTimer.Start();
        }

        #endregion

        #region 公共方法

        public void RefreshPagination()
        {
            CalculateTotalPages();
            RefreshData();
        }

        public void GoToFirstPage()
        {
            CurrentPage = 1;
        }

        public void GoToLastPage()
        {
            CurrentPage = TotalPages;
        }

        public void GoToPage(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= TotalPages)
                CurrentPage = pageNumber;
        }

        // 立即刷新（慎用）
        public void ForceRefresh()
        {
            _refreshTimer.Stop();
            CalculateTotalPages();  // 确保计算总页数
            RefreshData();
        }

        #endregion
    }
}
