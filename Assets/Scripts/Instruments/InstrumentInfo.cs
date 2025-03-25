using UnityEngine;
using System;

[CreateAssetMenu(menuName = "New Instrument")]
public class InstrumentInfo : ScriptableObject {
    private string typeName; // Store the name of the type as a string
    [SerializeField] private string instrumentName;
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private int rarity;


    public string TypeName { get { return typeName; } set { typeName = value; } }
    // Property to get the type from the string representation
    public Type Type {
        get {
            if (string.IsNullOrEmpty(typeName)) return null;
            return Type.GetType(typeName);
        }
        set {
            if (value != null) {
                typeName = value.AssemblyQualifiedName; // Save the fully qualified type name
            }
        }
    }
}
