using System.Diagnostics;

namespace Gojek.Utilities
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object instance, string context = null)
        {
            Debug.WriteLine($"Call from: {context}");

            if (instance == null)
#if DEBUG
                throw new System.ArgumentNullException(nameof(instance),
                    $"Unable to sure cast null instance as type '{typeof(T).Name}");
#else
                return default(T);
#endif
            return (T) instance;
        }
    }
}