using System;
using System.Threading;
using System.Collections.Immutable;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HangfireContext
{
    using State = ImmutableDictionary<Type, (Type cls, object val)>;
    using Stack = ImmutableStack<ImmutableDictionary<Type, (Type cls, object val)>>;

    public class ContextManager : IContext, IContextSerialization
    {
        private readonly AsyncLocal<Stack> _state = new AsyncLocal<Stack>();

        private Stack Stack => _state.Value ?? Stack.Empty;

        private State State => Stack.IsEmpty ? State.Empty : Stack.Peek();

        private void Push(State state)
        {
            _state.Value = Stack.Push(state);
        }

        public void Pop()
        {
            _state.Value = _state.Value.Pop();
        }

        public bool TryGet<T>(out T value)
        {
            if (State.TryGetValue(typeof(T), out var obj))
            {
                value = (T)obj.val;
                return true;
            }

            value = default(T);
            return false;
        }

		public TResult With<TInterface, TClass, TResult>(TClass context, Func<TResult> perform)
			where TClass : TInterface
        {
            Push(State.SetItem(typeof(TInterface), (typeof(TClass), context)));

            try
            {
                return perform();
            }
            finally
            {
                Pop();
            }
        }

        public string Peek()
        {            
            return JsonConvert.SerializeObject(State);
        }

        public void Push(string json)
        {
            var state = State;

            var serializer = new JsonSerializer();

            foreach (var item in JObject.Parse(json).Properties())
            {                
                var interfaceType = Type.GetType(item.Name, true, true);
                var classType = Type.GetType(item.Value.Value<string>("Item1"), true, true);
                var val = serializer.Deserialize(item.Value["Item2"].CreateReader(), classType);
                state = state.SetItem(interfaceType, (classType, val));
            }

            Push(state);
        }        
    }
}
