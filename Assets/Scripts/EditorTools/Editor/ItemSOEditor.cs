using RooseLabs.ScriptableObjects;
using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace RooseLabs.EditorTools.Editor
{
    [CustomEditor(typeof(ItemSO))]
    public class ItemSOEditor : UnityEditor.Editor
    {
        private void SyncIconToEffect(StatusEffectSO effect, Sprite icon)
        {
            if (effect == null) return;
            var iconField = typeof(StatusEffectSO).GetField("icon", BindingFlags.NonPublic | BindingFlags.Instance);
            if (iconField != null)
            {
                iconField.SetValue(effect, icon);
                EditorUtility.SetDirty(effect);
            }
        }

        private void SetEffectFields(StatusEffectSO effect, string effectType, string effectName, Sprite icon)
        {
            if (effect == null) return;
            var typeField = typeof(StatusEffectSO).GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);
            if (typeField != null)
            {
                var enumType = typeField.FieldType;
                var value = System.Enum.Parse(enumType, effectType);
                typeField.SetValue(effect, value);
            }
            var effectNameField = typeof(StatusEffectSO).GetField("effectName", BindingFlags.NonPublic | BindingFlags.Instance);
            if (effectNameField != null) effectNameField.SetValue(effect, effectName);

            var iconField = typeof(StatusEffectSO).GetField("icon", BindingFlags.NonPublic | BindingFlags.Instance);
            if (iconField != null) iconField.SetValue(effect, icon);

            var isStackableField = typeof(StatusEffectSO).GetField("isStackable", BindingFlags.NonPublic | BindingFlags.Instance);
            if (isStackableField != null) isStackableField.SetValue(effect, true);

            var isHiddenField = typeof(StatusEffectSO).GetField("isHidden", BindingFlags.NonPublic | BindingFlags.Instance);
            if (isHiddenField != null) isHiddenField.SetValue(effect, true);

            var canBeCleansedField = typeof(StatusEffectSO).GetField("canBeCleansed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (canBeCleansedField != null) canBeCleansedField.SetValue(effect, false);
        }

        private void DrawStatusEffectFields(StatusEffectSO effect, string label)
        {
            if (effect == null) return;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            SerializedObject so = new SerializedObject(effect);
            SerializedProperty prop = so.GetIterator();
            bool enter = true;
            while (prop.NextVisible(enter))
            {
                enter = false;
                if (prop.name == "m_Script") continue;
                EditorGUILayout.PropertyField(prop, true);
            }
            so.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty prop = serializedObject.GetIterator();
            bool enterChildren = true;
            while (prop.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (prop.name == "m_Script") continue;

                if (prop.name == "icon")
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(prop, true);
                    if (GUILayout.Button("Sync", GUILayout.Width(50)))
                    {
                        ItemSO itemSO = (ItemSO)target;
                        SyncIconToEffect(itemSO.BuffEffect, itemSO.Icon);
                        SyncIconToEffect(itemSO.DebuffEffect, itemSO.Icon);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(prop, true);
                }
            }

            serializedObject.ApplyModifiedProperties();

            ItemSO itemSORef = (ItemSO)target;
            bool needsBuff = itemSORef.BuffEffect == null;
            bool needsDebuff = itemSORef.DebuffEffect == null;

            if (needsBuff || needsDebuff)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Create Effects"))
                {
                    string itemPath = AssetDatabase.GetAssetPath(itemSORef);
                    string folder = Path.GetDirectoryName(itemPath);
                    string itemFileName = Path.GetFileNameWithoutExtension(itemPath);

                    if (needsBuff)
                    {
                        var buff = CreateInstance<StatusEffectSO>();
                        SetEffectFields(buff, "Buff", itemSORef.Name, itemSORef.Icon);
                        string buffPath = Path.Combine(folder, $"{itemFileName}_Buff.asset");
                        AssetDatabase.CreateAsset(buff, buffPath);
                        SerializedObject so = new SerializedObject(itemSORef);
                        so.FindProperty("buffEffect").objectReferenceValue = buff;
                        so.ApplyModifiedProperties();
                    }
                    if (needsDebuff)
                    {
                        var debuff = CreateInstance<StatusEffectSO>();
                        SetEffectFields(debuff, "Debuff", itemSORef.Name, itemSORef.Icon);
                        string debuffPath = Path.Combine(folder, $"{itemFileName}_Debuff.asset");
                        AssetDatabase.CreateAsset(debuff, debuffPath);
                        SerializedObject so = new SerializedObject(itemSORef);
                        so.FindProperty("debuffEffect").objectReferenceValue = debuff;
                        so.ApplyModifiedProperties();
                    }
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            DrawStatusEffectFields(itemSORef.BuffEffect, "Buff Effect");
            DrawStatusEffectFields(itemSORef.DebuffEffect, "Debuff Effect");
        }
    }
}
