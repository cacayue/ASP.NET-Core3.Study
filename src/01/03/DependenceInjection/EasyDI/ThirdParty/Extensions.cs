using Microsoft.Extensions.DependencyInjection;

namespace EasyDI
{
    public static class Extensions
    {
        public static LifeTime AsCatLigLifeTime(this ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    return LifeTime.Self;
                case ServiceLifetime.Singleton:
                    return LifeTime.Root;
                default:
                    return LifeTime.Transient;
            }
            //return lifetime switch
            //{
            //    ServiceLifetime.Scoped => LifeTime.Self,
            //    ServiceLifetime.Singleton => LifeTime.Root,
            //    _ => LifeTime.Transient
            //};
        }
    }
}