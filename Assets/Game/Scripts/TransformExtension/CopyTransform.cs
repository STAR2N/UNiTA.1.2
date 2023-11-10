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
    public class CopyTransform : MonoBehaviour {
        public Transform Source;
        public Mode CopyMode = Mode.None;
        public Event CopyEvent = Event.Update;
        public Target CopyTarget = Target.FromSource;

        private void Update() => TryCopy(Event.Update);
        private void LateUpdate() => TryCopy(Event.LateUpdate);
        private void FixedUpdate() => TryCopy(Event.FixedUpdate);

        void TryCopy(Event @event) {
            if (Source == null)
                return;
            if (CopyEvent != @event)
                return;

            switch(CopyTarget) {
                case Target.FromSource:
                    Copy(Source, transform);
                    break;
                case Target.ToSource:
                    Copy(transform, Source);
                    break;
            }
        }
        void Copy(Transform src, Transform dest) {
            bool update = (CopyMode & Mode.Position) != Mode.None;
            if (update)
                dest.position = src.position;

            update = (CopyMode & Mode.Rotation) != Mode.None;
            if (update)
                dest.rotation = src.rotation;

            update = (CopyMode & Mode.Scale) != Mode.None;
            if (update)
                dest.localScale = src.localScale;
        }

        [System.Flags]
        public enum Mode : byte {
            None        = 0b0000,
            Position    = 0b0001,
            Rotation    = 0b0010,
            Scale       = 0b0100,
            PositionAndRotation = Position | Rotation,
            PositionAndScale = Position | Scale,
            RotationAndScale = Rotation | Scale,
            All = Position | Rotation | Scale
        }
        public enum Event {
            Update,
            LateUpdate,
            FixedUpdate
        }
        public enum Target {
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