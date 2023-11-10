
namespace Game.TransformExtension {
    using UnityEngine;

    public class CopyRotation : MonoBehaviour {
        public Transform Source;
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
                    Copy(Source, transform);
                    break;
                case CopyMethod.ToSource:
                    Copy(transform, Source);
                    break;
            }
        }
        void Copy(Transform src, Transform dest) {
            Vector3 euler = dest.eulerAngles;

            bool update = (CopyMode & Mode.X) != Mode.None;
            if (update)
                euler.x = src.eulerAngles.x;

            update = (CopyMode & Mode.Y) != Mode.None;
            if (update)
                euler.y = src.eulerAngles.y;

            update = (CopyMode & Mode.Z) != Mode.None;
            if (update)
                euler.z = src.eulerAngles.z;

            dest.eulerAngles = euler;
        }

        [System.Flags]
        public enum Mode : byte {
            None        = 0b000,
            X           = 0b001,
            Y           = 0b010,
            Z           = 0b100,
            XY          = 0b011,
            YZ          = 0b110,
            XZ          = 0b101,
            XYZ          = 0b111,
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