# Compatibility with Better Info Cards

Better Info Cards is an inherently complex mod that messes with many internals, making compatibility frequently difficult.  Particularily, `SelectToolHoverTextCard.UpdateHoverElements` is a massive method that multiple mods already alter.  This file details my approach to transpiling this method, and explains how you can add compatibility with BIC to your mod.

## Compatibility Quick Guide:

* The first `.GetComponent<PrimaryElement>` is what I target to extract the `KSelectable` from the method.  Do *NOT* remove this, add another before it, etc.  If the `KSelectable` is not properly exported the entire mod will break.
* Most text entries are handled through status items.  Those that aren't need their type exported so that BIC knows how to handle them.
	* If this doesn't occur, the mod will still function, however the entry will be treated as raw text, so card grouping and data processing will not work.
	* As a general rule of thumb, as long as you're not removing/replacing code, these shouldn't break.
* If you want BIC to know how to handle your info, you need to:
	* Export the type and data of the entry.  This is only necessary if you've transpiled additional entries in (not for status items).
	* Add a dictionary entry mapping how to process the data of your type.
	
## My Transpiler

Due to the number of changes I must make, and the general complexity of this method, my transpiler is complicated, my apologies.  If you need help making your transpiler compatible with mine, please reach out and I will be happy to help.  If you have suggestions for me I would love to hear them.

The following is a list of targets that I use in my transpiler.  For compatibility avoid removing/replacing these:

* The first `.GetComponent<PrimaryElement>` - Will crash if removed.
	* After that target:
	* Locate `titleLocal` variable using `GameUtil.GetUnitFormattedName`
	* Locate `germLocal` variable using `string.Format`
	* `HoverTextDrawer.DrawText`
		* At each call, searches backwards for the last string pushed onto the stack, and checks the operand:
		* `titleLocal`
		* `germLocal`
		* `StatusItemGroup.Entry.GetName`
		* `GameUtil.GetFormattedTemperature`
	* `WorldInspector.MassStringsReadOnly`
	
## Exporting Your Entry

If you transpile to add additional entries to info cards, you will need to provide BIC with more info so that it knows how to process your data.  If you do not do this nothing will crash, but the entry will be treated as raw text, which prevents card grouping and data processing from working well.

To do this, you must export a unique string `name`, describing the type of your entry, and an object `data` to be used in processing.  For example, if your mod showed the heat capacity of an element, the name might be `MyNamespaceHeatCapacity`, and the data could be the `GameObject` of the selectable, or a specific `Component`.

To accomplish this, you must push a string, and then an object onto the stack, and then call `GetSelectInfo_Patch.Export`.  This should be done right after the `DrawText` call you insert.  

Since mods cannot safely be used as references, this method must be called through reflection, and should handle BIC not being present.  Additionally, to be load order agnostic, you reference BIC after all mods have loaded.  This is usually done by manually patching later in the load process.

```
if(AccessTools.Method("GetSelectInfo_Patch:ExportGO") is MethodInfo export)
{
	yield return new CodeInstruction(OpCodes.Ldstr, "MyNamespaceHeatCapacity"); // Push your string onto the stack.
	yield return new CodeInstruction(); // Push your object onto the stack however you want.
	yield return new CodeInstruction(OpCodes.Call, export); // Call the export method.
}
```

If the `data` you desire is just the `KSelectable.GameObject`, you may use `GetSelectInfo_Patch.ExportGO` and omit pushing an object onto the stack.

## Defining the Data Converter

Knowing the type of your entry is worthless without knowing how to *use* that type.  To tell BIC how to use it, you must provide a Data Converter for it.

This consists of four things:

* `string name`, which should match the name you export, or the name of the status item you've created.
* `Func<object, T> getValue` which takes in your `data`, and processes it into the result.
* `Func<string, List<T>, string> getTextOverride` which takes in the original text string displayed and a list of your results, and outputs the formatted text string to use.
* `List<(Func<T, float>, float)> splitListDefs` an optional parameter that takes a list of splitters.  Each splitter is operated on sequentially to split the list of info cards into a smaller list.
	* `Func<T, float>` defines a function that process your result into the value to use for splitting.
	* `float` defines the band size to split with.
	
Adding a Data Converter should be done by invoking `ConverterManager.AddConverterReflect` through reflection and providing the above parameters.

```
if (AccessTools.Method("ConverterManager:AddConverterReflect") is MethodInfo addConverter)
{
	addConverter.Invoke(null, new object[] {
		oreMass,
		new Func<object, float>((data) => ((GameObject)data).GetComponent<PrimaryElement>().Mass),
		new Func<string, List<float>, string>((original, masses) => oreMass.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + ConverterManager.sumSuffix),
		null });
	}
```			