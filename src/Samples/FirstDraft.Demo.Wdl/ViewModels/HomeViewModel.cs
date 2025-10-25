using Microsoft.Win32;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Path = System.IO.Path;

namespace FirstDraft.Demo.Wdl.ViewModels
{


    public class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {

        }

        public void InitRandomContests()
        {
            if (Contests.Count != 0) return;
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                Contests.Add(new Contest
                {
                    DD = rand.Next(1, 30),
                    DL = rand.Next(1, 30),
                    DW = rand.Next(1, 30),
                    WW = rand.Next(1, 30),
                    WD = rand.Next(1, 30),
                    WL = rand.Next(1, 30),
                    LW = rand.Next(1, 30),
                    LD = rand.Next(1, 30),
                    LL = rand.Next(1, 30),
                });
            }
        }

        public ObservableCollection<Contest> Contests { get; set; } = new ObservableCollection<Contest>();

        public RelayCommand GenCommand => new RelayCommand(() =>
        {
            Generate();
            Filter();
        });

        public RelayCommand DelCommand => new RelayCommand(() =>
        {
            if (Contests.Count > 0)
            {
                Contests.RemoveAt(Contests.Count - 1);
            }
        });
        public RelayCommand AddCommand => new RelayCommand(() =>
        {
            Contests.Add(new Contest());
        });

        private List<string> lines = new List<string>();

        public void Generate()
        {
            var query = AlgCore.EnumAll();

            lines.Clear();
            foreach (var idx in query)
            {
                (string line, double prod) = Contests.ToList().BuildLine(idx);

                lines.Add($"{line} {prod.ToString("F2", CultureInfo.InvariantCulture)}");
            }
        }

        #region 加载和保存


        public void LoadFirst(string filePath)
        {
            Load(filePath);
            InitRandomContests();
        }

        private void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                var rows = MiniExcel.Query<Contest>(filePath);

                if (rows == null) return;

                if (rows.Count() == 0) return;

                Contests.Clear();

                var contests = rows.ToList();

                for (int i = 0; i < contests.Count; i++)
                {
                    Contests.Add(contests[i]);
                }
            }
            catch
            {
                return;
            }
        }
        public void Save(string filePath)
        {
            // 确保目录存在
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            MiniExcel.SaveAs(filePath, Contests, overwriteFile: true, sheetName: "WDL");
        }

        public RelayCommand LoadCommand => new RelayCommand(() =>
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel 文件|*.xlsx",
                Title = "util.xlsx"
            };
            if (dlg.ShowDialog() != true) return;

            Load(dlg.FileName);
        });

        public RelayCommand SaveCommand => new RelayCommand(() =>
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Excel文件|*.xlsx|所有文件|*.*",
                DefaultExt = "xlsx",
                FileName = "未命名",
                RestoreDirectory = true
            };

            // 2. 弹窗
            if (dlg.ShowDialog() == true)
            {
                Save(dlg.FileName);
            }
        });



        #endregion

        public ObservableCollection<ContestItem> Results { get; set; } = new ObservableCollection<ContestItem>();

        #region 导出 导入

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { Set(ref fileName, value); }
        }

        public RelayCommand ExportCommand => new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(fileName))
            {
                ShowErr("请输入名称");
                return;
            }

            var dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllLines(System.IO.Path.Combine(dir, DateTime.Now.ToString("yyyyMMddHHmmss_") + FileName + $"_{Results.Count}_" + ".txt"), Results.ToList().Select(t => $"{t.Key} {t.Value}"));

            File.WriteAllLines(System.IO.Path.Combine(dir, DateTime.Now.ToString("yyyyMMddHHmmss_") + FileName + $"_{Results.Count}_" + "express.txt"), Results.ToList().Select(t => $"{t.Key}"));
            FileName = "";
            Process.Start("explorer.exe", dir);
        });


        private void ShowErr(string message)
        {
            MessageBox.Show(message, "MDL", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public RelayCommand ImportCommand => new RelayCommand(() =>
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "txt 文件|*.txt",
                Title = "选择 数据文件(不是express).txt"
            };
            if (dlg.ShowDialog() != true) return;

            var fileLines = File.ReadAllLines(dlg.FileName);

            if (fileLines.All(t => t.Split(' ').Length == 2))
            {
                this.lines.Clear();
                foreach (var item in fileLines)
                {
                    lines.Add(item.Trim());
                }
                Filter();
            }
            else
            {
                ShowErr("文件格式不对，注意是否选择的是正确的数据文件（非express标识）");
            }
        });


        #endregion

        #region 过滤

        private double prodMin = 300;
        public double ProdMin
        {
            get { return prodMin; }
            set { SetProperty(ref prodMin, value); }
        }


        private double prodMax = 500;
        public double ProdMax
        {
            get { return prodMax; }
            set { SetProperty(ref prodMax, value); }
        }

        public RelayCommand FilterCommand => new RelayCommand(() =>
        {
            Filter();
        });

        private void Filter()
        {
            Results.Clear();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                if (parts.Length != 2) continue;

                if (double.TryParse(parts[1],
                                    NumberStyles.Float,
                                    CultureInfo.InvariantCulture,
                                    out var prod)
                    && prod >= ProdMin && prod <= ProdMax)
                {
                    Results.Add(new ContestItem() { Key = parts[0], Value = prod });
                }
            }

            OnPropertyChanged(nameof(CanExport));
        }

        public bool CanExport => Results.Count > 0;



        #endregion



    }

    public class ContestItem
    {
        public string Key { get; set; }
        public Double Value { get; set; }
    }




}
