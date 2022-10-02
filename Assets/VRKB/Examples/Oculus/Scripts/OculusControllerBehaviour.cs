using System.Collections.Generic;
using UnityEngine;
using OVR;

namespace VRKB
{
    public class OculusControllerBehaviour : MonoBehaviour
    {
        public OVRInput.Controller Controller;
        protected bool _vibrating = false;
        protected float _vibrationDuration = 0.05f;
        protected float _vibrationStartTime;

        public void Vibrate()
        {
            if (Controller != OVRInput.Controller.LTouch
                && Controller != OVRInput.Controller.RTouch)
                    return;

            _vibrating = true;
            _vibrationStartTime = Time.time;
            OVRInput.SetControllerVibration(0.01f, 0.4f, Controller);
        }

        public void Update()
        {
            if (_vibrating && Time.time - _vibrationStartTime > _vibrationDuration) {
                _vibrating = false;
                OVRInput.SetControllerVibration(0.0f, 0.0f, Controller);
            }
        }
    }
}