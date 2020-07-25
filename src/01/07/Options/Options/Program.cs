using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;
using Configuration.KeyValue;
using Configuration.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;

namespace Options
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test7(args);
        }

        static void Test1()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("profile1.json")
                .Build();
            var profile = new ServiceCollection()
                .AddOptions()
                .Configure<Profile>(configuration)
                .BuildServiceProvider()
                .GetRequiredService<IOptions<Profile>>()
                .Value;
            Console.WriteLine(profile.Age);
            Console.WriteLine(profile.Gender);
            Console.WriteLine(profile.ContactInfo.EmailAddress);
            Console.WriteLine(profile.ContactInfo.PhoneNo);
        }

        static void Test2()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("profile2.json")
                .Build();
            var serviceProvider = new ServiceCollection()
                    .AddOptions()
                    .Configure<Profile>("foo", configuration.GetSection("foo"))
                    .Configure<Profile>("bar", configuration.GetSection("bar"))
                    .BuildServiceProvider()
                ;
            var optionsAccessor = serviceProvider.GetRequiredService<IOptionsSnapshot<Profile>>();
            Print(optionsAccessor.Get("foo"));
            Console.WriteLine();
            Print(optionsAccessor.Get("bar"));


            void Print(Profile profile)
            {
                Console.WriteLine(profile.Age);
                Console.WriteLine(profile.Gender);
                Console.WriteLine(profile.ContactInfo.EmailAddress);
                Console.WriteLine(profile.ContactInfo.PhoneNo);
            }
        }

        static void Test3()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("profile1.json", false, true)
                .Build();
            var service = new ServiceCollection()
                    .AddOptions()
                    .Configure<Profile>(configuration)
                    .BuildServiceProvider()
                    .GetRequiredService<IOptionsMonitor<Profile>>()
                ;
            Console.WriteLine(service.CurrentValue.Age);
            service.OnChange(profile =>
            {
                Console.WriteLine(profile.Age);
                Console.WriteLine(profile.Gender);
                Console.WriteLine(profile.ContactInfo.EmailAddress);
                Console.WriteLine(profile.ContactInfo.PhoneNo);
            });
            Console.Read();
        }

        static void Test4()
        {
            var profile = new ServiceCollection()
                .AddOptions()
                .Configure<Profile>(p =>
                {
                    p.Age = 18;
                    p.Gender = Gender.Female;
                    p.ContactInfo = new ContactInfo()
                    {
                        EmailAddress = "aa",
                        PhoneNo = "12312"
                    };
                })
                .BuildServiceProvider()
                .GetRequiredService<IOptions<Profile>>()
                .Value;
            Console.WriteLine("------Profile-------");
            Console.WriteLine(profile.Age);
            Console.WriteLine(profile.Gender);
            Console.WriteLine(profile.ContactInfo.EmailAddress);
            Console.WriteLine(profile.ContactInfo.PhoneNo);
        }

        static void Test5()
        {
            var serviceProvider = new ServiceCollection()
                    .AddOptions()
                    .Configure<Profile>("foo", p =>
                    {
                        p.Age = 18;
                        p.Gender = Gender.Male;
                        p.ContactInfo = new ContactInfo()
                        {
                            EmailAddress = "aa",
                            PhoneNo = "12312"
                        };
                    })
                    .Configure<Profile>("bar", p =>
                    {
                        p.Age = 15;
                        p.Gender = Gender.Female;
                        p.ContactInfo = new ContactInfo()
                        {
                            EmailAddress = "aa",
                            PhoneNo = "12312"
                        };
                    })
                    .BuildServiceProvider()
                ;
            var optionsAccessor = serviceProvider.GetRequiredService<IOptionsSnapshot<Profile>>();
            Print(optionsAccessor.Get("foo"));
            Console.WriteLine();
            Print(optionsAccessor.Get("bar"));


            void Print(Profile profile)
            {
                Console.WriteLine(profile.Age);
                Console.WriteLine(profile.Gender);
                Console.WriteLine(profile.ContactInfo.EmailAddress);
                Console.WriteLine(profile.ContactInfo.PhoneNo);
            }
        }

        static void Test6(string[] args)
        {
            var environment = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build()["env"];

            var serivces = new ServiceCollection();
            serivces.AddSingleton<IHostEnvironment>(
                    sp => new HostingEnvironment() {EnvironmentName = environment})
                .AddOptions<DateTimeFormatOptions>()
                .Configure<IHostEnvironment>(
                    (options, env) =>
                    {
                        if (env.IsDevelopment())
                        {
                            options.DatePattern = "dddd,MMMM d, yyyy";
                            options.TimePattern = "M/d/yyyy";
                        }
                        else
                        {
                            options.DatePattern = "M/d/yyyy";
                            options.TimePattern = "h:mm tt";
                        }
                    });

            var options = serivces
                .BuildServiceProvider()
                .GetRequiredService<IOptions<DateTimeFormatOptions>>().Value;
            Console.WriteLine(options);
        }

        static void Test7(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var datePattern = config["date"];
            var timePattern = config["time"];

            var services = new ServiceCollection();
            services.AddOptions<DateTimeFormatOptions>()
                .Configure(options =>
                {
                    options.DatePattern = datePattern;
                    options.TimePattern = timePattern;
                })
                .Validate(options => Validate(options.DatePattern)
                                     && Validate(options.TimePattern), "Invalid Date or Time pattern");
            try
            {
                var options = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<DateTimeFormatOptions>>().Value;
                Console.WriteLine(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            static bool Validate(string format)
            {
                var time = new DateTime(1981,8,24,2,2,2);
                var formatted = time.ToString(format);
                return DateTimeOffset.TryParseExact(formatted, format, null, DateTimeStyles.None, out var value)
                       && (value.Date == time.Date || value.TimeOfDay == time.TimeOfDay);
            }
        }
    }
}
