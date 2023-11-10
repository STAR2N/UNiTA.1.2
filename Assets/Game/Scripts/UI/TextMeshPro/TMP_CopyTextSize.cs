/// TMP_CopyTextSize.cs
//
//  Author: https://github.com/rrrfffrrr

using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace Production.UI.TMPro
{
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_CopyTextSize : MonoBehaviour
    {
        TMP_Text m_Source = null;
        public TMP_Text Source {
            get {
                if (m_Source == null) {
                    m_Source = GetComponent<TMP_Text>();
                }
                return m_Source;
            }
        }
        [SerializeField] public List<TMP_Text> m_Destination;

        private bool m_IsDirty = false;

        private void Update() {
            if (m_IsDirty) {
                m_IsDirty = false;
                UpdateSize();
            }
        }

        private void OnEnable() {
            SetDirty();
        }

        private void OnTransformParentChanged() {
            UpdateSize();
        }
        private void OnRectTransformDimensionsChange() {
            UpdateSize();
        }

        public void UpdateSize() {
            Source?.ForceMeshUpdate(true);
            foreach(var text in m_Destination) {
                text.enableAutoSizing = false;
                text.fontSize = m_Source.fontSize;
            }
        }

        public void SetDirty() {
            m_IsDirty = true;
        }
    }
}