/// \file
/// <summary>
/// Positions child game objects at equal distances along a chosen axis (X, Y, or Z).
/// </summary>

using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace VRKB
{
// See https://docs.unity3d.com/Manual/UpgradeGuide20183.html
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    public class LinearLayoutBehaviour : MonoBehaviour
    {
        public enum Axis { X, Y, Z };

        public float Spacing;
        public Axis AlignmentAxis;

        public void OnEnable()
        {
            Realign();
        }

        public void OnValidate()
        {
            Realign();
        }

        public void OnTransformChildrenChanged()
        {
            Realign();
        }

        public void Update()
        {
            Realign();
        }

        public void Realign()
        {
            if (transform.childCount <= 0)
                return;

            Vector3 childPosition = Vector3.zero;
            float halfWidth = Spacing * (transform.childCount - 1) / 2.0f;
            switch(AlignmentAxis) {
                case Axis.X: childPosition.x -= halfWidth; break;
                case Axis.Y: childPosition.y -= halfWidth; break;
                case Axis.Z: childPosition.z -= halfWidth; break;
                default: Assert.IsTrue(false); break;
            }

            foreach(Transform child in transform) {
                if (!child.gameObject.activeInHierarchy)
                    continue;
                child.transform.localPosition = childPosition;
                switch(AlignmentAxis) {
                    case Axis.X: childPosition.x += Spacing; break;
                    case Axis.Y: childPosition.y += Spacing; break;
                    case Axis.Z: childPosition.z += Spacing; break;
                    default: Assert.IsTrue(false); break;
                }
            }
        }
    }
}
