using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using DG.Tweening;

namespace iLLi
{
    public class WelcomeProp_Page6 : MonoBehaviour
    {
        [SerializeField] int index;
        public int Index
        {
            get => index;
            set
            {
                ActiveElementInternal(value);
                index = value;
            }
        }

        [SerializeField] RectTransform bigElementPlaceholder;
        [SerializeField] List<RectTransform> elements = new List<RectTransform>();
        [SerializeField] UnityEvent endOfPageEvent = new UnityEvent();

        Sequence sequence;

        private void OnEnable()
        {
            index = -1;
            DeactiveAllInternal();
        }
        private void DeactiveAllInternal()
        {
            if (sequence != null)
            {
                sequence.Kill(true);
                sequence = null;
            }

            foreach (var element in elements)
            {
                element.offsetMin = Vector2.zero;
                element.offsetMax = Vector2.zero;
                element.localScale = Vector3.one;
                element.gameObject.SetActive(false);
            }
        }
        private void ActiveElementInternal(int index)
        {
            DeactiveAllInternal();
            var target = elements[index];
            target.gameObject.SetActive(true);
            sequence = DOTween.Sequence()
                .Join(target.DOMove(bigElementPlaceholder.position, 0.5f))
                .Join(target.DOScale(1.5f, 0.5f))
                .Play();
        }

        public void Next()
        {
            if (index < (elements.Count - 1))
                Index = index + 1;
            else
                endOfPageEvent.Invoke();
        }
    }
}