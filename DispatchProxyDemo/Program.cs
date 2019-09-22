using System;
using System.Reflection;

namespace DispatchProxyDemo
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class ProxyGenerator : DispatchProxy
    {
        private object target;

        public void SetTarget(object target)
        {
            this.target = target;
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            Console.WriteLine(" before Invoking...");
            var result = targetMethod.Invoke(target, args);
            Console.WriteLine(" after Invoking...");
            return result;
        }

        public static T Generate<T, Ttarget>(Ttarget target)
        {
            var proxy = DispatchProxy.Create<T, ProxyGenerator>();
            var wrapper = proxy as ProxyGenerator;
            wrapper?.SetTarget(target);
            //((WrapperProxy)proxy).SetTarget(target);
            return proxy;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = ProxyGenerator.Generate<ILogger, ConsoleLogger>(new ConsoleLogger());
            proxy.Log("This is test message");
        }
    }
}
