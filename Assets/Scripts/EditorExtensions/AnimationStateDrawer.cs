using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RooseLabs.Models;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace RooseLabs.EditorExtensions
{
    [CustomPropertyDrawer(typeof(AnimationStateData))]
    public class AnimationStateDrawer : PropertyDrawer
    {
        private List<AnimatorState> m_animationStates = new();
        private AnimatorController m_cachedAnimatorController;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Find the AnimatorController from the serialized object
            AnimatorController animatorController = FindAnimatorController(property);
            if (animatorController != null)
            {
                if (m_cachedAnimatorController != animatorController)
                {
                    m_cachedAnimatorController = animatorController;
                    LoadAnimationStates(animatorController);
                }

                SerializedProperty nameProperty = property.FindPropertyRelative(nameof(AnimationStateData.name));
                SerializedProperty hashProperty = property.FindPropertyRelative(nameof(AnimationStateData.hash));
                SerializedProperty lengthProperty = property.FindPropertyRelative(nameof(AnimationStateData.length));

                string[] stateNames = m_animationStates.Select(state => state.name).ToArray();
                int selectedIndex =
                    m_animationStates.FindIndex(state => Animator.StringToHash(state.name) == hashProperty.intValue);
                if (selectedIndex == -1) selectedIndex = 0;

                // Dropdown for selecting animation state
                selectedIndex =
                    EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                        label.text, selectedIndex, stateNames);

                // Update properties based on selected state
                AnimatorState selectedState = m_animationStates[selectedIndex];
                nameProperty.stringValue = selectedState.name;
                hashProperty.intValue = Animator.StringToHash(selectedState.name);
                lengthProperty.floatValue = selectedState.motion != null ? selectedState.motion.averageDuration : 0f;

                // Display the length as subtext
                string lengthText = $"Length: {lengthProperty.floatValue:F2} seconds";
                EditorGUI.LabelField(
                    new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width,
                        EditorGUIUtility.singleLineHeight), lengthText, EditorStyles.miniLabel);
            }
            else
            {
                EditorGUI.HelpBox(position, $"AnimatorController is required to edit {label.text}",
                    MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Add extra height for the subtext
            return EditorGUIUtility.singleLineHeight * 2 + 2;
        }

        private AnimatorController FindAnimatorController(SerializedProperty property)
        {
            Object targetObject = property.serializedObject.targetObject;
            var targetType = targetObject.GetType();

            // Search for any field of type AnimatorController
            var field = targetType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(f => f.FieldType == typeof(AnimatorController));

            return field?.GetValue(targetObject) as AnimatorController;
        }

        private void LoadAnimationStates(AnimatorController animatorController)
        {
            m_animationStates = animatorController.layers
                .SelectMany(layer => GetAllStates(layer.stateMachine))
                .OrderBy(state => state.name)
                .ToList();
        }

        private IEnumerable<AnimatorState> GetAllStates(AnimatorStateMachine stateMachine)
        {
            // Get states from the current state machine
            var states = stateMachine.states.Select(state => state.state);

            // Recursively get states from child state machines
            var subStates = stateMachine.stateMachines
                .SelectMany(subStateMachine => GetAllStates(subStateMachine.stateMachine));

            return states.Concat(subStates);
        }
    }
}
