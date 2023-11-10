using UnityEngine;

namespace iLLi
{
    /// <summary>
    /// Sync transform with mouse
    /// </summary>
    public class Mouse2Transform : MonoBehaviour
    {
        [SerializeField] new Camera camera;

        public void Update()
        {
            if (camera == null)
                camera = Camera.main;

            if (camera == null)
                return;

            var ray = camera.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.origin;
            transform.rotation = Quaternion.LookRotation(ray.direction, camera.transform.up);
        }
    }
}