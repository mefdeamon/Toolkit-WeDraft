using Microsoft.Win32;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Path = System.IO.Path;

namespace FirstDraft.Demo.Wdl.ViewModels
{


    public class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {

        }


        private bool isX16;
        public bool IsX16
        {
            get { return isX16; }
            set { Set(ref isX16, value); }
        }

        private bool isX3;
        public bool IsX3
        {
            get { return isX3; }
            set { Set(ref isX3, value); }
        }

        private bool isX9 = true;
        public bool IsX9
        {
            get { return isX9; }
            set { Set(ref isX9, value); }
        }

        private bool isWorking;
        public bool IsWorking
        {
            get { return isWorking; }
            set { Set(ref isWorking, value); }
        }

        private void Wrok(Action action)
        {
            if (IsWorking) { return; }
            IsWorking = true;
            Task.Run(() =>
            {
                action?.Invoke();
                IsWorking = false;
            });
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
            int count = Contests.Count;

            long a = 0;
            if (IsX3)
            {
                a = (long)Math.Pow(3, count);
            }
            else if (IsX9)
            {
                a = (long)Math.Pow(9, count);
            }
            else if (IsX16)
            {
                a = (long)Math.Pow(16, count);
            }

            if (MessageBox.Show("数据量为【" + a.ToChinese() + "】 条。过大的数据计算机无法支持，请慎重使用！", "是否继续？", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            Wrok(() =>
            {
                Generate();
                Filter();
            });
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


        public void Generate()
        {
            GenerateAndDump();
        }

        string dataFileName = "data.dat";
        public void ClearData()
        {
            if (File.Exists(dataFileName))
            {
                File.Delete(dataFileName);
            }
        }

        public void GenerateAndDump()
        {
            using (var fs = new FileStream(dataFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8, bufferSize: 65536))
                {
                    int pow = IsX3 ? 3 : IsX9 ? 9 : IsX16 ? 16 : 0;
                    var query = AlgCore.EnumAll(pow, Contests.Count);

                    foreach (var idx in query)
                    {
                        if (IsX9)
                        {
                            (string line, double prod) = Contests.ToList().BuildLine(idx);
                            sw.WriteLine($"{line} {prod:F2}");   // 纯内容 + 空格 + 2 位小数
                        }
                        else if (IsX3)
                        {
                            (string line, double prod) = Contests.ToList().BuildLine3(idx);
                            sw.WriteLine($"{line} {prod:F2}");   // 纯内容 + 空格 + 2 位小数
                        }
                        else if (IsX16)
                        {
                            (string line, double prod) = Contests.ToList().BuildLine16(idx);
                            sw.WriteLine($"{line} {prod:F2}");   // 纯内容 + 空格 + 2 位小数
                        }
                    }
                }
            }
        }
        public void GenerateAndDump(IEnumerable<string> lines)
        {
            ClearData();
            File.WriteAllLines(dataFileName, lines);
        }


        public IEnumerable<string> QueryRange(double minProd, double maxProd)
        {
            using (var fs = new FileStream(dataFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    Span<char> buf = stackalloc char[32];   // 足够放 F2 数字
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parts = line.Split(' ');
                        if (parts.Length != 2) continue;

                        if (double.TryParse(parts[1],
                                            NumberStyles.Float,
                                            CultureInfo.InvariantCulture,
                                            out var prod)
                            && prod >= minProd && prod <= maxProd)
                        {
                            yield return line;   // 整行原样给出
                        }
                    }
                }
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

            if (IsX9)
            {
                var data = Contests.Select(c => new WDLContest()
                {
                    Name = c.Name,
                    DD = c.DD,
                    WW = c.WW,
                    LL = c.LL,
                    WD = c.WD,
                    WL = c.WL,
                    LW = c.LW,
                    LD = c.LD,
                    DL = c.DL,
                    DW = c.DW,
                }).ToList();
                MiniExcel.SaveAs(filePath, data, overwriteFile: true, sheetName: "WDL^9");
            }
            else if (IsX3)
            {
                var data = Contests.Select(c => new BaseContest()
                {
                    Name = c.Name,
                    DD = c.DD,
                    WW = c.WW,
                    LL = c.LL,
                }).ToList();

                MiniExcel.SaveAs(filePath, data, overwriteFile: true, sheetName: "WDL^3");
            }
            else if (IsX16)
            {
                MiniExcel.SaveAs(filePath, Contests, overwriteFile: true, sheetName: "WDL^16");
            }
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

            Wrok(() =>
            {
                var dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllLines(System.IO.Path.Combine(dir, DateTime.Now.ToString("yyyyMMddHHmmss_") + FileName + $"_{Results.Count}_" + ".txt"), Results.ToList().Select(t => $"{t.Key} {t.Value}"));

                File.WriteAllLines(System.IO.Path.Combine(dir, DateTime.Now.ToString("yyyyMMddHHmmss_") + FileName + $"_{Results.Count}_" + "express.txt"), Results.ToList().Select(t => $"{t.Key}"));
                FileName = "";
            });

            // Process.Start("explorer.exe", dir);
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
                GenerateAndDump(fileLines);
                Filter();
            }
            else
            {
                ShowErr("文件格式不对，注意是否选择的是正确的数据文件（非express标识）");
            }
        });


        #endregion

        #region 过滤

        private double prodMin = 0;
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
            Wrok(Filter);
        });

        private void Filter()
        {
            Application.Current.Dispatcher.Invoke(() => Results.Clear());


            var lines = QueryRange(ProdMin, ProdMax);

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
                    Application.Current.Dispatcher.Invoke(() => Results.Add(new ContestItem() { Key = parts[0], Value = prod }));
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


    public static class NumberChinese
    {
        // 每 4 位一个中文单位
        private static readonly string[] Unit4 = { "", "万", "亿", "万亿" };

        /// <summary>
        /// 30000 -> "3万" ， 300000 -> "30万" ， 130000000 -> "1.3亿"
        /// </summary>
        public static string ToChinese(this long n)
        {
            if (n == 0) return "0";

            var sb = new StringBuilder();
            int unitPos = 0;          // 当前在哪一档（万、亿...）
            bool needZero = false;    // 是否需要补零

            while (n > 0)
            {
                long section = n % 10_000;   // 取低 4 位
                n /= 10_000;

                if (section != 0)
                {
                    string secStr = section.ToString();   // 先转数字
                    if (needZero && section < 1000)       // 中间有 0 要补
                        sb.Insert(0, '0');

                    sb.Insert(0, secStr + Unit4[unitPos]);
                    needZero = false;
                }
                else
                {
                    needZero = true;   // 这一段全 0，后面可能要补零
                }
                unitPos++;
            }

            return sb.ToString();
        }
    }



}
