using System.Collections.Generic;

namespace FirstDraft.Demo.Wdl
{
    public class AlgCore
    {
        /* ---------- 3. 迭代器版 9^6 遍历 ---------- */
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int[]> EnumAll(int Choices = 9, int colums = 6)
        {
            int[] cur = new int[colums];
            do
            {
                yield return (int[])cur.Clone();

                int r = colums - 1;
                while (r >= 0 && ++cur[r] == Choices)
                {
                    cur[r] = 0;
                    --r;
                }
                if (r < 0) break;
            } while (true);
        }
    }


}
