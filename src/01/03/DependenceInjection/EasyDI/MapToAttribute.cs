using System;

namespace EasyDI
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class MapToAttribute:Attribute
    {
        public Type ServiceType { get; }
        public LifeTime LifeTime { get; }

        public MapToAttribute(Type serviceType, LifeTime lifeTime)
        {
            ServiceType = serviceType;
            LifeTime = lifeTime;
        }
    }
}