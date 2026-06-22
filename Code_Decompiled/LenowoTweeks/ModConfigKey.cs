using System;
using FrooxEngine.UIX;
using ResoniteModLoader;

namespace LenowoTweeks;

public class ModConfigKey
{
	private readonly Type defaultType;

	public ModConfigurationKey ConfigKey;

	public string ConfigName;

	public string ConfigKeyName;

	public string ConfigDescription;

	public ModConfigKey(string keyName, string name, string description, Type type, ModConfigurationKey key)
	{
		defaultType = type;
		ConfigKey = key;
		ConfigName = name;
		ConfigKeyName = keyName;
		ConfigDescription = description;
	}

	public Type ValueType()
	{
		return defaultType;
	}

	public virtual void OnConfigExists()
	{
	}

	public virtual void BuildField(ConfigUIBuilder builder, UIBuilder ui)
	{
	}
}
public class ModConfigKey<T> : ModConfigKey
{
	public ModConfigurationKey<T> TypedConfigKey;

	public T DefaultValue;

	public bool ValueDefined = false;

	public T Value
	{
		get
		{
			return TypedConfigKey.Value;
		}
		set
		{
			TypedConfigKey.Value = value;
		}
	}

	public ModConfigKey(string name, string description, T defaultValue, string overrideKey = "")
		: base((!string.IsNullOrEmpty(overrideKey)) ? overrideKey : name, name, description, typeof(T), new ModConfigurationKey<T>((!string.IsNullOrEmpty(overrideKey)) ? overrideKey : name.ToLowerInvariant().Replace(" ", "_"), description, () => defaultValue))
	{
		DefaultValue = defaultValue;
		TypedConfigKey = (ModConfigurationKey<T>)ConfigKey;
	}

	public override void BuildField(ConfigUIBuilder builder, UIBuilder ui)
	{
		builder.BuildGenericField(ui, this);
	}
}
