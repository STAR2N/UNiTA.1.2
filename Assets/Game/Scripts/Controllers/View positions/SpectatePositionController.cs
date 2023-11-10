namespace Game.Controller {
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Game.Controller.Player;
    using Game.CMF.Input;

    public class SpectatePositionController : MonoBehaviour {
        [SerializeField] SeatInputHandler m_Input;
        [SerializeField] SeatedCharacterController m_Controller;
        [HideInInspector] public List<ViewPosition> Positions = new List<ViewPosition>();
        public int Cursor { get; private set; }


        [Header("Events")]
        public UnityEvent OnPositionChanged = new UnityEvent();


        #region Unity events
        private void Update() {
            if (Positions.Count > 0) {
                var input = m_Input.GetChangeSeatInput();
                if (input < 0) {
                    MovePrevious();
                } else if (input > 0) {
                    MoveNext();
                }
            }
        }
        #endregion

        #region Position loader
        public void LoadPositions(string roomName) {
            Positions = new List<ViewPosition>();
            Cursor = 0;

            if (!string.IsNullOrEmpty(roomName) && ViewPosition.Positions.TryGetValue(roomName, out var list)) {
                foreach(var pos in list) {
                    Positions.Add(pos);
                }
            }
        }

        public void AddPosition(ViewPosition pos) {
            if (!Positions.Contains(pos)) {
                Positions.Add(pos);
            }
        }
        public void InsertPosition(int index, ViewPosition pos) {
            if (!Positions.Contains(pos)) {
                Positions.Insert(index, pos);
            }
        }
        #endregion

        #region Mover
        public void MoveNext() {
            if (Positions.Count <= 0)
                return;

            Cursor++;
            if (Cursor >= Positions.Count) {
                Cursor = 0;
            }

            MoveToPosition(Cursor);
        }
        public void MovePrevious() {
            if (Positions.Count <= 0)
                return;

            Cursor--;
            if (Cursor < 0) {
                Cursor = Positions.Count - 1;
            }

            MoveToPosition(Cursor);
        }

        public void MoveToPosition(int index) {
            if (index < 0 || index >= Positions.Count)
                return;

            var pos = Positions[index];
            MoveToPosition(pos);
        }
        private void MoveToPosition(ViewPosition pos, bool includeScale = false) {
            CanvasRotationController.EnableAdvancedRotation = pos.EnableAdvancedRotation;
            transform.SetPositionAndRotation(pos.transform.position, pos.transform.rotation);
            if (includeScale)
                transform.localScale = pos.transform.localScale;
            if (m_Controller.Camera != null) {
                m_Controller.Camera.fieldOfView = pos.Fov;
            }

            OnPositionChanged.Invoke();
        }
        #endregion
    }
}