using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Configuration.KeyValue;
using Configuration.Model;
using Configuration.Structured;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Primitives;

namespace Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            Test11();
        }

        static void Test1()
        {
            var source = new Dictionary<string, string>
            {
                ["LongDatePattern"] = "dddd,MMMM d, yyyy",
                ["LongTimePattern"] = "h:mm:ss tt",
                ["ShortDatePattern"] = "M/d/yyyy",
                ["ShortTimePattern"] = "h:mm tt",
            };

            var config = new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource() { InitialData = source })
                .Build();

            var options = new DateTimeFormatOptions(config);

            Console.WriteLine(options.LongDatePattern);
            Console.WriteLine(options.LongTimePattern);
            Console.WriteLine(options.ShortTimePattern);
            Console.WriteLine(options.ShortDatePattern);
        }

        static void Test2()
        {
            var source = new Dictionary<string, string>
            {
                ["format:dateTime:longDatePattern"] = "dddd,MMMM d, yyyy",
                ["format:dateTime:longTimePattern"] = "h:mm:ss tt",
                ["format:dateTime:shortDatePattern"] = "M/d/yyyy",
                ["format:dateTime:shortTimePattern"] = "h:mm tt",
                ["format:currencyDecimal:digits"] = "2",
                ["format:currencyDecimal:symbol"] = "$",
            };

            var config = new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource() { InitialData = source })
                .Build();

            var options = new FormatOptions(config.GetSection("Format"));

            Console.WriteLine("DateTime");
            Console.WriteLine(options.DateTime.LongDatePattern);
            Console.WriteLine(options.DateTime.LongTimePattern);
            Console.WriteLine(options.DateTime.ShortTimePattern);
            Console.WriteLine(options.DateTime.ShortDatePattern);
            Console.WriteLine("CurrencyDecimal");
            Console.WriteLine(options.CurrencyDecimal.Digits);
            Console.WriteLine(options.CurrencyDecimal.Symbol);

        }

        static void Test3()
        {
            var source = new Dictionary<string, string>
            {
                ["format:dateTime:longDatePattern"] = "dddd,MMMM d, yyyy",
                ["format:dateTime:longTimePattern"] = "h:mm:ss tt",
                ["format:dateTime:shortDatePattern"] = "M/d/yyyy",
                ["format:dateTime:shortTimePattern"] = "h:mm tt",
                ["format:currencyDecimal:digits"] = "2",
                ["format:currencyDecimal:symbol"] = "$",
            };

            var config = new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource() { InitialData = source })
                .Build()
                .GetSection("format")
                .Get<FormatOptions>();

            Console.WriteLine("DateTime");
            Console.WriteLine(config.DateTime.LongDatePattern);
            Console.WriteLine(config.DateTime.LongTimePattern);
            Console.WriteLine(config.DateTime.ShortTimePattern);
            Console.WriteLine(config.DateTime.ShortDatePattern);
            Console.WriteLine("CurrencyDecimal");
            Console.WriteLine(config.CurrencyDecimal.Digits);
            Console.WriteLine(config.CurrencyDecimal.Symbol);

        }

        static void Test4()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("format")
                .Get<FormatOptions>();

            Console.WriteLine("DateTime");
            Console.WriteLine(config.DateTime.LongDatePattern);
            Console.WriteLine(config.DateTime.LongTimePattern);
            Console.WriteLine(config.DateTime.ShortTimePattern);
            Console.WriteLine(config.DateTime.ShortDatePattern);
            Console.WriteLine("CurrencyDecimal");
            Console.WriteLine(config.CurrencyDecimal.Digits);
            Console.WriteLine(config.CurrencyDecimal.Symbol);
        }

        static void Test5(string[] args)
        {
            var index = Array.IndexOf(args, "/env");
            var environment = index > -1
                ? args[index + 1]
                : "Development";

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json",false)
            .AddJsonFile($"appsettings.{environment}.json",true)
            .Build()
            .GetSection("format")
            .Get<FormatOptions>();

            Console.WriteLine("DateTime");
            Console.WriteLine(config.DateTime.LongDatePattern);
            Console.WriteLine(config.DateTime.LongTimePattern);
            Console.WriteLine(config.DateTime.ShortTimePattern);
            Console.WriteLine(config.DateTime.ShortDatePattern);
            Console.WriteLine("CurrencyDecimal");
            Console.WriteLine(config.CurrencyDecimal.Digits);
            Console.WriteLine(config.CurrencyDecimal.Symbol);
        }

        static void Test6()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();
           
            ChangeToken.OnChange(() => config.GetReloadToken(), CallBack);
            void CallBack()
            {
                var options = config.GetSection("format").Get<FormatOptions>();
                var dateTime = options.DateTime;
                var currencyDecimal = options.CurrencyDecimal;

                Console.WriteLine("DateTime:");
                Console.WriteLine($"\tLongDatePattern: {dateTime.LongDatePattern}");
                Console.WriteLine($"\tLongTimePattern: {dateTime.LongTimePattern}");
                Console.WriteLine($"\tShortDatePattern: {dateTime.ShortDatePattern}");
                Console.WriteLine($"\tShortTimePattern: {dateTime.ShortTimePattern}");

                Console.WriteLine("CurrencyDecimal:");
                Console.WriteLine($"\tDigits:{currencyDecimal.Digits}");
                Console.WriteLine($"\tSymbol:{currencyDecimal.Symbol}\n\n");
            }
            Console.Read();
        }

        static void Test7()
        {
            var source = new Dictionary<string, string>
            {
                ["foo"] = null,
                ["bar"] = "",
                ["baz"] = "123",
            };

            var config = new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource() { InitialData = source })
                .Build();

            Debug.Assert(config.GetValue<Object>("foo") == null);
            Debug.Assert("".Equals(config.GetValue<Object>("bar")));
            Debug.Assert("123".Equals(config.GetValue<Object>("baz")));

            Debug.Assert(config.GetValue<int>("foo") == 0);
            Debug.Assert(config.GetValue<int>("baz") == 123);

            Debug.Assert(config.GetValue<int?>("foo") == null);
            Debug.Assert(config.GetValue<int?>("bar") == null);

        }

        static void Test8()
        {
            var source = new Dictionary<string, string>
            {
                ["point"] = "(123,456)"
            };

            var config = new ConfigurationBuilder()
                .Add(new MemoryConfigurationSource() { InitialData = source })
                .Build();

            var point = config.GetValue<Point>("point");
            Debug.Assert(point.X == 123);
            Debug.Assert(point.Y == 456);

        }

        static void Test9()
        {
            var source = new Dictionary<string, string>
            {
                ["gender"] = "Male",
                ["age"] = "12",
                ["contactInfo:emailAddress"] = "@out",
                ["contactInfo:phoneNo"] = "1234",
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(source)
                .Build();

            var profile = config.Get<Profile>();
            Debug.Assert(profile.Equals(new Profile(Gender.Male,12, "@out","1234")));

        }

        static void Test10()
        {
            var source = new Dictionary<string, string>
            {
                ["foo:gender"] = "Male",
                ["foo:age"] = "12",
                ["foo:contactInfo:emailAddress"] = "@out",
                ["foo:contactInfo:phoneNo"] = "1134",

                ["bar:gender"] = "Male",
                ["bar:age"] = "13",
                ["bar:contactInfo:emailAddress"] = "@out",
                ["bar:contactInfo:phoneNo"] = "1334",

                ["baz:gender"] = "Female",
                ["baz:age"] = "11",
                ["baz:contactInfo:emailAddress"] = "@out",
                ["baz:contactInfo:phoneNo"] = "1235",
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(source)
                .Build();

            var profiles = new Profile[3]
            {
                new Profile(Gender.Male,12,"@out","1134"), 
                new Profile(Gender.Male,13,"@out","1334"), 
                new Profile(Gender.Female,11,"@out","1235"),
            };

            var collections = config.Get<IEnumerable<Profile>>();
            Debug.Assert(collections.Any(it => it.Equals(profiles[0])));
            Debug.Assert(collections.Any(it => it.Equals(profiles[1])));
            Debug.Assert(collections.Any(it => it.Equals(profiles[2])));

            var array = config.Get<Profile[]>();
            Debug.Assert(array[0].Equals(profiles[1]));
            Debug.Assert(array[1].Equals(profiles[2]));
            Debug.Assert(array[2].Equals(profiles[0]));
        }

        static void Test11()
        {
            var source = new Dictionary<string, string>
            {
                ["foo:gender"] = "Male",
                ["foo:age"] = "12",
                ["foo:contactInfo:emailAddress"] = "@out",
                ["foo:contactInfo:phoneNo"] = "1134",

                ["bar:gender"] = "Male",
                ["bar:age"] = "13",
                ["bar:contactInfo:emailAddress"] = "@out",
                ["bar:contactInfo:phoneNo"] = "1334",

                ["baz:gender"] = "Female",
                ["baz:age"] = "11",
                ["baz:contactInfo:emailAddress"] = "@out",
                ["baz:contactInfo:phoneNo"] = "1235",
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(source)
                .Build();


            var collections = config.Get<IDictionary<string,Profile>>();
            Debug.Assert(collections["foo"].Equals(
                new Profile(Gender.Male, 12, "@out", "1134")));
            Debug.Assert(collections["bar"].Equals(
                new Profile(Gender.Male, 13, "@out", "1334")));
            Debug.Assert(collections["baz"].Equals(
                new Profile(Gender.Female, 11, "@out", "1235")));
        }
    }
}
