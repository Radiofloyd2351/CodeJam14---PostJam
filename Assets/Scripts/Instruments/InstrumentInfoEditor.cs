using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;

[CustomEditor(typeof(InstrumentInfo))]
public class InstrumentInfoEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        InstrumentInfo instrumentInfo = (InstrumentInfo)target;

        // Get all classes that are derived from AbsInstrument
        var allTypes = Assembly.GetAssembly(typeof(AbsInstrument))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbsInstrument)) && !t.IsAbstract)
            .ToList();

        // Use the typeName field to determine the current selected type
        string selectedTypeName = instrumentInfo.TypeName;
        Type selectedType = string.IsNullOrEmpty(selectedTypeName) ? null : Type.GetType(selectedTypeName);

        // Find and display the dropdown for selecting a specific subclass of AbsInstrument
        string[] typeNames = allTypes.Select(t => t.FullName).ToArray();
        int currentIndex = Array.IndexOf(typeNames, selectedType?.FullName); // Null check to prevent issues

        // Display the dropdown
        int newIndex = EditorGUILayout.Popup("Instrument Type", currentIndex, typeNames);

        // Update the typeName string if selected
        if (newIndex >= 0 && newIndex < allTypes.Count()) {
            instrumentInfo.TypeName = allTypes[newIndex].AssemblyQualifiedName; // Store the type name
        }
    }

}