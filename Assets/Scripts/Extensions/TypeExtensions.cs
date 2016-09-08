using System;
#if NETFX_CORE
using System.Reflection;
#endif


public static class TypeExtensions
{
    public static bool SubclassOf(this Type type, Type other)
    {
#if NETFX_CORE
        return type.GetTypeInfo().IsSubclassOf(other);
#else
        return other.IsAssignableFrom(type);
#endif
    }

    public static bool IsEnum(this Type type)
    {
#if NETFX_CORE
        return type.GetTypeInfo().IsEnum;
#else
        return type.IsEnum;
#endif
    }

}
