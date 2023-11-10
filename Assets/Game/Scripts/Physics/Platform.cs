namespace Game.PhysicsExtension {
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Platform : MonoBehaviour {
        /// <summary>
        /// List of transforms that move with platform.
        /// </summary>
        private readonly List<Transform> m_WalkingTransforms = new List<Transform>();
        private Vector3 m_LatestPosition;

        private void FixedUpdate() {
            var diff = transform.position - m_LatestPosition;
            m_LatestPosition = transform.position;
            UpdateEachTransforms(diff);
        }

        private void UpdateEachTransforms(Vector3 movement) {
            foreach(var transform in m_WalkingTransforms) {
                if (transform != null)
                    transform.position += movement;
            }
        }

        public void Regist(Transform target) {
            if (!m_WalkingTransforms.Contains(target))
                m_WalkingTransforms.Add(target);
        }

        public void Unregist(Transform target) {
            m_WalkingTransforms.Remove(target);
        }
    }
}