using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataResolver
{
    public class EncodeingHelper
    {
        /// <summary>
        /// 8421
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double CompressBCD(byte[] bytes)
        {
            double s = USymbolCompressBCD(bytes);
            double l = 8 * Math.Pow(10, bytes.Length * 2 - 1);
            if (s >= l) s -= l;
            return s;
        }
        /// <summary>
        /// 默认8421  以后使用enum 作为参数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double USymbolCompressBCD(byte[] bytes)
        {
            int l = bytes.Length, i = 0;
            double s = 0;
            while (--l > -1) s += (bytes[l] / 16 * 10 + bytes[l] % 16) * Math.Pow(10, i++ * 2);
            return s;
        }

        public static byte[] CutOff(byte[] bytes, int start, int count)
        {
            var s = new byte[count];
            while (--count > -1) s[count] = bytes[start + count];
            return s;
        }

        /// <summary>
        /// 异或校验码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte BCCValidateCode(byte[] source, int offset, int lenght)
        {
            byte s = 0; int i = offset;
            while (lenght-- > 0) s ^= source[i++];
            return s;
        }
    }

}
