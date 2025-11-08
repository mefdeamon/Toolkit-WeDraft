using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;

namespace FirstDraft.Demo.Wdl
{

    public class BaseContest : ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private double ww = 1;
        [Description("10")]
        public double WW
        {
            get { return ww; }
            set { SetProperty(ref ww, value); }
        }

        private double dd = 1;
        [Description("31")]
        public double DD
        {
            get { return dd; }
            set { SetProperty(ref dd, value); }
        }

        private double ll = 1;
        [Description("22")]
        public double LL
        {
            get { return ll; }
            set { SetProperty(ref ll, value); }
        }
    }

    public class WDLContest : ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private double ww = 1;
        [Description("10")]
        public double WW
        {
            get { return ww; }
            set { SetProperty(ref ww, value); }
        }

        private double wd = 1;
        [Description("20")]
        public double WD
        {
            get { return wd; }
            set { SetProperty(ref wd, value); }
        }

        private double wl = 1;
        [Description("30")]
        public double WL
        {
            get { return wl; }
            set { SetProperty(ref wl, value); }
        }

        private double dw = 1;
        [Description("21")]
        public double DW
        {
            get { return dw; }
            set { SetProperty(ref dw, value); }
        }

        private double dd = 1;
        [Description("31")]
        public double DD
        {
            get { return dd; }
            set { SetProperty(ref dd, value); }
        }

        private double dl = 1;
        [Description("32")]
        public double DL
        {
            get { return dl; }
            set { SetProperty(ref dl, value); }
        }

        private double lw = 1;
        [Description("00")]
        public double LW
        {
            get { return lw; }
            set { SetProperty(ref lw, value); }
        }

        private double ld = 1;
        [Description("11")]
        public double LD
        {
            get { return ld; }
            set { SetProperty(ref ld, value); }
        }

        private double ll = 1;
        [Description("22")]
        public double LL
        {
            get { return ll; }
            set { SetProperty(ref ll, value); }
        }
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
        [Description("10")]
        public double WW
        {
            get { return ww; }
            set { SetProperty(ref ww, value); }
        }

        private double wd = 1;
        [Description("20")]
        public double WD
        {
            get { return wd; }
            set { SetProperty(ref wd, value); }
        }

        private double wl = 1;
        [Description("30")]
        public double WL
        {
            get { return wl; }
            set { SetProperty(ref wl, value); }
        }

        private double dw = 1;
        [Description("21")]
        public double DW
        {
            get { return dw; }
            set { SetProperty(ref dw, value); }
        }

        private double dd = 1;
        [Description("31")]
        public double DD
        {
            get { return dd; }
            set { SetProperty(ref dd, value); }
        }

        private double dl = 1;
        [Description("32")]
        public double DL
        {
            get { return dl; }
            set { SetProperty(ref dl, value); }
        }

        private double lw = 1;
        [Description("00")]
        public double LW
        {
            get { return lw; }
            set { SetProperty(ref lw, value); }
        }

        private double ld = 1;
        [Description("11")]
        public double LD
        {
            get { return ld; }
            set { SetProperty(ref ld, value); }
        }

        private double ll = 1;
        [Description("22")]
        public double LL
        {
            get { return ll; }
            set { SetProperty(ref ll, value); }
        }

        private double r33 = 1;
        [Description("33")]
        public double R33
        {
            get { return r33; }
            set { Set(ref r33, value); }
        }

        private double r01 = 1;
        [Description("01")]
        public double R01
        {
            get { return r01; }
            set { Set(ref r01, value); }
        }

        private double r02 = 1;
        [Description("02")]
        public double R02
        {
            get { return r02; }
            set { Set(ref r02, value); }
        }

        private double r03 = 1;
        [Description("03")]
        public double R03
        {
            get { return r03; }
            set { Set(ref r03, value); }
        }

        private double r12 = 1;
        [Description("12")]
        public double R12
        {
            get { return r12; }
            set { Set(ref r12, value); }
        }

        private double r13 = 1;
        [Description("13")]
        public double R13
        {
            get { return r13; }
            set { Set(ref r13, value); }
        }

        private double r23 = 1;
        [Description("23")]
        public double R23
        {
            get { return r23; }
            set { Set(ref r23, value); }
        }
    }

    public static class ContestHelper
    {

        public static string Label(int c)
        {
            switch (c)
            {
                case 0:
                    return "3-3";
                case 1:
                    return "3-1";
                case 2:
                    return "3-0";
                case 3:
                    return "1-3";
                case 4:
                    return "1-1";
                case 5:
                    return "1-0";
                case 6:
                    return "0-3";
                case 7:
                    return "0-1";
                case 8:
                    return "0-0";
                default:
                    return "x-x";
            }
        }

        /* ---------- 把 idx[] → 3-3-2-1... + 乘积 ---------- */
        public static (string line, double prod) BuildLine(this List<Contest> Contests, int[] idx)
        {
            int columns = Contests.Count;
            var labels = new string[columns];
            double prod = 1.0;

            for (int r = 0; r < columns; r++)
            {
                int col = idx[r];               // 当前行选的列 0-8
                labels[r] = Label(col);

                switch (col)               // 取对应 double 属性
                {
                    case 0:
                        prod *= Contests[r].WW;
                        break;
                    case 1:
                        prod *= Contests[r].WD;
                        break;
                    case 2:
                        prod *= Contests[r].WL;
                        break;
                    case 3:
                        prod *= Contests[r].DW;
                        break;
                    case 4:
                        prod *= Contests[r].DD;
                        break;
                    case 5:
                        prod *= Contests[r].DL;
                        break;
                    case 6:
                        prod *= Contests[r].LW;
                        break;
                    case 7:
                        prod *= Contests[r].LD;
                        break;
                    case 8:
                        prod *= Contests[r].LL;
                        break;
                    default:
                        prod *= 0.0;
                        break;
                }
            }
            return (string.Join("-", labels), prod);
        }


        public static string Label3(int c)
        {
            switch (c)
            {
                case 0:
                    return "3";
                case 1:
                    return "1";
                case 2:
                    return "0";
                case 3:
                default:
                    return "x-x";
            }
        }
        public static (string line, double prod) BuildLine3(this List<Contest> Contests, int[] idx)
        {
            int columns = Contests.Count;
            var labels = new string[columns];
            double prod = 1.0;

            for (int r = 0; r < columns; r++)
            {
                int col = idx[r];               // 当前行选的列 0-8
                labels[r] = Label3(col);

                switch (col)               // 取对应 double 属性
                {
                    case 0:
                        prod *= Contests[r].WW;
                        break;
                    case 1:
                        prod *= Contests[r].DD;
                        break;
                    case 2:
                        prod *= Contests[r].LL;
                        break;
                    default:
                        prod *= 0.0;
                        break;
                }
            }
            return (string.Join("-", labels), prod);
        }



        public static (string line, double prod) BuildLine16(this List<Contest> Contests, int[] idx)
        {
            int columns = Contests.Count;
            var labels = new string[columns];
            double prod = 1.0;

            for (int r = 0; r < columns; r++)
            {
                int col = idx[r];               // 当前行选的列 0-8
                labels[r] = Label16(col);

                switch (col)               // 取对应 double 属性
                {
                    case 0:
                        prod *= Contests[r].WW;
                        break;
                    case 1:
                        prod *= Contests[r].WD;
                        break;
                    case 2:
                        prod *= Contests[r].WL;
                        break;
                    case 3:
                        prod *= Contests[r].DW;
                        break;
                    case 4:
                        prod *= Contests[r].DD;
                        break;
                    case 5:
                        prod *= Contests[r].DL;
                        break;
                    case 6:
                        prod *= Contests[r].LW;
                        break;
                    case 7:
                        prod *= Contests[r].LD;
                        break;
                    case 8:
                        prod *= Contests[r].LL;
                        break;
                    case 9:
                        prod *= Contests[r].R33;
                        break;
                    case 10:
                        prod *= Contests[r].R01;
                        break;
                    case 11:
                        prod *= Contests[r].R02;
                        break;
                    case 12:
                        prod *= Contests[r].R03;
                        break;
                    case 13:
                        prod *= Contests[r].R12;
                        break;
                    case 14:
                        prod *= Contests[r].R13;
                        break;
                    case 15:
                        prod *= Contests[r].R23;
                        break;
                    default:
                        prod *= 0.0;
                        break;
                }
            }
            return (string.Join("", labels), prod);
        }

        public static string Label16(int c)
        {
            string llcolumns = "10-20-30-21-31-32-00-11-22-33-01-02-03-12-13-23";
            var items = llcolumns.Split('-');
            if (c < items.Length)
            {
                return items[c];
            }
            return "x";

        }


        /*
         1、9场比赛，3个结果分别为3、1、0
2、14场比赛，3个结果分别为3、1、0
3、4场比赛，16个结果分别为：10、20、30、21、31、32、00、11、22、33、01、02、03、12、13、23
         
         */

    }


    public static class WdlcoreHelper
    {
        private const string FileName = "Wdlcore.dll";
        private const int RandomBlockSize = 16 * 1024;

        public static bool IsValid()
        {
            return true;
            if (!File.Exists(FileName))
            {
                return false;
                // 创建文件
                FileStream fs = null;
                BinaryWriter bw = null;
                try
                {
                    fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    bw = new BinaryWriter(fs);

                    // 第一段随机
                    bw.Write(GetRandomBytes(RandomBlockSize));

                    // OpenCount = 0
                    bw.Write(0U);

                    // 0xFFFFFFFF
                    bw.Write(0xFFFFFFFFU);

                    // 第二段随机
                    bw.Write(GetRandomBytes(RandomBlockSize));
                }
                finally
                {
                    if (bw != null) bw.Close();
                    if (fs != null) fs.Close();
                }

                return true;
            }
            else
            {
                FileStream fs = null;
                BinaryReader br = null;
                BinaryWriter bw = null;
                try
                {
                    fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
                    br = new BinaryReader(fs);
                    bw = new BinaryWriter(fs);

                    // 跳过第一段随机
                    fs.Seek(RandomBlockSize, SeekOrigin.Begin);

                    // 读 OpenCount
                    uint openCount = br.ReadUInt32();
                    if (openCount > 50)
                    {
                        return false;
                    }

                    // 写回 ++
                    openCount++;
                    fs.Seek(RandomBlockSize, SeekOrigin.Begin);
                    bw.Write(openCount);
                }
                finally
                {
                    if (br != null) br.Close();
                    if (bw != null) bw.Close();
                    if (fs != null) fs.Close();
                }
                return true;
            }
        }

        private static byte[] GetRandomBytes(int count)
        {
            byte[] buf = new byte[count];
            new Random().NextBytes(buf);
            return buf;
        }
    }
}