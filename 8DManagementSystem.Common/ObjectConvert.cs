using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8DManagementSystem.Common
{
    /// <summary>
    /// object转换为基础数据类型 扩展方法
    /// </summary>
    public static class ObjectConvert
    {
        #region ToString
        public static string EToString(this object obj)
        {
            string result = string.Empty;

            if (obj != null)
            {
                result = obj.ToString();
            }

            return result;
        }
        #endregion

        #region ToInt
        public static int EToInt(this object obj)
        {
            int result = default(int);

            if (obj != null && obj != DBNull.Value)
            {
                int.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static int EToInt(this object obj, int defaultValue)
        {
            int result = defaultValue;

            if (obj != null && obj != DBNull.Value)
            {
                int.TryParse(obj.ToString(), out result);
            }

            return result;
        }
        #endregion

        #region ToDemical
        public static decimal EToDemical(this object obj)
        {
            decimal result = default(decimal);

            if (obj != null && obj != DBNull.Value)
            {
                decimal.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static decimal EToDemical(this object obj, decimal defaultValue)
        {
            decimal result = defaultValue;

            if (obj != null && obj != DBNull.Value)
            {
                decimal.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        #endregion

        #region ToDateTime
        public static DateTime EToDateTime(this object obj)
        {
            DateTime result = default(DateTime);

            if (obj != null && obj != DBNull.Value)
            {
                DateTime.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static DateTime EToDateTime(this object obj, DateTime defualtValue)
        {
            DateTime result = defualtValue;

            if (obj != null && obj != DBNull.Value)
            {
                DateTime.TryParse(obj.ToString(), out result);
            }

            return result;
        }
        #endregion

        #region ToDouble
        public static double EToDouble(this object obj)
        {
            double result = default(double);

            if (obj != null && obj != DBNull.Value)
            {
                double.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static double EToDouble(this object obj, double defaultValue)
        {
            double result = defaultValue;

            if (obj != null && obj != DBNull.Value)
            {
                double.TryParse(obj.ToString(), out result);
            }

            return result;
        }
        #endregion

        #region ToGuid
        public static Guid ToGuid(this object obj)
        {
            Guid result = Guid.Empty;

            if (obj != null && obj != DBNull.Value)
            {
                Guid.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static Guid ToGuid(this object obj, Guid defaultValue)
        {
            Guid result = defaultValue;

            if (obj != null && obj != DBNull.Value)
            {
                Guid.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static Guid? ToGuidNull(this object obj)
        {
            if (obj.ToString() == "") return null;
            Guid result = new Guid();
            if (obj != DBNull.Value)
            {
                Guid.TryParse(obj.ToString(), out result);
            }
            return result;
        }

        #endregion

        #region ToFloat
        public static float EToFloat(this object obj)
        {
            float result = default(float);

            if (obj != null && obj != DBNull.Value)
            {
                float.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static float EToFloat(this object obj, float defaultValue)
        {
            float result = defaultValue;

            if (obj != null && obj != DBNull.Value)
            {
                float.TryParse(obj.ToString(), out result);
            }

            return result;
        }
        #endregion

        #region 时间对象处理
        /// <summary>
        /// 获取当前月份的第一天日期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime EGetFirstDayOfMonth(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, 1, time.Hour, time.Minute, time.Second);
        }

        /// <summary>
        /// 获取当前月份的最后一天日期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime EGetLastDayOfMonth(this DateTime time)
        {
            return time.EGetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }
        #endregion
    }
}
