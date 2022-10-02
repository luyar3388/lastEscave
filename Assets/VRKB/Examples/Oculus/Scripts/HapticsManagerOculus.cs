using System.Collections.Generic;
using UnityEngine;
using OVR;

namespace VRKB
{
    public class HapticsManagerOculus : MonoBehaviour
    {
        public void OnKeyPress(KeyBehaviour key, Collider collider, bool autoRepeat)
        {
            OculusControllerBehaviour controller = collider.GetComponentInParent<OculusControllerBehaviour>();
            if (controller == null)
                return;

            controller.Vibrate();
        }
    }
}