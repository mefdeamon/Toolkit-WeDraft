using System.Collections.Generic;

namespace FirstDraft.Demo.Wdl
{

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

    public static class ContestHelper
    {

       public  static string Label(int c)
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
        public static (string line, double prod) BuildLine(this List<Contest> Contests,int[] idx)
        {
            var labels = new string[6];
            double prod = 1.0;

            for (int r = 0; r < 6; r++)
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

    }


}
