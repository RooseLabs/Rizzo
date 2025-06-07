using System;
using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEditor;
using System.Linq;
using UnityEngine;

namespace RooseLabs.EditorTools.Editor
{
    [CustomEditor(typeof(StatusEffectSO), true)]
    public class StatusEffectSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Change Status Effect Type..."))
            {
                var baseType = typeof(StatusEffectSO);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => baseType.IsAssignableFrom(t) && t != baseType && !t.IsAbstract)
                    .ToArray();

                // Use the effectName property if available, otherwise fallback to type name
                string effectName = serializedObject.FindProperty("effectName").stringValue;
                if (string.IsNullOrEmpty(effectName))
                    effectName = target.GetType().Name;
                effectName = effectName.Replace(" ", string.Empty);

                // Order types by name likeness to effectName
                Func<string, string, int> likeness = (a, b) => {
                    int score = 0, ai = 0, bi = 0;
                    while (ai < a.Length && bi < b.Length)
                    {
                        if (char.ToLower(a[ai]) == char.ToLower(b[bi]))
                        {
                            score++;
                            ai++;
                            bi++;
                        }
                        else
                        {
                            bi++;
                        }
                    }
                    return score;
                };
                var ordered = types
                    .Select(t => new { Type = t, Name = t.Name, Score = likeness(effectName, t.Name.Replace(" ", string.Empty).Replace("_", string.Empty)) })
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.Name)
                    .ToArray();
                var orderedTypes = ordered.Select(x => x.Type).ToArray();
                var typeNames = ordered.Select(x => x.Name).ToArray();
                int currentTypeIndex = Array.IndexOf(orderedTypes, target.GetType());

                StatusEffectTypePickerWindow.Open(typeNames, currentTypeIndex, (selectedTypeIndex) =>
                {
                    if (selectedTypeIndex != currentTypeIndex)
                    {
                        if (EditorUtility.DisplayDialog("Change Status Effect Type", $"Change type to {typeNames[selectedTypeIndex]}? This will forcefully change the script reference and may lose data not shared between types.", "Yes", "No"))
                        {
                            var scriptProp = serializedObject.FindProperty("m_Script");
                            var monoScript = MonoScript.FromScriptableObject(CreateInstance(orderedTypes[selectedTypeIndex]));
                            scriptProp.objectReferenceValue = monoScript;
                            serializedObject.ApplyModifiedProperties();
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                });
            }

            EditorGUILayout.Space();
            // Show StatusEffectType as a disabled EnumPopup for read-only effect
            if (target is StatusEffectSO effect)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.EnumPopup("Type", effect.Type);
                EditorGUI.EndDisabledGroup();
            }
            base.OnInspectorGUI();
        }

        private class StatusEffectTypePickerWindow : EditorWindow
        {
            private string[] m_typeNames;
            private int m_currentTypeIndex;
            private Action<int> m_onTypeSelected;
            private Vector2 m_scroll;

            public static void Open(string[] typeNames, int currentTypeIndex, Action<int> onTypeSelected)
            {
                var window = CreateInstance<StatusEffectTypePickerWindow>();
                window.m_typeNames = typeNames;
                window.m_currentTypeIndex = currentTypeIndex;
                window.m_onTypeSelected = onTypeSelected;
                window.titleContent = new GUIContent("Select Status Effect Type");
                window.ShowUtility();
            }

            private void OnGUI()
            {
                EditorGUILayout.LabelField("Select a Status Effect Type:", EditorStyles.boldLabel);
                m_scroll = EditorGUILayout.BeginScrollView(m_scroll);
                for (int i = 0; i < m_typeNames.Length; i++)
                {
                    GUI.enabled = i != m_currentTypeIndex;
                    if (GUILayout.Button(m_typeNames[i]))
                    {
                        m_onTypeSelected?.Invoke(i);
                        Close();
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndScrollView();
                if (GUILayout.Button("Cancel"))
                {
                    Close();
                }
            }
        }
    }
}
