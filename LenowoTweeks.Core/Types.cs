namespace LenowoTweeks.Core;

using ResoniteModLoader;

public interface ILenowoModule
{
	public string ModuleName { get; }
	public string ModuleVersion { get; }
}

public abstract class LenowoTweak : ResoniteMod, ILenowoModule
{
	public abstract string ModuleName { get; }
	public abstract string ModuleVersion { get; }

	public override void OnEngineInit()
	{
		LenowoTweeks_Core.RegisterModule(this);

		Init();
	}


	public virtual void Init() { }
}
