using EasyDI.Interface;

namespace EasyDI
{
    [MapTo(typeof(IQux),LifeTime.Root)]
    public class Qux : Base, IQux
    {
        
    }
}