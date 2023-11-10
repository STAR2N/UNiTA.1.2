/// SubtractAspectSizeFitter.cs
//
//  Author: https://github.com/rrrfffrrr

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Production.UI
{
    /// <summary>
    /// Same like AspectSizeFitter, but set size parent size - aspect * other 
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class SubtractAspectSizeFitter : UIBehaviour, ILayoutSelfController
    {
        RectTransform m_rectTransform;
        RectTransform rectTransform {
            get {
                if (m_rectTransform == null)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        public DirectionType Direction = DirectionType.None;
        public float AspectRatio = 1;
        public bool ClampZero = false;

        // field is never assigned warning
#pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
#pragma warning restore 649

        private bool m_DoesParentExist = false;
        bool m_DelayedUpdate = false;

        protected override void OnTransformParentChanged() {
            m_DoesParentExist = rectTransform.parent ? true : false;
            SetDirty();
        }
        protected override void OnRectTransformDimensionsChange() {
            UpdateRect();
        }

        protected override void OnEnable() {
            base.OnEnable();
            m_DoesParentExist = rectTransform.parent ? true : false;
            SetDirty();
        }
        protected override void OnDisable() {
            m_Tracker.Clear();
            base.OnDisable();
        }

        private void Update() {
            if (m_DelayedUpdate) {
                m_DelayedUpdate = false;
                UpdateRect();
            }
        }

        void UpdateRect() {
            float Calculate(in float axis, in float alterAxis, in float ratio) {
                var ret = axis - alterAxis * ratio;
                if (ClampZero) {
                    ret = Mathf.Max(ret, 0);
                }
                return ret;
            }

            m_Tracker.Clear();

            if (!m_DoesParentExist)
                return;

            switch (Direction) {
                case DirectionType.None: break;
                case DirectionType.Horizontal: {
                        var parentSize = GetParentSize();
                        m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Calculate(parentSize.x, parentSize.y, AspectRatio));
                    }
                    break;
                case DirectionType.Vertical: {
                        var parentSize = GetParentSize();
                        m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Calculate(parentSize.y, parentSize.x, 1f / AspectRatio));
                    }
                    break;
            }
        }

        private Vector2 GetParentSize() {
            RectTransform parent = rectTransform.parent as RectTransform;
            return !parent ? Vector2.zero : parent.rect.size;
        }

        void SetDirty() {
            m_DelayedUpdate = true;
        }

        public void SetLayoutHorizontal() { }

        public void SetLayoutVertical() { }

#if UNITY_EDITOR
        protected override void OnValidate() {
            SetDirty();
        }
#endif
        public enum DirectionType
        {
            None,
            Horizontal,
            Vertical,
        }
    }
}