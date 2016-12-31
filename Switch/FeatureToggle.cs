using System;

namespace Switch
{
    public interface IOption
    {

    }

    public interface IFeature : IOption
    {
        bool IsEnabled { get; set; }
    }

    public abstract class Feature : IFeature
    {
        public bool IsEnabled { get; set; }
    }

    public class OptionalAttribute : Attribute
    {

    }
}