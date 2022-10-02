/// \file
/// <summary>
/// Represents the type of action to perform on key press (e.g.
/// "output", "cancel", "confirm"), and an associated argument
/// for that action (if any).
/// </summary>

namespace VRKB
{
    public enum ActionType {
        None,
        Output,
        EnableLayer,
        EnableLayerForNextKey,
        Cancel,
        Confirm
    };

    [System.Serializable]
    public struct Action
    {
        public ActionType Type;
        public string Arg;

        public Action(ActionType type)
        {
            Type = type;
            Arg = null;
        }

        public Action(ActionType type, string arg)
        {
            Type = type;
            Arg = arg;
        }
    }
}
