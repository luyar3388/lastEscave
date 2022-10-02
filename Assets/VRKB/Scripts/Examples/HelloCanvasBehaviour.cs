/// \file
/// <summary>
/// Canvas that displays a greeting message, which is used in
/// Oculus/SteamVR example scenes.
/// </summary>

using TMPro;
using UnityEngine;

public class HelloCanvasBehaviour : MonoBehaviour
{
    public void SetName(string name)
    {
        if (name == null || name.Length == 0)
            name = "<blank>";

        TextMeshProUGUI textField = GetComponentInChildren<TextMeshProUGUI>();
        textField.text = string.Format("Hello, {0}!", name);
    }
}
