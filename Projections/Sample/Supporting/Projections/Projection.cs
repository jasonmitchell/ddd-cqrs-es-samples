using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Supporting.Projections
{
    public abstract class Projection
    {
        internal Dictionary<Type, List<Func<object, Task>>> Handlers { get; } = new Dictionary<Type, List<Func<object, Task>>>();
        internal Dictionary<string, Type> EventTypeMap { get; } = new Dictionary<string, Type>();

        protected void When<T>(Func<T, Task> handler)
        {
            if (!Handlers.ContainsKey(typeof(T)))
            {
                Handlers.Add(typeof(T), new List<Func<object, Task>>());
            }

            Handlers[typeof(T)].Add(e => handler((T)e));
            EventTypeMap[typeof(T).Name] = typeof(T);
        }
    }
}