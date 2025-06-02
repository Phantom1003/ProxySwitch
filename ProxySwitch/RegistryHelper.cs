using Microsoft.Win32;
using System;

namespace ProxySwitch
{
    public static class RegistryHelper
    {
        public static T Get<T>(RegistryKey hive, string subKeyPath, string valueName, T defaultValue = default)
        {
            using (var subKey = hive.OpenSubKey(subKeyPath, false))
            {
                if (subKey == null) return defaultValue;
                object value = subKey.GetValue(valueName);
                if (value is T typedValue)
                    return typedValue;

                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        public static void Set<T>(RegistryKey hive, string subKeyPath, string valueName, T value, RegistryValueKind valueKind = RegistryValueKind.String)
        {
            using (var subKey = hive.CreateSubKey(subKeyPath))
            {
                subKey.SetValue(valueName, value, valueKind);
            }
        }
    }
}
