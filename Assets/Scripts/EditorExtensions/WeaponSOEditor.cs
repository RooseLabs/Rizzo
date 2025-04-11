using RooseLabs.ScriptableObjects;
using UnityEditor;

namespace RooseLabs.EditorExtensions
{
    [CustomEditor(typeof(BaseWeaponSO), true)]
    public class WeaponSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw default fields except for dodgeResetsCombo
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true); // Skip script field
            do
            {
                if (property.name != "dodgeResetsCombo") // Use the field name as a string
                {
                    EditorGUILayout.PropertyField(property, true);
                }
            } while (property.NextVisible(false));

            // Check if attacks array has more than one element
            SerializedProperty attacksProperty = serializedObject.FindProperty("attacks"); // Use the field name as a string
            if (attacksProperty != null && attacksProperty.arraySize > 1)
            {
                SerializedProperty dodgeResetsComboProperty = serializedObject.FindProperty("dodgeResetsCombo");
                if (dodgeResetsComboProperty != null)
                {
                    EditorGUILayout.PropertyField(dodgeResetsComboProperty);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
