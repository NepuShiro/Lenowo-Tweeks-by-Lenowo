using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace LenowoTweeks;

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
		BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		return (parameterTypes == null) ? obj.GetType().GetMethod(methodName, bindingAttr) : obj.GetType().GetMethod(methodName, bindingAttr, null, parameterTypes, null);
	}

	public static MethodInfo[]? GetAllMethods(this object obj, Type[]? parameterTypes = null)
	{
		BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		return (parameterTypes == null) ? obj.GetType().GetMethods(bindingAttr) : (from m in obj.GetType().GetMethods(bindingAttr)
			where (from p in m.GetParameters()
				select p.ParameterType).SequenceEqual(parameterTypes)
			select m).ToArray();
	}

	public static TDelegate? GetMethodDelegate<TDelegate>(this object obj, string methodName) where TDelegate : Delegate
	{
		return ((TDelegate)(obj.GetMethodInfo(methodName)?.CreateDelegate(typeof(TDelegate), obj))) ?? null;
	}

	public static Delegate? GetMethodDelegate(this object obj, string methodName, Type delegateType)
	{
		return obj.GetMethodInfo(methodName)?.CreateDelegate(delegateType, obj) ?? null;
	}

	public static void InvokeMethod(this object obj, string methodName, params object[] args)
	{
		obj.GetMethodInfo(methodName)?.Invoke(obj, args);
	}

	public static T? InvokeMethod<T>(this object obj, string methodName, params object[] args)
	{
		T val = (T)(obj.GetMethodInfo(methodName)?.Invoke(obj, args));
		return (val != null) ? val : default(T);
	}
}
