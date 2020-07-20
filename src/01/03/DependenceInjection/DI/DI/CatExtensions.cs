namespace DI
{
    public static class CatExtensions
    {
        public static T GetService<T>(this Cat cat)
        {
            return default(T);
        }
    }
}