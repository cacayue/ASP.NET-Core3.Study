using System.Runtime.InteropServices.ComTypes;

namespace DI
{
    public static class CatExtensions
    {
        public static T GetService<T>(this Cat cat)
        {
            return default(T);
        }

        public static T Register<T1, T2, T>(this Cat cat)
        {
            return default(T);
        }
    }
}