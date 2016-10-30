using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flexlive.CQP.Framework.Utils
{
    /// <summary>
    /// 文本操作类。
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 通过正则表达式获取源字符串中所有匹配的起始和结束字符串之间的内容。
        /// </summary>
        /// <param name="sourceString">源字符串。</param>
        /// <param name="startString">起始字符串。</param>
        /// <param name="endString">结束字符串。</param>
        /// <returns>所有匹配的字符串数组，无匹配时返回Null。</returns>
        public static string[] GetMidStrings(this string sourceString, string startString, string endString)
        {
            //初始化正则表达示。
            Regex rg = new Regex("(?<=(" + startString + "))[.\\s\\S]*?(?=(" + endString + "))", RegexOptions.Multiline | RegexOptions.Singleline);

            //获取匹配结果。
            MatchCollection mc = rg.Matches(sourceString);

            if (mc.Count > 0)
            {
                string[] midStrings = new string[mc.Count];

                for (int i = 0; i < mc.Count; i++)
                {
                    midStrings[i] = mc[i].Value;
                }

                return midStrings;
            }
            else
            {
                //无匹配结果时返回Null。
                return null;
            }
        }
    }
}
