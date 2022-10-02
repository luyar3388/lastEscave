/// \file
/// <summary>
/// Custom Unity Inspector panel for "KeyboardBehaviour".
/// </summary>

using UnityEngine;
using UnityEditor;

namespace VRKB
{
    [CustomEditor(typeof(KeyboardBehaviour)), CanEditMultipleObjects]
    public class KeyboardBehaviourEditor : Editor
    {
        public bool EventsFoldoutOpen = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            KeyboardBehaviour keyboard = (KeyboardBehaviour)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PlaceholderText"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RepeatDelayInMilliseconds"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RepeatRatePerSecond"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowSimultaneousKeyPresses"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PasswordMode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UnpressedKeyMaterial"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PressedKeyMaterial"));

            EventsFoldoutOpen = EditorGUILayout.Foldout(EventsFoldoutOpen, "Event Callbacks");
            if (EventsFoldoutOpen)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnKeyPress"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnCancel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnConfirm"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("KeyboardConfigFile"));

            if (GUILayout.Button("Reload Keyboard Config File"))
                keyboard.LoadConfig();

            // Auto-assign names of the form ("key99")
            // to children with the "KeyBehaviour" property.
            // This button is disabled because it is only useful
            // for people who are designing custom 3D layouts
            // for their keyboard.

#if comment
            if (GUILayout.Button("Autogenerate Key Names"))
                keyboard.AutogenerateKeyNames();
#endif

            serializedObject.ApplyModifiedProperties();
        }
    }
}
