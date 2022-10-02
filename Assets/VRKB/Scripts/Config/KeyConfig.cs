/// \file
/// <summary>
/// Represents settings for a single key (e.g. "output", "label").
/// </summary>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRKB
{
    [System.Serializable]
    public class KeyConfig
    {
        public Action Action;
        public string[] Labels;
        public Color[] LabelColors;
        public float[] FontSizes;
        public string[] Images;
        public Color[] ImageColors;

        public KeyConfig()
        {
            Labels = null;
            Images = null;
            Action = new Action(ActionType.Output, "");
        }

        public KeyConfig(KeyConfig other)
        {
            Action = other.Action;

            if (other.Labels != null)
                Labels = (string[])other.Labels.Clone();
            else
                Labels = null;

            if (other.LabelColors != null)
                LabelColors = (Color[])other.LabelColors.Clone();
            else
                LabelColors = null;

            if (other.FontSizes != null)
                FontSizes = (float[])other.FontSizes.Clone();
            else
                FontSizes = null;

            if (other.Images != null)
                Images = (string[])other.Images.Clone();
            else
                Images = null;

            if (other.ImageColors != null)
                ImageColors = (Color[])other.ImageColors.Clone();
            else
                ImageColors = null;
        }

        public void Apply(JToken keyConfig, ColorDictionary colorDict)
        {
            if (keyConfig == null)
                return;

            var output = keyConfig["output"];
            if (output != null)
                Action = new Action(ActionType.Output, output.Value<string>());

            var enableLayerForNextKey = keyConfig["enableLayerForNextKey"];
            if (enableLayerForNextKey != null) {
                Action = new Action(
                    ActionType.EnableLayerForNextKey,
                    enableLayerForNextKey.Value<string>());
            }

            var enableLayer = keyConfig["enableLayer"];
            if (enableLayer != null) {
                Action = new Action(
                    ActionType.EnableLayer,
                    enableLayer.Value<string>());
            }

            var action = keyConfig["action"];
            if (action != null) {
                string actionStr = action.Value<string>();
                if (actionStr.ToLower() == "confirm") {
                    Action = new Action(ActionType.Confirm, null);
                } else if (actionStr.ToLower() == "cancel") {
                    Action = new Action(ActionType.Cancel, null);
                } else {
                    Debug.LogFormat("VRKB: unrecognized value for 'action': '{0}'", actionStr);
                    Debug.Log("VRKB: valid values for 'action' are: 'cancel', 'confirm'");
                }
            }

            var label = keyConfig["label"];
            if (label != null)
                Labels = new string[] { label.Value<string>() };

            var labels = keyConfig["labels"];
            if (labels != null)
                Labels = labels.Values<string>().ToArray();

            var labelColor = keyConfig["labelColor"];
            if (labelColor != null)
                LabelColors = new Color[] { colorDict[labelColor.Value<string>()] };

            var labelColors = keyConfig["labelColors"];
            if (labelColors != null)
                LabelColors = labelColor.Values<string>()
                    .Select(colorString => colorDict[colorString]).ToArray();

            var fontSize = keyConfig["fontSize"];
            if (fontSize != null)
                FontSizes = new float[] { fontSize.Value<float>() };

            var fontSizes = keyConfig["fontSizes"];
            if (fontSizes != null)
                FontSizes = fontSize.Values<float>().ToArray();

            var image = keyConfig["image"];
            if (image != null)
                Images = new string[] { image.Value<string>() };

            var images = keyConfig["images"];
            if (labels != null)
                Images = images.Values<string>().ToArray();

            var imageColor = keyConfig["imageColor"];
            if (imageColor != null)
                ImageColors = new Color[] { colorDict[imageColor.Value<string>()] };

            var imageColors = keyConfig["imageColors"];
            if (imageColors != null)
                ImageColors = imageColor.Values<string>()
                    .Select(colorString => colorDict[colorString]).ToArray();
        }

        public KeyConfig(JToken keyConfig, ColorDictionary colorDict) : this()
        {
            Apply(keyConfig, colorDict);
        }

        public KeyConfig(JToken keyConfig)
        {
            Apply(keyConfig, null);
        }
    }
}
