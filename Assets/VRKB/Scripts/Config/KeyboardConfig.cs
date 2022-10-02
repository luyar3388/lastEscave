/// \file
/// <summary>
/// Simple query interface for the JSON config file.
/// </summary>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VRKB
{
    public class KeyboardConfig
    {
        protected JToken _json;
        protected ColorDictionary _colorDict;

        public KeyboardConfig(string json)
        {
            _json = JToken.Parse(json);
            _colorDict = new ColorDictionary(_json["colors"]);
        }

        public string DefaultLayerName()
        {
            var defaultLayerName = _json["defaultLayerName"];
            return (defaultLayerName != null)
                ? defaultLayerName.Value<string>() : "defaultLayer";
        }

        public KeyConfig GetKeyConfig(string layerName, string keyName)
        {
            KeyConfig keyConfig = null;

            var layer = _json[layerName];
            if (layer == null) {
                Debug.LogFormat("VRKB: layer not found: '{0}'", layerName);
                return null;
            }

            var inheritsFromLayer = layer["inheritsFromLayer"];
            if (inheritsFromLayer != null)
                keyConfig = GetKeyConfig(inheritsFromLayer.Value<string>(), keyName);

            if (keyConfig == null)
                keyConfig = new KeyConfig(_json["keyDefaults"], _colorDict);

            keyConfig.Apply(layer[keyName], _colorDict);

            return keyConfig;
        }

    }
}
