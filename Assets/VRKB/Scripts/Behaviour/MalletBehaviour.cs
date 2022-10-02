/// \file
/// <summary>
/// Implement configurable handle length and head radius for the mallets.
/// </summary>
using UnityEditor;
using UnityEngine;

namespace VRKB
{
// See https://docs.unity3d.com/Manual/UpgradeGuide20183.html
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    public class MalletBehaviour : MonoBehaviour
    {
        public float HandleLength = 0.2f;
        public float HandleRadius = 0.01f;
        public float HeadRadius = 0.05f;

        public void OnEnable()
        {
            UpdateGeom();
        }

        public void Update()
        {
            UpdateGeom();
        }

        public void UpdateGeom()
        {
            CapsuleCollider handle = GetComponentInChildren<CapsuleCollider>();

#if UNITY_EDITOR
            Undo.RecordObject(handle.transform, "update VRKB mallet dimensions");
#endif

            handle.transform.localScale = new Vector3(HandleRadius, HandleLength / 2.0f, HandleRadius);
            handle.transform.localPosition = new Vector3(0, 0, HandleLength / 2.0f);
            handle.transform.localRotation = Quaternion.Euler(90, 0, 0);

#if UNITY_EDITOR
            PrefabUtility.RecordPrefabInstancePropertyModifications(handle.transform);
#endif

            SphereCollider head = GetComponentInChildren<SphereCollider>();

#if UNITY_EDITOR
            Undo.RecordObject(head.transform, "update VRKB mallet dimensions");
#endif

            head.transform.localPosition = new Vector3(0, 0, HandleLength);
            head.transform.localScale = Vector3.one * HeadRadius;

#if UNITY_EDITOR
            PrefabUtility.RecordPrefabInstancePropertyModifications(head.transform);
#endif
        }

    }
}
