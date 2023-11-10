using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CMF {
    public class FastMoveAbility : MonoBehaviour {
        [Header("Components")]
        [SerializeField] WalkerController Controller;

        [Header("Options")]
        public float RunSpeedMultipler = 2;

        [Header("Key")]
        public KeyCode RunKey = KeyCode.Space;

        private float OriginalMovementSpeed = 4;
        private bool IsRun = false;

        private void Awake() {
            OriginalMovementSpeed = Controller.movementSpeed;
        }

        private void Update() {
            IsRun = UnityEngine.Input.GetKey(RunKey);
        }

        private void FixedUpdate() {
            var canRun = Controller.IsGrounded() && IsRun;
            Controller.movementSpeed = OriginalMovementSpeed * (canRun ? RunSpeedMultipler : 1.0f);
        }
    }
}