using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
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
            GenerateAllCombinations(only_file, false);
        });
        public RelayCommand Gen2Command => new RelayCommand(() =>
        {
            GenerateAllCombinations(prod_file, true);
            LoadComboFile();
        });




        public void GenerateAllCombinations(string filePath, bool all)
        {
            var query = EnumAll();

            using var sw = new StreamWriter(filePath);
            foreach (var idx in query)
            {
                (string line, double prod) = BuildLine(idx);
                string outText = line;
                if (all)
                    outText = $"{line} {prod.ToString("F2", CultureInfo.InvariantCulture)}";
                sw?.WriteLine(outText);
            }
            sw?.Close();
        }

        /* ---------- 3. 迭代器版 9^6 遍历 ---------- */
        private IEnumerable<int[]> EnumAll()
        {
            const int Choices = 9;
            int[] cur = new int[6];
            do
            {
                yield return (int[])cur.Clone();

                int r = 5;
                while (r >= 0 && ++cur[r] == Choices)
                {
                    cur[r] = 0;
                    --r;
                }
                if (r < 0) break;
            } while (true);
        }

        /* ---------- 把 idx[] → 3-3-2-1... + 乘积 ---------- */
        private (string line, double prod) BuildLine(int[] idx)
        {
            // 列名与3/2/1映射
            static string Label(int c) => c switch
            {
                0 => "3-3",
                1 => "3-1",
                2 => "3-0",
                3 => "1-3",
                4 => "1-1",
                5 => "1-0",
                6 => "0-3",
                7 => "0-1",
                8 => "0-0",
                _ => "x-x"
            };

            var labels = new string[6];
            double prod = 1.0;

            for (int r = 0; r < 6; r++)
            {
                int col = idx[r];               // 当前行选的列 0-8
                labels[r] = Label(col);
                prod *= col switch               // 取对应 double 属性
                {
                    0 => Contests[r].WW,
                    1 => Contests[r].WD,
                    2 => Contests[r].WL,
                    3 => Contests[r].DW,
                    4 => Contests[r].DD,
                    5 => Contests[r].DL,
                    6 => Contests[r].LW,
                    7 => Contests[r].LD,
                    8 => Contests[r].LL,
                    _ => 0.0
                };
            }
            return (string.Join("-", labels), prod);
        }


        public RelayCommand LoadCommand => new RelayCommand(() =>
        {
            LoadComboFile();
        });


        public ObservableCollection<ContestItem> Results { get; set; } = new ObservableCollection<ContestItem>();

        private double prod = 300;
        public double Prod
        {
            get { return prod; }
            set { SetProperty(ref prod, value); }
        }


        readonly string prod_file = "prod_file.txt";
        readonly string only_file = "only_file.txt";

        /* 命令具体实现 */
        private void LoadComboFile()
        {
            //var dlg = new Microsoft.Win32.OpenFileDialog
            //{
            //    Filter = "TXT 文件|*.txt",
            //    Title = "选择 all_combo.txt"
            //};
            //if (dlg.ShowDialog() != true) return;

            Results.Clear();
            foreach (var line in File.ReadLines(prod_file))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) continue;

                if (double.TryParse(parts[1],
                                    NumberStyles.Float,
                                    CultureInfo.InvariantCulture,
                                    out var prod)
                    && prod > Prod)
                {
                    Results.Add(new ContestItem() { Key = parts[0], Value = prod });
                }
            }
        }
    }

    public class ContestItem
    {
        public string Key { get; set; }
        public Double Value { get; set; }
    }

    public class Contest : ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private double ww = 1;
        public double WW
        {
            get { return ww; }
            set { SetProperty(ref ww, value); }
        }

        private double wd = 1;
        public double WD
        {
            get { return wd; }
            set { SetProperty(ref wd, value); }
        }

        private double wl = 1;
        public double WL
        {
            get { return wl; }
            set { SetProperty(ref wl, value); }
        }

        private double dw = 1;
        public double DW
        {
            get { return dw; }
            set { SetProperty(ref dw, value); }
        }

        private double dd = 1;
        public double DD
        {
            get { return dd; }
            set { SetProperty(ref dd, value); }
        }

        private double dl = 1;
        public double DL
        {
            get { return dl; }
            set { SetProperty(ref dl, value); }
        }

        private double lw = 1;
        public double LW
        {
            get { return lw; }
            set { SetProperty(ref lw, value); }
        }

        private double ld = 1;
        public double LD
        {
            get { return ld; }
            set { SetProperty(ref ld, value); }
        }

        private double ll = 1;
        public double LL
        {
            get { return ll; }
            set { SetProperty(ref ll, value); }
        }


    }


    public class Rate : ObservableObject
    {
        public int MyProperty { get; set; }
    }


    public enum Result
    {
        胜, 平, 负, Win, Draw, Loss
    }



}
