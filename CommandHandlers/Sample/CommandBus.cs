using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sample
{
    public class CommandBus
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();
        
        public void RegisterHandler<T>(Action<T> handler)
        {
            _handlers.Add(typeof(T), x => Log((T)x, handler));
        }

        public void Send<T>(T command)
        {
            if (!_handlers.ContainsKey(typeof(T))) throw new InvalidOperationException("No handler registered");
            _handlers[typeof(T)](command);
        }

        // Example of composable handlers by wrapping the original handler in another function
        private static void Log<T>(T command, Action<T> next)
        {
            var stopwatch = new Stopwatch();
            
            Console.WriteLine($"AWESOM-O-LOG[{DateTime.UtcNow}]: Beginning handling {typeof(T).Name}");
            stopwatch.Start();

            next(command);
            
            stopwatch.Stop();
            Console.WriteLine($"AWESOM-O-LOG[{DateTime.UtcNow}]: Finished handling {typeof(T).Name}.  Total time: {stopwatch.Elapsed}");
        }
    }
}