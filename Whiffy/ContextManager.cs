using System;
using System.Threading;
using System.Collections.Immutable;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Whiffy
{
    public class ContextManager : IContext
    {
        private readonly AsyncLocal<ImmutableDictionary<Type, object>> _state 
                   = new AsyncLocal<ImmutableDictionary<Type, object>>();

        public bool TryGet<T>(out T value)
        {
            if (_state.Value.TryGetValue(typeof(T), out var obj))
            {
                value = (T)obj;
                return true;
            }

            value = default(T);
            return false;
        }
        
        public R With<T, R>(T context, Func<R> perform)
        {
            var oldState = _state.Value ?? ImmutableDictionary<Type, object>.Empty;
            var newState = oldState;

            _state.Value = oldState.SetItem(typeof(T), context);

            try
            {
                return perform();
            }
            finally
            {
                _state.Value = oldState;
            }
        }

        public string AsJson
        {
            get
            {
                using (var stringWriter = new StringWriter())
                {
                    var serializer = new JsonSerializer();

                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonWriter.WriteStartObject();
                        if (_state.Value != null)
                        {
                            foreach (var item in _state.Value)
                            {
                                jsonWriter.WritePropertyName(item.Key.AssemblyQualifiedName);
                                serializer.Serialize(jsonWriter, item.Value);
                            }
                        }
                        jsonWriter.WriteEndObject();
                    }

                    return stringWriter.ToString();
                }
            }
        }

        public R WithJson<R>(string context, Func<R> perform)
        {
            var oldState = _state.Value ?? ImmutableDictionary<Type, object>.Empty;
            var newState = oldState;

            var serializer = new JsonSerializer();

            foreach (var item in JObject.Parse(context).Properties())
            {
                var type = Type.GetType(item.Name, throwOnError: true, ignoreCase: true);
                var value = serializer.Deserialize(item.Value.CreateReader(), type);

                newState = newState.SetItem(type, value);
            }

            _state.Value = newState;

            try
            {
                return perform();
            }
            finally
            {
                _state.Value = oldState;
            }
        }
    }

    public static class ContextExtensions
    {
        public static T GetRequired<T>(this IContext context)
        {
            if (context.TryGet<T>(out var value))
            {
                return value;
            }

            throw new NotSupportedException($"No contextual object of type {typeof(T)} is available");
        }
    }
}
