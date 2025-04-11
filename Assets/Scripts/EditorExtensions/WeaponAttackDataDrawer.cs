using RooseLabs.Models;
using UnityEditor;
using UnityEngine;

namespace RooseLabs.EditorExtensions
{
    [CustomPropertyDrawer(typeof(WeaponAttackData))]
    public class WeaponAttackDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the index of the attack
            int index = GetArrayIndex(property);

            // Retrieve the animation state name
            SerializedProperty animationStateProperty = property.FindPropertyRelative("animationState");
            SerializedProperty animationStateNameProperty = animationStateProperty.FindPropertyRelative("name");
            string animationStateName = animationStateNameProperty != null ? animationStateNameProperty.stringValue : "Unknown";

            // Update the label to include the animation state name
            label.text = $"Attack {index + 1} ({animationStateName})";

            // Draw the default property field
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Use Unity's default height calculation for the property
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private int GetArrayIndex(SerializedProperty property)
        {
            string path = property.propertyPath;
            int startIndex = path.LastIndexOf('[') + 1;
            int endIndex = path.LastIndexOf(']');
            if (startIndex > 0 && endIndex > startIndex)
            {
                string indexString = path.Substring(startIndex, endIndex - startIndex);
                if (int.TryParse(indexString, out int index))
                {
                    return index;
                }
            }
            return 0;
        }
    }
}
