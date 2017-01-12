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
                System.Console.WriteLine(greeting.GreetingText);
                System.Console.WriteLine(greeting.Boolean);
                System.Console.WriteLine("Not a setting:" + greeting.ShoulNotBeLoaded);
                System.Console.WriteLine("Nested: " + greeting.Nested.Name);

            }
        }
    }
}