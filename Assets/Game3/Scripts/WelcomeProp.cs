using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace iLLi
{
    public class WelcomeProp : MonoBehaviour
    {
        [SerializeField] AutoRotation rotator;
        [SerializeField] float lerpTime = 0.8f;
        [SerializeField] AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] List<Transform> paintOnSphere = new List<Transform>();

        Transform selection;
        public int CurrentSelection
        {
            get
            {
                if (selection == null)
                    return -1;
                return paintOnSphere.IndexOf(selection);
            }
        }

        [SerializeField] Canvas referenceCanvas;

        float startTime;
        bool isLerping;
        int nextSelection;
        Quaternion startRotation;

        [SerializeField] GameObject foregroundUI;
        [SerializeField] GameObject foregroundElements;
        [SerializeField] Image foregroundBackground;
        [SerializeField] List<GameObject> foregroundUIElements = new List<GameObject>();

        private void Awake()
        {
            Release();
        }
        private void OnDestroy()
        {
            Release();
        }
        private void Update()
        {
            if (selection != null)
            {
                if (isLerping) {
                    var lerp = Mathf.Clamp01((Time.time - startTime) * lerpTime);

                    var targetPosition = rotator.transform.position + referenceCanvas.worldCamera.transform.InverseTransformPoint(referenceCanvas.transform.position) - referenceCanvas.worldCamera.transform.InverseTransformPoint(selection.position);
                    var targetRotation = Quaternion.Inverse(Quaternion.Inverse(rotator.transform.rotation) * selection.rotation);
                    var targetScale = referenceCanvas.transform.lossyScale;
                    targetScale.x /= selection.localScale.x;
                    targetScale.y /= selection.localScale.y;
                    targetScale.z /= selection.localScale.z;

                    rotator.transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, positionCurve.Evaluate(lerp));
                    rotator.transform.rotation = Quaternion.LerpUnclamped(startRotation, targetRotation, rotationCurve.Evaluate(lerp));
                    rotator.transform.localScale = Vector3.LerpUnclamped(Vector3.one, targetScale, scaleCurve.Evaluate(lerp));

                    if (lerp >= 1)
                    {
                        isLerping = false;
                        foregroundElements.SetActive(true);
                    }
                }
            }
            else
            {
                if (isLerping)
                {
                    var lerp = Mathf.Clamp01((Time.time - startTime) * lerpTime);
                    if (0 <= nextSelection && nextSelection < paintOnSphere.Count)
                    {
                        var targetRotation = Quaternion.Inverse(Quaternion.Inverse(rotator.transform.rotation) * paintOnSphere[nextSelection].rotation);
                        rotator.transform.rotation = Quaternion.LerpUnclamped(startRotation, targetRotation, lerp);
                    }
                    rotator.transform.localPosition = Vector3.MoveTowards(rotator.transform.localPosition, Vector3.zero, lerp);
                    rotator.transform.localScale = Vector3.MoveTowards(rotator.transform.localScale, Vector3.one, lerp);
                    if (lerp >= 1)
                    {
                        isLerping = false;
                    }
                }
            }
        }

        private void SetActiveMouseRotator(bool active)
        {
            var rotator = GetComponentInParent<MouseRotator>();
            if (rotator != null)
                rotator.enabled = active;
        }

        public void Select(int index)
        {
            if (selection != null)
                return;
            rotator.enabled = false;
            startTime = Time.time;
            selection = paintOnSphere[index];
            startRotation = rotator.transform.rotation;
            foregroundUI.SetActive(true);
            foregroundElements.SetActive(false);
            foregroundBackground.raycastTarget = true;
            isLerping = true;
            foregroundBackground.DOFade(1, lerpTime).From(0).Play();
            for (var i = 0; i < foregroundUIElements.Count; ++i)
            {
                foregroundUIElements[i].SetActive(index == i);
            }
            SetActiveMouseRotator(false);
        }
        public void ForceSelect(int index)
        {
            rotator.enabled = false;
            startTime = Time.time - 1f / lerpTime;
            selection = paintOnSphere[index];
            startRotation = rotator.transform.rotation;
            foregroundUI.SetActive(true);
            foregroundElements.SetActive(false);
            foregroundBackground.raycastTarget = true;
            isLerping = true;
            var color = foregroundBackground.color;
            color.a = 1;
            foregroundBackground.color = color;
            for (var i = 0; i < foregroundUIElements.Count; ++i)
            {
                foregroundUIElements[i].SetActive(index == i);
            }

            SetActiveMouseRotator(false);
        }

        public void Release()
        {
            rotator.enabled = true;
            nextSelection = CurrentSelection + 1;
            selection = null;
            startTime = Time.time;
            startRotation = rotator.transform.rotation;
            foregroundUI.SetActive(false);
            foregroundBackground.raycastTarget = false;
            isLerping = true;
            foregroundBackground.DOFade(0, lerpTime).From(1).Play();
            SetActiveMouseRotator(true);
        }
    }
}