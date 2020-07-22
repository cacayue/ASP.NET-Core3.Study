using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace FileSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Test1();
            //await Test2();
            //await Test3();
            await Test4();
        }

        static void Test1()
        {
            static void Print(int layer, string name)
                => Console.WriteLine($"{new string(' ', layer * 4)}{name}");
            dynamic type = (new Program()).GetType();
            string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
            Console.WriteLine(currentDirectory);
            var path = Directory.GetCurrentDirectory();
            new ServiceCollection()
                .AddSingleton<IFileProvider>(new PhysicalFileProvider($@"F:\test"))
                .AddSingleton<IFileManager, FileManager>()
                .BuildServiceProvider()
                .GetRequiredService<IFileManager>()
                .ShowStructure(Print);
        }

        static async Task Test2()
        {
            var content = await new ServiceCollection()
                .AddSingleton<IFileProvider>(new PhysicalFileProvider(@"F:\test"))
                .AddSingleton<IFileManager, FileManager>()
                .BuildServiceProvider()
                .GetRequiredService<IFileManager>()
                .ReadAllTextAsync("data.txt");
            Debug.Assert(content == File.ReadAllText(@"F:\test\data.txt"));
        }

        static async Task Test3()
        {
            var assembly = Assembly.GetEntryAssembly();

            var content = await new ServiceCollection()
                .AddSingleton<IFileProvider>(new EmbeddedFileProvider(assembly))
                .AddSingleton<IFileManager, FileManager>()
                .BuildServiceProvider()
                .GetRequiredService<IFileManager>()
                .ReadAllTextAsync("data.txt");
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.data.txt");
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var content2 = Encoding.Default.GetString(buffer);
            Debug.Assert(content == content2);
        }

        static async Task Test4()
        {
            using (var fileProvider = new PhysicalFileProvider(@"F:\test"))
            {
                string original = null;
                ChangeToken.OnChange(() => fileProvider.Watch("data.txt"), CallBack);
                while (true)
                {
                    await File.WriteAllTextAsync(@"F:\test\data.txt", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    await Task.Delay(2000);
                }

                async void CallBack()
                {
                    await using (var stream = fileProvider.GetFileInfo("data.txt").CreateReadStream())
                    {
                        var buffer = new byte[stream.Length];
                        await stream.ReadAsync(buffer, 0, buffer.Length);
                        string current = Encoding.Default.GetString(buffer);
                        if (current != original)
                        {
                            Console.WriteLine(original = current);
                        }
                    }
                }
            }
        }
    }
}
