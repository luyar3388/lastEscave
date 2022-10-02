/// \file
/// <summary>
/// Stores callbacks to invoke when the user presses the "confirm" key (e.g. green checkmark key).
/// </summary>

using UnityEngine;
using UnityEngine.Events;

namespace VRKB
{
    [System.Serializable]
    public class OnConfirmEvent : UnityEvent<string> {}
}
