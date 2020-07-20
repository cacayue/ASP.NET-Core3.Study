using EasyDI.Interface;

namespace EasyDI
{
    public class Foobar<T1,T2>: IFoobar<T1,T2>
    {
        public T1 Foo { get; }
        public T2 Bar { get; }

        public Foobar(T1 foo,T2 bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}