using System;
using System.Collections.Generic;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using Renderite.Shared;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtofluxToolHelp_Patch
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxTool), "GenerateMenuItems")]
	public static bool GenerateMenuItems_Prefix(ProtoFluxTool __instance, ContextMenu menu, SyncRefList<ProtoFluxNodeVisual> ____selectedNodes, SyncType ___SpawnNodeType)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit? hit = GetHit((Tool)(object)__instance);
		object obj;
		if (!hit.HasValue)
		{
			obj = null;
		}
		else
		{
			ICollider collider = hit.GetValueOrDefault().Collider;
			if (collider == null)
			{
				obj = null;
			}
			else
			{
				Slot slot = ((IComponent)collider).Slot;
				obj = ((slot != null) ? slot.GetComponentInParents<ProtoFluxNode>((Predicate<ProtoFluxNode>)null, true, false) : null);
			}
		}
		ProtoFluxNode targetNode = (ProtoFluxNode)obj;
		IWorldElement grabbedReference = ((Tool)__instance).GetGrabbedReference();
		Delegate grabbedDelegate = ((Tool)__instance).GetGrabbedDelegate();
		((SyncElementList<SyncRef<ProtoFluxNodeVisual>>)(object)____selectedNodes).RemoveAll((Predicate<SyncRef<ProtoFluxNodeVisual>>)((SyncRef<ProtoFluxNodeVisual> v) => v.Target == null));
		if (grabbedReference != null)
		{
			bool flag = true;
			if (!(grabbedReference is ProtoFluxGlobalRefProxy))
			{
				ProtoFluxReferenceProxy val = (ProtoFluxReferenceProxy)(object)((grabbedReference is ProtoFluxReferenceProxy) ? grabbedReference : null);
				if (val == null)
				{
					Slot slot2 = (Slot)(object)((grabbedReference is Slot) ? grabbedReference : null);
					if (slot2 != null)
					{
						if (((SyncElementList<SyncRef<ProtoFluxNodeVisual>>)(object)____selectedNodes).Count > 0)
						{
							LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.PackInto", new(string, object)[1] { ("name", slot2.Name) });
							colorX? val3 = colorX.Purple;
							menu.AddItem(ref val2, (Uri)null, ref val3).Button.LocalPressed += (ButtonEventHandler)delegate
							{
								//IL_0013: Unknown result type (might be due to invalid IL or missing references)
								//IL_0018: Unknown result type (might be due to invalid IL or missing references)
								List<ProtoFluxNodeGroup> list = Pool.BorrowList<ProtoFluxNodeGroup>();
								Enumerator<ProtoFluxNodeVisual> enumerator = ____selectedNodes.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										ProtoFluxNodeVisual current = enumerator.Current;
										object obj3;
										if (current == null)
										{
											obj3 = null;
										}
										else
										{
											RelayRef<ProtoFluxNode> node = current.Node;
											if (node == null)
											{
												obj3 = null;
											}
											else
											{
												ProtoFluxNode target = ((SyncRef<ProtoFluxNode>)(object)node).Target;
												obj3 = ((target != null) ? target.Group : null);
											}
										}
										ProtoFluxNodeGroup val6 = (ProtoFluxNodeGroup)obj3;
										if (val6 != null)
										{
											list.Add(val6);
										}
									}
								}
								finally
								{
									((IDisposable)enumerator/*cast due to constrained. prefix*/).Dispose();
								}
								foreach (ProtoFluxNodeGroup item in list)
								{
									ProtoFluxVisualHelper.Pack(item, slot2);
								}
								Pool.Return<ProtoFluxNodeGroup>(ref list);
								((SyncElementList<SyncRef<ProtoFluxNodeVisual>>)(object)____selectedNodes).Clear();
								InteractionHandler activeHandler = ((Tool)__instance).ActiveHandler;
								if (activeHandler != null)
								{
									activeHandler.CloseContextMenu();
								}
							};
						}
						else
						{
							string tag = slot2.Tag;
							bool flag2 = LenowoTweeks.AllowGooberUnpack.Value && !string.IsNullOrWhiteSpace(tag) && tag.StartsWith("[") && tag.Contains("&");
							LocaleString val4 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Unpack", new(string, object)[1] { ("name", slot2.Name) });
							string text = "GP Unpack <size=50%>" + slot2.Name + "</size>";
							LocaleString val2 = (flag2 ? LocaleString.op_Implicit(text) : val4);
							colorX? val3 = (flag2 ? colorX.Purple : colorX.Azure);
							menu.AddRefItem<Slot>(ref val2, (Uri)null, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<Slot>>("OnUnpack"), slot2);
						}
					}
					else
					{
						LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Drive", (string)null, true, (Dictionary<string, object>)null);
						Uri drive = ProtoFlux.Drive;
						colorX? val3 = null;
						menu.AddRefItem<IWorldElement>(ref val2, drive, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<IWorldElement>>("OnCreateDrive"), grabbedReference);
						val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Source", (string)null, true, (Dictionary<string, object>)null);
						Uri source = ProtoFlux.Source;
						val3 = null;
						menu.AddRefItem<IWorldElement>(ref val2, source, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<IWorldElement>>("OnCreateSource"), grabbedReference);
					}
				}
				else
				{
					flag = false;
					Type targetType = ((SyncRef<ISyncRef>)(object)val.NodeReference).Target.TargetType;
					if (targetType.IsGenericType && targetType.IsInterface && targetType.Name.StartsWith("IVariable"))
					{
						if (IsValidStorageNode(__instance, val, (Type t) => ProtoFluxHelper.GetLocalNode(t)))
						{
							LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.CreateLocal", (string)null, true, (Dictionary<string, object>)null);
							Uri source2 = ProtoFlux.Source;
							colorX? val3 = null;
							menu.AddRefItem<ProtoFluxReferenceProxy>(ref val2, source2, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<ProtoFluxReferenceProxy>>("OnCreateLocal"), val);
						}
						if (IsValidStorageNode(__instance, val, (Type t) => ProtoFluxHelper.GetStoreNode(t)))
						{
							LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.CreateStore", (string)null, true, (Dictionary<string, object>)null);
							Uri source3 = ProtoFlux.Source;
							colorX? val3 = null;
							menu.AddRefItem<ProtoFluxReferenceProxy>(ref val2, source3, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<ProtoFluxReferenceProxy>>("OnCreateStore"), val);
						}
						if (IsValidStorageNode(__instance, val, (Type t) => ProtoFluxHelper.GetDataModelStoreNode(t)))
						{
							LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.CreateDataModelStore", (string)null, true, (Dictionary<string, object>)null);
							Uri source4 = ProtoFlux.Source;
							colorX? val3 = null;
							menu.AddRefItem<ProtoFluxReferenceProxy>(ref val2, source4, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<ProtoFluxReferenceProxy>>("OnCreateDataModelStore"), val);
						}
					}
				}
			}
			else
			{
				flag = false;
				LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Drive", (string)null, true, (Dictionary<string, object>)null);
				Uri drive2 = ProtoFlux.Drive;
				colorX? val3 = null;
				menu.AddRefItem<IWorldElement>(ref val2, drive2, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<IWorldElement>>("OnCreateDrive"), grabbedReference);
				val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Write", (string)null, true, (Dictionary<string, object>)null);
				Uri drive3 = Tool.Drive;
				val3 = null;
				menu.AddRefItem<IWorldElement>(ref val2, drive3, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<IWorldElement>>("OnCreateWrite"), grabbedReference);
			}
			if (flag)
			{
				LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Reference", (string)null, true, (Dictionary<string, object>)null);
				Uri reference = ProtoFlux.Reference;
				colorX? val3 = null;
				menu.AddRefItem<IWorldElement>(ref val2, reference, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<IWorldElement>>("OnCreateReference"), grabbedReference);
			}
		}
		else if ((object)grabbedDelegate != null && (object)grabbedDelegate != null)
		{
			Delegate obj2 = grabbedDelegate;
			LocaleString val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Proxy", (string)null, true, (Dictionary<string, object>)null);
			Uri drive4 = Tool.Drive;
			colorX? val3 = null;
			menu.AddDelegateItem<Delegate>(ref val2, drive4, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<Delegate>>("OnCreateDelegateProxy"), obj2);
		}
		else
		{
			LocaleString val2;
			colorX? val3;
			if (Engine.IsAprilFools)
			{
				bool value = Traverse.Create(__instance).Property<bool>("AreVibesActive").Value;
				string text2 = (value ? "On" : "Off");
				val2 = LocaleString.op_Implicit("Vibe Coding Mode: " + text2);
				Uri sparks = General.Sparks;
				val3 = (value ? colorX.Yellow : colorX.Gray);
				menu.AddItem(ref val2, sparks, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ToggleVibes"));
			}
			val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.NodeBrowser", (string)null, true, (Dictionary<string, object>)null);
			Uri nodePanel = Tool.NodePanel;
			val3 = null;
			menu.AddItem(ref val2, nodePanel, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("OpenNodeBrowser"));
			val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Overview", (string)null, true, (Dictionary<string, object>)null);
			val3 = new colorX(1f, 1f, 0f, 1f, (ColorProfile)1);
			menu.AddItem(ref val2, (Uri)null, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ToggleOverviewMode"));
			if (!LenowoTweeks.nohelp.Value)
			{
				Type type = ((object)targetNode)?.GetType() ?? ((SyncField<Type>)(object)___SpawnNodeType).Value;
				val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.Wiki", new(string, object)[1] { ("node", ReflectionExtensions.GetBareName(type)) });
				Uri help = Inspector.Help;
				val3 = colorX.Cyan;
				Hyperlink.AttachForWikiPage(((Component)menu.AddItem(ref val2, help, ref val3)).Slot, type);
			}
			if (targetNode != null)
			{
				val2 = LocaleHelper.AsLocaleKey("Tools.ProtoFlux.PackInPlace", (string)null, true, (Dictionary<string, object>)null);
				val3 = colorX.Purple;
				menu.AddRefItem<ProtoFluxNode>(ref val2, (Uri)null, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler<ProtoFluxNode>>("OnPackInPlace"), targetNode);
				if (LenowoTweeks.InspectNodeShortcut.Value)
				{
					val2 = LocaleString.op_Implicit("Open Inspector On Node");
					Uri inspectorPanel = Tool.InspectorPanel;
					val3 = colorX.White;
					ContextMenuItem val5 = menu.AddItem(ref val2, inspectorPanel, ref val3);
					val5.Button.LocalPressed += (ButtonEventHandler)delegate
					{
						InspectorHelper.OpenInspectorForTarget((IWorldElement)(object)targetNode, (Slot)null, false);
					};
				}
			}
			if (((SyncElementList<SyncRef<ProtoFluxNodeVisual>>)(object)____selectedNodes).Count > 0)
			{
				val2 = LocaleHelper.AsLocaleKey("General.ClearSelection", (string)null, true, (Dictionary<string, object>)null);
				val3 = colorX.Red;
				menu.AddItem(ref val2, (Uri)null, ref val3, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("OnClearSelection"));
			}
		}
		return false;
	}

	[HarmonyReversePatch(HarmonyReversePatchType.Original)]
	[HarmonyPatch(typeof(ProtoFluxTool), "IsValidStorageNode")]
	public static bool IsValidStorageNode(ProtoFluxTool instance, ProtoFluxReferenceProxy proxy, Func<Type, Type> nodeTypeFunc)
	{
		throw new NotImplementedException("It's a reverse patch, dummy!");
	}

	[HarmonyReversePatch(HarmonyReversePatchType.Original)]
	[HarmonyPatch(typeof(Tool), "GetHit")]
	public static RaycastHit? GetHit(Tool instance)
	{
		throw new NotImplementedException("HIT GUHHH");
	}
}
