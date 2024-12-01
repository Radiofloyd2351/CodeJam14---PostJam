using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
public class SerializableDictDrawer : PropertyDrawer {
    private bool isFolded = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the foldout
        isFolded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            isFolded,
            label
        );

        if (isFolded) {
            // Find keys and values
            SerializedProperty keysProp = property.FindPropertyRelative("keys");
            SerializedProperty valuesProp = property.FindPropertyRelative("values");
            SerializedProperty newKeyNewProp = property.FindPropertyRelative("newKey");

            if (keysProp == null || valuesProp == null) {
                EditorGUI.LabelField(position, "Invalid SerializedDictionary.");
                EditorGUI.EndProperty();
                return;
            }

            // Draw each key-value pair
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            Rect fieldPosition = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);


            GUIStyle removeButtonStyle = new GUIStyle(GUI.skin.button);
            removeButtonStyle.normal.background = MakeRedTexture();
            removeButtonStyle.normal.textColor = Color.white; // Change text color to white for contrast
            removeButtonStyle.alignment = TextAnchor.MiddleCenter; // Center the text in the button

            GUIStyle addButtonStyle = new GUIStyle(GUI.skin.button);
            addButtonStyle.normal.background = MakeGreenTexture();
            addButtonStyle.normal.textColor = Color.white; // Change text color to white for contrast
            addButtonStyle.alignment = TextAnchor.MiddleCenter; // Center the text in the button


            for (int i = 0; i < keysProp.arraySize; i++) {
                Rect keyRect = new Rect(fieldPosition.x, fieldPosition.y + (lineHeight + spacing) * (2 + i), fieldPosition.width * 0.4f, lineHeight);
                Rect valueRect = new Rect(fieldPosition.x + fieldPosition.width * 0.45f, fieldPosition.y + (lineHeight + spacing) * (2+i), fieldPosition.width * 0.4f, lineHeight);
                Rect removeButtonRect = new Rect(fieldPosition.x + fieldPosition.width * 0.85f, fieldPosition.y + (lineHeight + spacing) * (2+i), fieldPosition.width * 0.15f, lineHeight);

                SerializedProperty keyProp = keysProp.GetArrayElementAtIndex(i);
                SerializedProperty valueProp = valuesProp.GetArrayElementAtIndex(i);

                if (keyProp != null) {
                    EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
                }

                if (valueProp != null) {
                    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
                }

                // Draw the "X" Remove button for each key-value pair
                if (GUI.Button(removeButtonRect, "X", removeButtonStyle)) {
                    keysProp.DeleteArrayElementAtIndex(i);
                    valuesProp.DeleteArrayElementAtIndex(i);
                    // Ensure that the array is refreshed and dirty
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }

            Rect newKeyFieldRect = new Rect(position.x, fieldPosition.y, fieldPosition.width , lineHeight);

            EditorGUI.PropertyField(newKeyFieldRect, newKeyNewProp, GUIContent.none);

            // Add button for adding new elements
            Rect addButtonRect = new Rect(position.x, fieldPosition.y + (lineHeight + spacing)/* + (lineHeight + spacing) * keysProp.arraySize*/, position.width, lineHeight);
            if (GUI.Button(addButtonRect, "Add Element", addButtonStyle)) {
                // Add key and value
                keysProp.InsertArrayElementAtIndex(keysProp.arraySize);
                valuesProp.InsertArrayElementAtIndex(valuesProp.arraySize);

                // Initialize the new key-value pair
                SerializedProperty newKeyProp = keysProp.GetArrayElementAtIndex(keysProp.arraySize - 1);
                SerializedProperty newValueProp = valuesProp.GetArrayElementAtIndex(valuesProp.arraySize - 1);

                // Set default values based on their types
                if (newKeyProp.propertyType == SerializedPropertyType.Integer) {
                    newKeyProp = newKeyNewProp; // Set a default value for int keys
                }

                if (newKeyProp.propertyType == SerializedPropertyType.String) {
                    newKeyProp.stringValue = newKeyNewProp.stringValue; // Set a default value for GameObject
                }

                if (newKeyProp.propertyType == SerializedPropertyType.Color) {
                    newKeyProp.colorValue = new Color32(0, 0, 0, 0); // Set a default value for GameObject
                }

                // Mark the serialized object as dirty to ensure changes are saved
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            // Optionally, add a "Remove All" button if needed
            Rect removeAllButtonRect = new Rect(position.x, fieldPosition.y + 2*(lineHeight + spacing) + (lineHeight + spacing) * keysProp.arraySize, position.width, lineHeight);
            if (GUI.Button(removeAllButtonRect, "Remove All", removeButtonStyle)) {
                if (keysProp.arraySize > 0) {
                    keysProp.ClearArray();
                    valuesProp.ClearArray();
                    // Mark as dirty
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!isFolded) return EditorGUIUtility.singleLineHeight;

        SerializedProperty keysProp = property.FindPropertyRelative("keys");
        if (keysProp == null) return EditorGUIUtility.singleLineHeight;

        // Add height for each dictionary element + buttons
        return (keysProp.arraySize + 4) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
    }


    private Texture2D MakeRedTexture() {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color32(200, 100, 100, 255));
        texture.Apply();
        return texture;
    }
    private Texture2D MakeGreenTexture() {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color32(100, 150, 100, 255));
        texture.Apply();
        return texture;
    }
}