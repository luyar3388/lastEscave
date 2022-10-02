/// \file
/// <summary>
/// Custom Unity Inspector panel for "RoundedRect".
/// </summary>

using UnityEditor;
using UnityEngine;

namespace VRKB
{
    [CustomEditor(typeof(RoundedRect)), CanEditMultipleObjects]
    public class RoundedCuboidInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Size"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CornerRadius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("VerticesPerCorner"));
            if (serializedObject.ApplyModifiedProperties()) {
                foreach (RoundedRect rect in targets) {

                    // Old code which worked in Unity 2018.3, but did not work
                    // in Unity 5.5.0f3 because `rect.gameObject.scene`
                    // is a value type in Unity 5.5.0f3.
#if comment
                    bool isPrefab = rect.gameObject.scene == null;
                    if (!isPrefab)
                        rect.UpdateMesh();
#endif

                    // Note 1: This part is tricky. The goal of this code is to
                    // have changes to RoundedRect properties (e.g. corner radius)
                    // instantly reflected in the mesh shown in the Editor view,
                    // in both Scene Mode and Prefab Mode.
                    //
                    // Note 2: If we try to update `rect.mesh` while in Prefab Mode, we will
                    // get warnings about leaking memory and be told to use `rect.sharedMesh`
                    // instead. But we want the dimensions of each RoundedRect to be independently
                    // controllable, so using a sharedMesh is not right.  For prefabs, I instead
                    // update the meshes in `OnEnable()`, which gets called every time an inspector
                    // property is changed.
                    //
                    // Note 3: See the "Star" tutorial by Catlike Coding, for an example
                    // of how to implement a live preview of a procedural mesh in the Editor:
                    // https://catlikecoding.com/unity/tutorials/editor/star/
                    //
                    // Note 4: In Unity 2018.3, `PrefabUtility.GetPrefabType()` was split into
                    // `PrefabUtility.GetPrefabAssetType()` and `PrefabUtility.GetPrefabInstanceStatus()`.
                    // See: https://forum.unity.com/threads/test-if-prefab-is-an-instance.562900/#post-3735301
#if UNITY_2018_3_OR_NEWER
                    PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(rect.gameObject);
                    if (prefabType == PrefabAssetType.NotAPrefab)
                        rect.UpdateMesh();
#else
                    PrefabType prefabType = PrefabUtility.GetPrefabType(rect.gameObject);
                    switch(prefabType) {
                        case PrefabType.None:
                        case PrefabType.PrefabInstance:
                        case PrefabType.DisconnectedPrefabInstance:
                            rect.UpdateMesh();
                            break;
                        default:
                            break;
                    }
#endif

                }
            }
        }
    }
}
