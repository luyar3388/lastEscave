/// \file
/// <summary>
/// Stores callbacks to invoke every time the user presses a key.
/// </summary>

using UnityEngine;
using UnityEngine.Events;

namespace VRKB
{
    [System.Serializable]
    public class OnKeyPressEvent : UnityEvent<KeyBehaviour, Collider, bool> {}
}
