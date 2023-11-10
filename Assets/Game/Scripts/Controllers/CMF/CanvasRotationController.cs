using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotationController : MonoBehaviour {
    public static bool EnableAdvancedRotation = true;
    [SerializeField] Camera m_Camera;

    private RectTransform m_RectTransform;
    public RectTransform rectTransform {
        get {
            return m_RectTransform;
        }
    }
    // 0 == original forward
    // 1 == look at camera
    public AnimationCurve RotationCurve = AnimationCurve.Constant(0, 4, 1);
    public AnimationCurve PivotCurve = AnimationCurve.Constant(0, 4, 1);

    public Camera Camera {
        get {
            return (m_Camera == null) ? Camera.main : m_Camera;
        }
    }

    private void Awake() {
        m_RectTransform = transform as RectTransform;
    }
    private void LateUpdate() {
        var camera = Camera;
        if (camera != null) {
            // Evaluate look quaterion and distance
            var toWebcam = transform.position - camera.transform.position;
            var look = Quaternion.LookRotation(-toWebcam);
            var lookY = look.eulerAngles.y;
            var dist = toWebcam.magnitude;

            // Calculate param
            var rotParam = RotationCurve.Evaluate(dist);
            var pivot = new Vector2(0.5f, rectTransform.pivot.y);

            if (EnableAdvancedRotation) {
                var pivotParam = PivotCurve.Evaluate(dist);

                // Calculate pivot direction
                var right = Vector3.Cross(camera.transform.forward, toWebcam);
                var isRight = Vector3.Dot(right, camera.transform.up) > 0;
                pivot.x = 0.5f + pivotParam * (isRight ? -0.5f : 0.5f);
            }

            // Dispatch output
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.parent.rotation.eulerAngles.y, lookY, rotParam), 0);
            rectTransform.pivot = pivot;
        }
    }
}
