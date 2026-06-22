using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using Renderite.Shared;
using ResoniteModLoader;

namespace LenowoTweeks;

public class LenowoTweeks : ResoniteMod
{
	internal const string VERSION_CONSTANT = "1.1.3";

	private const string ModName = "Lenowo Tweeks";

	public const string harmonyID = "com.Lenowo.LenowoTweeks";

	private static Harmony harmony;

	public static ResoniteMod? instance;

	public static ModConfigKey<bool> expandedStringInputs;

	public static ModConfigKey<bool> modifiedInspectorUIX;

	public static ModConfigKey<bool> slotInspectorResetAll;

	public static ModConfigKey<bool> collapseComponents;

	public static ModConfigKey<colorX> collapsedComponentColor;

	public static ModConfigKey<colorX> expandedComponentColor;

	public static ModConfigKey<bool> modifiedComponentHeaders;

	public static ModConfigKey<string> dynvarComponentHeaderName;

	public static ModConfigKey<bool> listCollapsing;

	public static ModConfigKey<int> maxListElementsForAutoCollapse;

	public static ModConfigKey<bool> enableAddChildrenBuilder;

	public static ModConfigKey<float> buttonMinHeightDefault;

	public static ModConfigKey<bool> disableRelayBackground;

	public static ModConfigKey<bool> disablePhysicalInteraction;

	public static ModConfigKey<bool> protofluxEditableNames;

	public static ModConfigKey<bool> collapsibleProtoflux;

	public static ModConfigKey<int> collapseThreshold;

	public static ModConfigKey<int> collapseAwakeDelay;

	public static ModConfigKey<TextureWrapMode> wireImageWrapMode;

	public static ModConfigKey<TextureFilterMode> wireTextureFilterMode;

	public static ModConfigKey<bool> useCustomProtofluxConnections;

	public static ModConfigKey<Uri> customProtofluxWireUIX;

	public static ModConfigKey<Uri> customProtofluxConnectorUIX;

	public static ModConfigKey<Uri?> customProtofluxEmptyConnectorUIX;

	public static ModConfigKey<bool> wireConnectorFlipUV;

	public static ModConfigKey<colorX> wireTextureColor;

	public static ModConfigKey<colorX> connectorTextureColor;

	public static ModConfigKey<bool> initializeProtofluxGlobals;

	public static ModConfigKey<ValueFieldDropMode> valueFieldDroppingMode;

	public static ModConfigKey<string> worldSearchGlobalTags;

	public static ModConfigKey<bool> nohelp;

	public static ModConfigKey<bool> AllowGooberUnpack;

	public static ModConfigKey<bool> InspectNodeShortcut;

	public static ModConfigKey<colorX> defaultUIXPanelColor;

	public static ModConfigKey<colorX> primaryUIColor;

	public static ModConfigKey<colorX> secondaryUIColor;

	public static ModConfigKey<bool> ensureConfigSpace;

	public static ModConfigKey<string> newConfigOption;

	public static ModConfiguration? Config;

	public static readonly Dictionary<string, Dictionary<string, List<ModConfigKey>>> SortedConfigKeys;

	private static Assembly ModAssembly => typeof(LenowoTweeks).Assembly;

	public override string Name => "Lenowo Tweeks";

	public override string Author => "Rosa";

	public override string Version => "1.1.3";

	public override string Link => "https://github.com/bobjbo/Lenowo-Tweaks";

	public static void GenerateUI(UIBuilder ui)
	{
		new ConfigUIBuilder(instance?.GetConfiguration()).BuildConfigUI(ui, SortedConfigKeys);
	}

	public static void ModSettings_BuildModUi(UIBuilder ui)
	{
		ResoniteModBase obj = ModLoader.Mods().Last((ResoniteModBase m) => m.Name == "Lenowo Tweeks");
		obj.InvokeMethod("GenerateUI", ui);
	}

	public override void DefineConfiguration(ModConfigurationDefinitionBuilder builder)
	{
		builder.AutoSave(autoSave: true);
		foreach (Dictionary<string, List<ModConfigKey>> value in SortedConfigKeys.Values)
		{
			foreach (List<ModConfigKey> value2 in value.Values)
			{
				foreach (ModConfigKey item in value2)
				{
					builder.Key(item.ConfigKey);
				}
			}
		}
	}

