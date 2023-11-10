/*
MIT License
Copyright (c) 2021 Chan-yeong Kang
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Game.TransformExtension {
    using UnityEngine;

    [ExecuteAlways]
    [ExecuteInEditMode]
    public class CopyRectTransform : MonoBehaviour {
        private RectTransform m_RectTransform;
        public RectTransform rectTransform {
            get {
                if (m_RectTransform == null)
                    m_RectTransform = transform as RectTransform;
                return m_RectTransform;
            }
        }
        public RectTransform Source;
        public Mode CopyMode = Mode.None;
        public Event CopyEvent = Event.Update;
        public CopyMethod CopypMethod = CopyMethod.FromSource;

        private void Update() => TryCopy(Event.Update);
        private void LateUpdate() => TryCopy(Event.LateUpdate);
        private void FixedUpdate() => TryCopy(Event.FixedUpdate);

        void TryCopy(Event @event) {
            if (Source == null)
                return;
            if (CopyEvent != @event)
                return;

            switch (CopypMethod) {
                case CopyMethod.FromSource:
                    Copy(Source, rectTransform);
                    break;
                case CopyMethod.ToSource:
                    Copy(rectTransform, Source);
                    break;
            }
        }
        void Copy(RectTransform src, RectTransform dest) {
            bool update = (CopyMode & Mode.Width) != Mode.None;
            if (update)
                dest.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, src.rect.width);

            update = (CopyMode & Mode.Height) != Mode.None;
            if (update)
                dest.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, src.rect.height);
        }

        [System.Flags]
        public enum Mode : byte {
            None        = 0b0000,
            Width       = 0b0001,
            Height      = 0b0010,
        }
        public enum Event {
            Update,
            LateUpdate,
            FixedUpdate
        }
        public enum CopyMethod {
            ToSource,
            FromSource,
        }

#if UNITY_EDITOR
        private void OnValidate() {
            TryCopy(CopyEvent);
        }
#endif
    }
}