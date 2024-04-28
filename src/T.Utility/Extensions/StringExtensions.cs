using System;

namespace T.Utility.Extensions
{
    /// <summary>
    /// String extension methods for common scenarios.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Calculate the similarity of two strings by edit distance.
        /// https://zhuanlan.zhihu.com/p/80682302
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns></returns>
        public static double Similarity(this string str1, string str2, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(str1))
            {
                if (string.IsNullOrEmpty(str2))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (string.IsNullOrEmpty(str2))
            {
                return 0;
            }

            string s1, s2;
            if (ignoreCase)
            {
                s1 = str1.ToLower();
                s2 = str2.ToLower();
            }
            else
            {
                s1 = str1;
                s2 = str2;
            }

            //
            int m = s1.Length;
            int n = s2.Length;

            var dp = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
                dp[i, 0] = i;
            for (int j = 1; j <= n; j++)
                dp[0, j] = j;

            //
            for (int i = 1; i <= m; i++)
            {
                int si = s1[i - 1];
                for (int j = 1; j <= n; j++)
                {
                    if (si == s2[j - 1])
                        dp[i, j] = dp[i - 1, j - 1];
                    else
                        dp[i, j] = Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1])) + 1;
                }
            }

            //
            int ml = Math.Max(m, n);
            double similarity = ((double)(ml - dp[m, n])) / ml;

            return similarity;
        }
    }
}
