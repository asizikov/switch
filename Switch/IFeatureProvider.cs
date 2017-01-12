using System.Threading.Tasks;

namespace Switch
{
    internal interface IFeatureProvider
    {
        Task<TFeature> LoadAsync<TFeature>() where TFeature : IOption, new();
    }
}