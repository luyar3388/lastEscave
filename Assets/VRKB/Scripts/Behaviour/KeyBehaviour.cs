/// \file
/// <summary>
/// Manages settings for a single key. Detects collisions
/// with mallets and calls KeyPress method on parent keyboard
/// (KeyboardBehaviour).
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VRKB
{
    [RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
    public class KeyBehaviour : MonoBehaviour
    {
        protected bool _pressed = false;
        protected float _pressStartTime;
        protected bool _repeating = false;
        protected float _prevRepeatTime;

        [SerializeField]
        protected KeyConfig _config;
        public KeyConfig Config
        {
            get {
                return _config;
            }
            set {
                _config = value;
                ApplyConfig(_config);
            }
        }

        protected Collider _collider;
        protected KeyboardBehaviour _keyboard;
        protected MeshRenderer _renderer;

        public void Start()
        {
            _collider = GetComponent<Collider>();
            _keyboard = GetComponentInParent<KeyboardBehaviour>();
            _renderer = GetComponentInParent<MeshRenderer>();

            _renderer.sharedMaterial = _keyboard.UnpressedKeyMaterial;

            ApplyConfig(Config);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag != "vrkb_mallet")
                return;

            // Usability tweak: Only allow the mallet to strike the key
            // from above. This prevents frequent double-striking of
            // characters when the mallet goes all the way
            // through the key and then collides with the key again on the
            // up-bounce.

            if (other.bounds.center.y > _collider.bounds.center.y
                && _keyboard.PressKey(this, other)) {
                _pressed = true;
                _pressStartTime = Time.time;
                _prevRepeatTime = Time.time;
                _renderer.sharedMaterial = _keyboard.PressedKeyMaterial;
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (!_pressed)
                return;

            if (other.tag != "vrkb_mallet")
                return;

            // Autorepeat keys when holding them down,
            // except for keys with special actions (e.g. Shift).

            if (Config.Action.Type != ActionType.Output)
                return;

            float delay = _keyboard.RepeatDelayInMilliseconds / 1000;
            float repeatInterval = 1.0f / _keyboard.RepeatRatePerSecond;
            float pressedTime = Time.time - _pressStartTime;
            float timeSinceRepeat = Time.time - _prevRepeatTime;

            _repeating = _pressed && delay > 0 && pressedTime >= delay;

            if (_repeating && timeSinceRepeat > repeatInterval) {
                _prevRepeatTime = Time.time;
                _keyboard.PressKey(this, other, true);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (!_pressed)
                return;

            if (other.tag != "vrkb_mallet")
                return;

            _pressed = false;
            _repeating = false;

            _keyboard.ReleaseKey(this, other);

            _renderer.sharedMaterial = _keyboard.UnpressedKeyMaterial;
        }

        protected void VisitComponentsWithTagPrefix<T>(string tagPrefix, Action<T, int> action)
            where T : MonoBehaviour
        {
            foreach (var o in GetComponentsInChildren<T>()) {
                if (o.tag.StartsWith(tagPrefix)) {

#if UNITY_EDITOR
                    Undo.RecordObject(o, "apply key config from JSON file");
#endif

                    int index;
                    if (int.TryParse(o.tag.Substring(tagPrefix.Length), out index))
                        action(o, index);

#if UNITY_EDITOR
                    PrefabUtility.RecordPrefabInstancePropertyModifications(o);
#endif

                }
            }
        }

        public void ApplyConfig(KeyConfig config)
        {
            // update label texts and colors

            VisitComponentsWithTagPrefix<TextMeshPro>("vrkb_label",
                (label, index) => {

                    label.text = null;
                    if (config.Labels != null && index < config.Labels.Length)
                        label.text = config.Labels[index];

                    if (config.LabelColors != null && index < config.LabelColors.Length)
                        label.color = config.LabelColors[index];
                    else
                        label.color = Color.black;

                    if (config.FontSizes != null && index < config.FontSizes.Length)
                        label.fontSize = config.FontSizes[index];
                    else
                        label.fontSize = 6.0f;

                });

            // update image sprites and their background colors

            VisitComponentsWithTagPrefix<Image>("vrkb_image",
                (image, index) => {

                    image.sprite = null;
                    // set image background color to clear so that image
                    // is invisible by default
                    image.color = Color.clear;
                    if (config.Images != null && index < config.Images.Length) {
                        image.sprite = Resources.Load<Sprite>(config.Images[index]);
                        image.preserveAspect = true;
                        // set background to white by to show the image
                        // with original colors (no tint)
                        image.color = Color.white;
                        if (config.ImageColors != null && index < config.ImageColors.Length)
                            image.color = config.ImageColors[index];
                    }

                });
        }
    }
}
