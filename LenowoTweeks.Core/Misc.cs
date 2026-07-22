using System.Reflection;

using HarmonyLib;

namespace LenowoTweeks.Core;

public static class Misc
{
	public static T GetField<T>(this object obj, string fieldName)
	{
		return Traverse.Create(obj).Field<T>(fieldName).Value;
	}

	public static T GetProperty<T>(this object obj, string propertyName)
	{
		return Traverse.Create(obj).Property<T>(propertyName).Value;
	}

	public static MethodInfo? GetMethodInfo(this object obj, string methodName, Type[]? parameterTypes = null)
	{
		BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		return (parameterTypes == null) ? obj.GetType().GetMethod(methodName, flags) : obj.GetType().GetMethod(methodName, flags, null, parameterTypes, null);
	}

	public static MethodInfo[]? GetAllMethods(this object obj, Type[]? parameterTypes = null)
	{
		BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		return (parameterTypes == null) ? obj.GetType().GetMethods(flags) : [.. obj.GetType().GetMethods(flags).Where(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes))];
	}

	public static TDelegate? GetMethodDelegate<TDelegate>(this object obj, string methodName) where TDelegate : Delegate
	{
		return ((TDelegate)(GetMethodInfo(obj, methodName)?.CreateDelegate(typeof(TDelegate), obj))) ?? null;
	}

	public static Delegate? GetMethodDelegate(this object obj, string methodName, Type delegateType)
	{
		return GetMethodInfo(obj, methodName)?.CreateDelegate(delegateType, obj) ?? null;
	}

	public static void InvokeMethod(this object obj, string methodName, params object[] args)
	{
		GetMethodInfo(obj, methodName)?.Invoke(obj, args);
	}

	public static T? InvokeMethod<T>(this object obj, string methodName, params object[] args)
	{
		T val = (T)(GetMethodInfo(obj, methodName)?.Invoke(obj, args));
		return (val != null) ? val : default;
	}
}
