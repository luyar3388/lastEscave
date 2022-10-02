/// \file
/// <summary>
/// Stores callbacks to invoke when the user presses the "cancel" key (e.g. red "X" button).
/// </summary>

using UnityEngine;
using UnityEngine.Events;

namespace VRKB
{
    [System.Serializable]
    public class OnCancelEvent : UnityEvent<string> {}
}
