using System;
using System.Collections.Generic;
using Sample.Console.FeatureToggles;
using Switch;

namespace Sample.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Loading");
            var featureProvider = new FeatureProvider();
            var greeting = featureProvider.LoadAsync<Greeting>().GetAwaiter().GetResult();

            if (greeting.IsEnabled)
            {
                System.Console.WriteLine("Hello");
            }
        }
    }
}