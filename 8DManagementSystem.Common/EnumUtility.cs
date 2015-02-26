using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _8DManagementSystem.Common
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class EnumUtility
    {
        /// <summary>
        /// 获得枚举值的Description特性的值，一般是消息的搜索码
        /// </summary>
        /// <param name="value">要查找特性的枚举值</param>
        /// <returns>返回查找到的Description特性的值，如果没有，就返回.ToString()</returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
              (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 根据特定的枚举值名称获得枚举值的Description特性的值
        /// </summary>
        /// <param name="value">枚举类型</param>
        /// <param name="name">枚举值的名称</param>
        /// <returns>返回查找到的Description特性的值，如果没有，就返回.ToString()</returns>
        public static string GetEnumDescription(System.Type value, string name)
        {
            FieldInfo fi = value.GetField(name);
            DescriptionAttribute[] attributes =
              (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : name;
        }

        /// <summary>
        /// 基于Description特性或者枚举值名称获得具体的枚举值
        /// </summary>
        /// <param name="value">枚举类型</param>
        /// <param name="description">Description特性值或者元素的名称</param>
        /// <returns>返回枚举值。如果没找到，返回传入的Description值</returns>
        public static object GetEnumValue(System.Type value, string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return fi.GetValue(fi.Name);
                    }
                }
                if (fi.Name == description)
                {
                    return fi.GetValue(fi.Name);
                }
            }
            return description;
        }

        /// <summary>
        /// 将指定枚举类型转换成List，用来绑定ListControl
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<SelectListItem> ConvertEnumToListItems(Type enumType)
        {
            if (enumType.IsEnum == false) { return null; }
            List<SelectListItem> list = new List<SelectListItem>();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName) continue;
                strValue = field.GetRawConstantValue().ToString();
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    strText = (arr[0] as DescriptionAttribute).Description;
                }
                else
                {
                    strText = field.Name;
                }
                list.Add(new SelectListItem { Text = strText, Value = strValue });
            }
            return list;
        }

        /// <summary>
        /// 将指定枚举类型转换成MvcSelectList
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ConvertEnumToSelectListItems(Type enumType)
        {
            if (enumType.IsEnum == false) { return null; }
            List<SelectListItem> list = new List<SelectListItem>();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName) continue;
                strValue = field.GetRawConstantValue().ToString();
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    strText = (arr[0] as DescriptionAttribute).Description;
                }
                else
                {
                    strText = field.Name;
                }

                list.Add(new SelectListItem() { Text = strText, Value = strValue });
            }
            return list;
        }
    }
}