	static LenowoTweeks()
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		harmony = new Harmony("com.Lenowo.LenowoTweeks");
		expandedStringInputs = new ModConfigKey<bool>("Expanded String Inputs", "This toggles if string fields on protoflux and inspectors should get bigger based on the text in them.", defaultValue: false, "expandedStringInputs");
		modifiedInspectorUIX = new ModConfigKey<bool>("Modified Inspector UIX", "This toggles if the inspector UIX Fields should load in the custom way.", defaultValue: false, "modifiedInspectorUIX");
		slotInspectorResetAll = new ModConfigKey<bool>("Slot Inspector Reset All", "This toggles if a 'Reset All' button is created on inspectors", defaultValue: false, "slotInspectorResetAll");
		collapseComponents = new ModConfigKey<bool>("Collapse Components", "This toggles if the components in the inspector should load collapsed.", defaultValue: false, "collapseComponents");
		collapsedComponentColor = new ModConfigKey<colorX>("Collapsed Component Color", "This text color of a collapsed component.", MidLight.YELLOW, "collapsedComponentColor");
		expandedComponentColor = new ModConfigKey<colorX>("Expanded Component Color", "This text color of a expanded component.", RadiantUI_Constants.LABEL_COLOR);
		modifiedComponentHeaders = new ModConfigKey<bool>("Modified Component Headers", "This toggles if the component headers can be modified on certain types", defaultValue: true);
		dynvarComponentHeaderName = new ModConfigKey<string>("Dynvar Component Header Name", "Replaces ['Variable','Field','Reference'] with the provided text on dynamic variables in the component header (Formatted as 'Variable;Field;Reference')", "");
		listCollapsing = new ModConfigKey<bool>("List Collapsing", "If lists should be able to collapse", defaultValue: true, "listCollapsing");
		maxListElementsForAutoCollapse = new ModConfigKey<int>("Max List Elements For Auto Collapse", "The maximum amount of list elements before it collapses by default. -1 to never collapse, -2 to always collapse.", 25, "maxListElementsForAutoCollapse");
		enableAddChildrenBuilder = new ModConfigKey<bool>("Enable Add Children Builder", "Enables a custom UIX panel for quickly creating UIX and Context Menu's", defaultValue: false, "enableAddChildrenBuilder");
		buttonMinHeightDefault = new ModConfigKey<float>("Button Min Height Default", "The default value for the min height property on the UIX Builder", 0f, "buttonMinHeightDefault");
		disableRelayBackground = new ModConfigKey<bool>("Disable Relay Background", "Disables the background for relays.", defaultValue: false, "disableRelayBackground");
		disablePhysicalInteraction = new ModConfigKey<bool>("Disable Physical Interaction", "Disables physical touch on protoflux nodes.", defaultValue: false);
		protofluxEditableNames = new ModConfigKey<bool>("Protoflux Editable Node Names", "Allows Protoflux inputs/displays/calls to be renamed using a button", defaultValue: false, "protofluxEditableNames");
		collapsibleProtoflux = new ModConfigKey<bool>("Collapsible Protoflux Nodes", "Allows Protoflux Nodes to be collapsed", defaultValue: false);
		collapseThreshold = new ModConfigKey<int>("Protoflux Collapse Threshold", "How many inputs/outputs a node can have before it collapses", 2);
		collapseAwakeDelay = new ModConfigKey<int>("WireManager Awake Update Delay", "How many updates to wait before running the onAwake function (this might need to be increased if users are lagging)", 5);
		wireImageWrapMode = new ModConfigKey<TextureWrapMode>("Wire ImageWrap Mode", "The wrapping mode to use for the protoflux wire texture", (TextureWrapMode)0, "wireImageWrapMode");
		wireTextureFilterMode = new ModConfigKey<TextureFilterMode>("Wire Texture Filter Mode", "The mode to use for the wire texture filtering.", (TextureFilterMode)0, "wireTextureFilterMode");
		useCustomProtofluxConnections = new ModConfigKey<bool>("Use Custom Protoflux Connections", "This toggles if protoflux should use the above uris when generating.", defaultValue: false, "useCustomProtofluxConnections");
		customProtofluxWireUIX = new ModConfigKey<Uri>("Custom Protoflux Wire Image", "This url is used to replace the wires on protoflux nodes.", new Uri("resdb:///1199546a9976a6a907aebfd4e4b45663f7559efd007f03e28e93f26773812f99.png"), "customProtofluxWireUIX");
		customProtofluxConnectorUIX = new ModConfigKey<Uri>("Custom Protoflux Connector Image", "This url is used to replace the connectors on protoflux nodes.", new Uri("resdb:///b09b97338a59244be33fcf1b3366f23bf823c09dd556d7818b65b79801611b47.png"), "customProtofluxConnectorUIX");
		customProtofluxEmptyConnectorUIX = new ModConfigKey<Uri>("Custom Protoflux Empty Connector Image", "This url is used to replace the connectors on protoflux nodes when nothing is connected.", null, "customProtofluxEmptyConnectorUIX");
		wireConnectorFlipUV = new ModConfigKey<bool>("Wire Connector Flip UV", "Toggles if the protoflux connector visual uses left=output right=input rather than the other way around.", defaultValue: false, "wireConnectorFlipUV");
		wireTextureColor = new ModConfigKey<colorX>("Wire Texture Color", "This controls the color to be multiplied with the type color of the wires. white=normal, black=black, red=string normal but int black", new colorX(1f, 1f, (ColorProfile)1));
		connectorTextureColor = new ModConfigKey<colorX>("Connector Texture Color", "This controls the color to be multiplied with the type color of the connectors. white=normal, black=black, red=string normal but int black", new colorX(1f, 1f, (ColorProfile)1));
		initializeProtofluxGlobals = new ModConfigKey<bool>("Initialize Protoflux Globals", "When enabled, this will try to initialize globals in protoflux, like string inputs for DynamicInputs or booleans in Update", defaultValue: false);
		valueFieldDroppingMode = new ModConfigKey<ValueFieldDropMode>("ValueField Dropping Mode", "This controls how ValueFields are allowed to drop into TextFields.\t <size=90%>AlwaysAllow = Works as normal, AllowIfNotSelf = Prevent ValueFields under the same ObjectRoot as the TextField, NeverAllow = Never drop ValueFields</size>", ValueFieldDropMode.AlwaysAllow);
		worldSearchGlobalTags = new ModConfigKey<string>("Global World Search Filter", "Global Tags to apply to all world searches.", "", "worldSearchGlobalTags");
		nohelp = new ModConfigKey<bool>("Disable Help Buttons", "disables the help buttons in the inspctor and context menu", defaultValue: false, "nohelp");
		AllowGooberUnpack = new ModConfigKey<bool>("Allow GooberPrint Unpack", "If the held slot is a GooberPrint packed slot, allow the spawning of a new print and auto unpack rather than normal unpacking", defaultValue: false);
		InspectNodeShortcut = new ModConfigKey<bool>("Allow Opening Inspector On Nodes", "If enabled, adds a new context menu item, which will open an inspector on the hovered ProtoFluxNode", defaultValue: false, "nodeQuickInspect");
		defaultUIXPanelColor = new ModConfigKey<colorX>("Default UIX Panel Color", "This controls the color that is used when creating a blank UIX panel", colorX.DarkGray);
		primaryUIColor = new ModConfigKey<colorX>("Primary UI Color", "Controls the primary color used for any custom UI in this mod", Hero.YELLOW);
		secondaryUIColor = new ModConfigKey<colorX>("Secondary UI Color", "Controls the primary color used for any custom UI in this mod", Hero.ORANGE);
		ensureConfigSpace = new ModConfigKey<bool>("Ensure Config Space", "If enabled, will ensure that the 'mod config' slot exists when loading into a world", defaultValue: false);
		newConfigOption = new ModConfigKey<string>("Config Display Name", "Config Description", "Default Value");
		Dictionary<string, Dictionary<string, List<ModConfigKey>>> dictionary = new Dictionary<string, Dictionary<string, List<ModConfigKey>>>();
		Dictionary<string, List<ModConfigKey>> dictionary2 = new Dictionary<string, List<ModConfigKey>>();
		int num = 7;
		List<ModConfigKey> list = new List<ModConfigKey>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<ModConfigKey> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
		span[num2] = expandedStringInputs;
		num2++;
		span[num2] = valueFieldDroppingMode;
		num2++;
		span[num2] = worldSearchGlobalTags;
		num2++;
		span[num2] = nohelp;
		num2++;
		span[num2] = ensureConfigSpace;
		num2++;
		span[num2] = primaryUIColor;
		num2++;
		span[num2] = secondaryUIColor;
		dictionary2.Add("Base", list);
		dictionary.Add("General", dictionary2);
		Dictionary<string, List<ModConfigKey>> dictionary3 = new Dictionary<string, List<ModConfigKey>>();
		num2 = 3;
		List<ModConfigKey> list2 = new List<ModConfigKey>(num2);
		CollectionsMarshal.SetCount(list2, num2);
		Span<ModConfigKey> span2 = CollectionsMarshal.AsSpan(list2);
		num = 0;
		span2[num] = modifiedInspectorUIX;
		num++;
		span2[num] = slotInspectorResetAll;
		num++;
		span2[num] = defaultUIXPanelColor;
		dictionary3.Add("Base", list2);
		num = 5;
		List<ModConfigKey> list3 = new List<ModConfigKey>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<ModConfigKey> span3 = CollectionsMarshal.AsSpan(list3);
		num2 = 0;
		span3[num2] = collapseComponents;
		num2++;
		span3[num2] = collapsedComponentColor;
		num2++;
		span3[num2] = expandedComponentColor;
		num2++;
		span3[num2] = modifiedComponentHeaders;
		num2++;
		span3[num2] = dynvarComponentHeaderName;
		dictionary3.Add("Components", list3);
		num2 = 2;
		List<ModConfigKey> list4 = new List<ModConfigKey>(num2);
		CollectionsMarshal.SetCount(list4, num2);
		Span<ModConfigKey> span4 = CollectionsMarshal.AsSpan(list4);
		num = 0;
		span4[num] = listCollapsing;
		num++;
		span4[num] = maxListElementsForAutoCollapse;
		dictionary3.Add("Lists", list4);
		num = 1;
		List<ModConfigKey> list5 = new List<ModConfigKey>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<ModConfigKey> span5 = CollectionsMarshal.AsSpan(list5);
		num2 = 0;
		span5[num2] = enableAddChildrenBuilder;
		dictionary3.Add("Actions", list5);
		dictionary.Add("Inspectors", dictionary3);
		Dictionary<string, List<ModConfigKey>> dictionary4 = new Dictionary<string, List<ModConfigKey>>();
		num2 = 6;
		List<ModConfigKey> list6 = new List<ModConfigKey>(num2);
		CollectionsMarshal.SetCount(list6, num2);
		Span<ModConfigKey> span6 = CollectionsMarshal.AsSpan(list6);
		num = 0;
		span6[num] = protofluxEditableNames;
		num++;
		span6[num] = disableRelayBackground;
		num++;
		span6[num] = disablePhysicalInteraction;
		num++;
		span6[num] = initializeProtofluxGlobals;
		num++;
		span6[num] = AllowGooberUnpack;
		num++;
		span6[num] = InspectNodeShortcut;
		dictionary4.Add("Base", list6);
		num = 5;
		List<ModConfigKey> list7 = new List<ModConfigKey>(num);
		CollectionsMarshal.SetCount(list7, num);
		Span<ModConfigKey> span7 = CollectionsMarshal.AsSpan(list7);
		num2 = 0;
		span7[num2] = useCustomProtofluxConnections;
		num2++;
		span7[num2] = wireImageWrapMode;
		num2++;
		span7[num2] = wireTextureFilterMode;
		num2++;
		span7[num2] = wireTextureColor;
		num2++;
		span7[num2] = connectorTextureColor;
		dictionary4.Add("Image Settings", list7);
		num2 = 1;
		List<ModConfigKey> list8 = new List<ModConfigKey>(num2);
		CollectionsMarshal.SetCount(list8, num2);
		Span<ModConfigKey> span8 = CollectionsMarshal.AsSpan(list8);
		num = 0;
		span8[num] = customProtofluxWireUIX;
		dictionary4.Add("Wires", list8);
		num = 3;
		List<ModConfigKey> list9 = new List<ModConfigKey>(num);
		CollectionsMarshal.SetCount(list9, num);
		Span<ModConfigKey> span9 = CollectionsMarshal.AsSpan(list9);
		num2 = 0;
		span9[num2] = customProtofluxConnectorUIX;
		num2++;
		span9[num2] = customProtofluxEmptyConnectorUIX;
		num2++;
		span9[num2] = wireConnectorFlipUV;
		dictionary4.Add("Connectors", list9);
		num2 = 2;
		List<ModConfigKey> list10 = new List<ModConfigKey>(num2);
		CollectionsMarshal.SetCount(list10, num2);
		Span<ModConfigKey> span10 = CollectionsMarshal.AsSpan(list10);
		num = 0;
		span10[num] = collapsibleProtoflux;
		num++;
		span10[num] = collapseThreshold;
		dictionary4.Add("Collapsing", list10);
		dictionary.Add("ProtoFlux", dictionary4);
		SortedConfigKeys = dictionary;
	}

	public override void OnEngineInit()
	{
		Config = GetConfiguration();
		Config.Save(saveDefaultValues: true);
		instance = this;
		harmony.PatchAll(ModAssembly);
	}
}
