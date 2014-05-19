namespace SolutionValidator.Helpers
{
    using Microsoft.Win32;

    public static class RegistryHelper
    {
        public static void DeleteUserValue(string key, string name)
        {
            RegistryKey registryKey = null;

            try
            {
                registryKey = Registry.CurrentUser.CreateSubKey(key);

                if (registryKey != null)
                {
                    registryKey.DeleteValue(name);
                }
            }
            catch {}
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
        }

        public static object ReadUserValue(string key, string name)
        {
            return Registry.CurrentUser.CreateSubKey(key).GetValue(name);
        }

        public static bool WriteUserValue(string key, string name, object value)
        {
            bool result;
            var registryKey = Registry.CurrentUser.CreateSubKey(key);
            
            if (registryKey == null)
            {
                return false;
            }
            try
            {
                registryKey.SetValue(name, value);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                registryKey.Close();
            }

            return result;
        }
    }
}
