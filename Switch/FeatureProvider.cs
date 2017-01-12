using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Switch.Configuration;
using Switch.Read;

namespace Switch
{
    public class FeatureProvider : IFeatureProvider
    {
        public FeatureProvider()
        {
        }

        public Task<TOption> LoadAsync<TOption>() where TOption : IOption, new()
        {
            var name = typeof(TOption).Name;
            var option = LoadOption<TOption>(name);
            return Task.FromResult(option);
        }

        private TOption LoadOption<TOption>(string key) where TOption : IOption, new()
        {

            var reader = new ConfigurationParameterReader(key);
            var option = CustomActivator.CreateInstance<TOption>();
            LoadSettings(option, reader);
            return option;
        }

        private void LoadSettings<TOption>(TOption option, ConfigurationParameterReader reader) where TOption : IOption
        {
            var propertyInfos = option.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                LoadProperty(propertyInfo, option, reader);
            }
        }

        private void LoadProperty(PropertyInfo propertyInfo, IOption option, ConfigurationParameterReader reader)
        {
            var isNotASetting = propertyInfo.GetCustomAttribute<NotASettingAttribute>();
            if (isNotASetting != null)
            {
                return;
            }


            var propertyType = propertyInfo.PropertyType;
            var value = reader.Key(propertyInfo.Name);

            if (typeof(IOption).IsAssignableFrom(propertyType))
            {
                LoadComplexType(propertyInfo, option, reader);
            }
            if (propertyType == typeof(string))
            {
                LoadString(propertyInfo, value, option);
            }
            if (propertyType == typeof(bool))
            {
                LoadBool(propertyInfo, value, option);
            }
//            else
//            {
//                throw new ConfigurationLoadingException($"Failed to load {option.GetType().FullName}.{Environment.NewLine}Failed to find expected config entry {reader.BaseKey}:{settingKey}");
//            }
        }

        private void LoadComplexType(PropertyInfo propertyInfo, IOption option, ConfigurationParameterReader reader)
        {
            var methodInfos = typeof(FeatureProvider).GetMethods(BindingFlags.NonPublic| BindingFlags.Instance);
            var loadOptionMethodInfo = typeof(FeatureProvider).GetMethod(nameof(LoadOption), BindingFlags.NonPublic| BindingFlags.Instance);
            var generic = loadOptionMethodInfo.MakeGenericMethod(propertyInfo.PropertyType);

            propertyInfo.SetValue(option, generic.Invoke(this, new object[] {$"{reader.BaseKey}:{propertyInfo.Name}"}),
                null);
        }

        private static void LoadGuid(PropertyInfo propertyInfo, string value, IOption option)
            => propertyInfo.SetValue(option, Guid.Parse(value), null);

        private static void LoadInt(PropertyInfo propertyInfo, string value, IOption option)
            => propertyInfo.SetValue(option, int.Parse(value), null);

        private static void LoadBool(PropertyInfo propertyInfo, string value, IOption option)
            => propertyInfo.SetValue(option, bool.Parse(value), null);

        private static void LoadString(PropertyInfo propertyInfo, string value, IOption option)
            => propertyInfo.SetValue(option, value, null);
    }
}