using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Switch.Read;

namespace Switch
{

    interface IFeatureProvider
    {
        Task<TFeature> LoadAsync<TFeature>() where TFeature : IOption, new();
    }

    public class FeatureProvider : IFeatureProvider
    {
        public Task<TOption> LoadAsync<TOption>() where TOption : IOption, new()
        {
            var name = typeof(TOption).Name;
            var isEnabled = ConfigurationManager.AppSettings[$"{name}:IsEnabled"];
            var option = CustomActivator.CreateInstance<TOption>();
            if (option is IFeature)
            {
                var feature = option as IFeature;
                feature.IsEnabled = bool.Parse(isEnabled);
            }
            LoadSettings(option, name);
            return Task.FromResult(option);
        }

        private void LoadSettings<TOption>(TOption option, string baseKey) where TOption : IOption
        {
            var propertyInfos = option.GetType().GetProperties(BindingFlags.Public);
            foreach (var propertyInfo in propertyInfos)
            {
                LoadProperty(propertyInfo, option,baseKey);
            }
        }

        private void LoadProperty(PropertyInfo propertyInfo, IOption option, string baseKey)
        {
            var settingKey = $"{baseKey}:{propertyInfo.Name}";
            var value = ConfigurationManager.AppSettings[settingKey];
            if (!string.IsNullOrWhiteSpace(value))
            {
                var propertyType = propertyInfo.PropertyType;
                if (propertyType == typeof(string))
                {
                    LoadString(propertyInfo, value, option);
                }
            }
        }

        private void LoadString(PropertyInfo propertyInfo, string value, IOption option)
        {
            throw new System.NotImplementedException();
        }
    }
}