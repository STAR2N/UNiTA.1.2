using UnityEngine;

namespace iLLi
{
    public class AutoRotation : MonoBehaviour, MouseRotator.IEvent
    {
        float startTime;
        [SerializeField] AnimationCurve weight = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] float rotatePerSecond = 30f;

        private void OnEnable()
        {
            startTime = Time.time;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, weight.Evaluate(Time.time - startTime) * rotatePerSecond * Time.deltaTime);
        }

        public void OnDragInspectable(Vector2 rotation)
        {
            startTime = Time.time;
        }
    }
}