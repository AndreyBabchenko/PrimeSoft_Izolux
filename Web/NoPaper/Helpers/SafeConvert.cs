using System;
using System.Data;

namespace Utils
{
  public static class SafeConvert
  {
    // Преобразование в long
    public static long ToLong(object value, long defaultValue = 0) =>
        value == null || value == DBNull.Value ? defaultValue : Convert.ToInt64(value);

    // Преобразование в int
    public static int ToInt(object value, int defaultValue = 0) =>
        value == null || value == DBNull.Value ? defaultValue : Convert.ToInt32(value);

    public static int ToInt(string value, int defaultValue = 0) =>
        int.TryParse(value, out var result) ? result : defaultValue;

    public static int? ToNullableInt(object value) =>
      value == null || value == DBNull.Value ? (int?)null : Convert.ToInt32(value); 

    // Преобразование в double
    public static double ToDouble(object value, double defaultValue = 0.0) =>
        value == null || value == DBNull.Value ? defaultValue : Convert.ToDouble(value);


    // Преобразование в Float
    public static float ToFloat(object value, float defaultValue = 0f) =>
        value == null || value == DBNull.Value ? defaultValue : Convert.ToSingle(value);

    // Преобразование в decimal
    public static decimal ToDecimal(object value, decimal defaultValue = 0m) =>
        value == null || value == DBNull.Value ? defaultValue : Convert.ToDecimal(value);

    // Преобразование в bool
    public static bool ToBool(object value, bool defaultValue = false)
    {
      if (value == null || value == DBNull.Value)
        return defaultValue;

      if ( value is bool   b ) return b;
      if ( value is int    i ) return i != 0;
      if ( value is string s ) return bool.TryParse(s, out bool result) && result;

      return Convert.ToBoolean(value);
    }

    // Преобразование в DateTime
    public static DateTime ToDateTime(object value, DateTime defaultValue)
    {
      if (value == null || value == DBNull.Value)
        return defaultValue;

      return DateTime.TryParse(value.ToString(), out DateTime result) ? result : defaultValue;
    }

    // Преобразование в строку (гарантированно не null)
    public static string ToString(object value, string defaultValue = "") =>
        value == null || value == DBNull.Value ? defaultValue : value.ToString();

    public static object SafeEval(object dataItem, string fieldName)
    {
      if (dataItem is DataRowView rowView)
        return rowView.Row.Table.Columns.Contains(fieldName)
            ? rowView[fieldName]
            : null;

      return null;
    }

  }
}