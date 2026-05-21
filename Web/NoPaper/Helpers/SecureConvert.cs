using System;

namespace NoPaper.Helpers
{
  internal class SecureConvert
  {
    public static int? ToNullableInt(string value)
    {
      if (string.IsNullOrEmpty(value))
        return null;
      else
        return Convert.ToInt32(value);
    }

  }
}