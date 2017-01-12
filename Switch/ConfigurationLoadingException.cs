using System;

namespace Switch
{
    public class ConfigurationLoadingException : Exception
    {
        public ConfigurationLoadingException(string message) : base(message)
        {
        }
    }
}