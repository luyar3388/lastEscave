/// \file
/// <summary>
/// Maps color names to corresponding Unity Color objects.
/// </summary>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace VRKB
{
    public class ColorDictionary
    {
        protected Dictionary<string, Color> _dict;

        public ColorDictionary()
        {
            _dict = new Dictionary<string, Color>();
            _dict.Add("black", Color.black);
            _dict.Add("blue", Color.blue);
            _dict.Add("clear", Color.clear);
            _dict.Add("cyan", Color.cyan);
            _dict.Add("gray", Color.gray);
            _dict.Add("green", Color.green);
            _dict.Add("grey", Color.grey);
            _dict.Add("magenta", Color.magenta);
            _dict.Add("red", Color.red);
            _dict.Add("white", Color.white);
            _dict.Add("yellow", Color.yellow);
        }

        public ColorDictionary(JToken jsonDict) : this()
        {
            Apply(jsonDict);
        }

        public void Apply(JToken jsonDict)
        {
            if (jsonDict == null)
                return;

            var colors = jsonDict["colors"];
            if (colors != null) {
                foreach (var colorEntry in (JObject)colors) {
                    string colorName = colorEntry.Key;
                    string colorString = colorEntry.Value.Value<string>();
                    Color color;
                    if (ColorUtility.TryParseHtmlString(colorString, out color))
                        _dict[colorName] = color;
                    else
                        Debug.LogFormat("failed to parse color string: '{0}'", colorString);
                }
            }
        }

        public Color this[string colorNameOrHtmlString]
        {
            get {
                Color color;
                if (ColorUtility.TryParseHtmlString(colorNameOrHtmlString, out color))
                    return color;
                else if (_dict.TryGetValue(colorNameOrHtmlString, out color))
                    return color;
                else
                    throw new KeyNotFoundException();
            }
            set {
                _dict[colorNameOrHtmlString] = value;
            }
        }
    }
}
