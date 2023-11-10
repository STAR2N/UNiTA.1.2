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

namespace Game.PhysicsExtension {
    using UnityEngine;

    /// <summary>
    /// Unreal spring arm to cast a ray and move tail transform to it.
    /// </summary>
    [ExecuteAlways]
    [ExecuteInEditMode]
    public sealed class SpringArm : MonoBehaviour {
        [Header("Engine settings")]
        [SerializeField] EventType m_TickMode;

        [Header("Options")]
        [SerializeField] Transform m_TailTransform;
        [SerializeField] float m_Distance;
        [SerializeField] float m_SphereRadius = 0.1f;
        [SerializeField] LayerMask m_CollisionLayers;
        [SerializeField] CastType m_CollisionType;

        public Transform TailTransform => m_TailTransform;
        public float Distance {
            get => m_Distance;
            set => m_Distance = Mathf.Max(value, 0);
        }

        void Update() {
            if (m_TickMode == EventType.Update)
                UpdateArm();
        }
        void LateUpdate() {
            if (m_TickMode == EventType.LateUpdate)
                UpdateArm();
        }
        void FixedUpdate() {
            if (m_TickMode == EventType.FixedUpdate)
                UpdateArm();
        }

        public void UpdateArm() {
            if (m_Distance == 0) {
                m_TailTransform.position = transform.position;
                return;
            }

            var calculatedTailPosition = transform.position -transform.forward * m_Distance;
            var ray = new Ray(transform.position, -transform.forward);

            switch(m_CollisionType) {
                case CastType.Raycast: {
                        if (Physics.Raycast(ray, out var hit, m_Distance, m_CollisionLayers.value)) {
                            calculatedTailPosition = transform.position - transform.forward * hit.distance;
                        }
                    }
                    break;
                case CastType.Spherecast: {
                        if (Physics.SphereCast(ray, m_SphereRadius, out var hit, m_Distance, m_CollisionLayers.value)) {
                            calculatedTailPosition = transform.position - transform.forward * hit.distance;
                        }
                    }
                    break;
            }

            m_TailTransform.position = calculatedTailPosition;
        }

        public enum EventType {
            Update,
            LateUpdate,
            FixedUpdate,
        }
        public enum CastType {
            Raycast,
            Spherecast,
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            var pushColor = Gizmos.color;

            var desirePos = transform.position - transform.forward * m_Distance;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, desirePos);

            if (m_CollisionType == CastType.Spherecast) {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, m_SphereRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(desirePos, m_SphereRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(m_TailTransform.position, m_SphereRadius);
            }

            Gizmos.color = pushColor;
        }
#endif
    }
}