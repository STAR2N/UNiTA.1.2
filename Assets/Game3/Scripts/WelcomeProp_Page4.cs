using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using DG.Tweening;

namespace iLLi
{
    public class WelcomeProp_Page4 : MonoBehaviour
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
        [SerializeField] RectTransform container;
        [SerializeField] List<RectTransform> elements = new List<RectTransform>();
        [SerializeField] List<float> graphicAnchor = new List<float>();
        [SerializeField] List<VideoPlayer> videos = new List<VideoPlayer>();

        [SerializeField] UnityEvent endOfPageEvent = new UnityEvent();

        private void Awake()
        {
            foreach (var video in videos)
            {
                if (video != null)
                {
                    video.Prepare();
                    video.prepareCompleted += Video_prepareCompleted;
                }
            }
        }

        private void OnEnable()
        {
            index = 1;
            var pos = container.localPosition;
            pos.x = graphicAnchor[index];
            container.localPosition = pos;
            ActiveElementInternal(index);
        }
        private void OnDisable()
        {
            foreach(var video in videos)
            {
                if (video != null)
                    video.Stop();
            }
        }

        private void Video_prepareCompleted(VideoPlayer source)
        {
            source.Play();
            var videoIndex = videos.IndexOf(source);
            if (videoIndex < 0 || videoIndex != Index)
            {
                IEnumerator delayedStop()
                {
                    yield return null;
                    source.Pause();
                }
                StartCoroutine(delayedStop());
            }
        }

        private void ActiveElementInternal(int index)
        {
            for(var i = 0; i < elements.Count; ++i)
            {
                var video = videos[i];
                if (i == index)
                {
                    if (video != null)
                        video.Play();
                } else
                {
                    if (video != null)
                        video.Pause();
                }
            }
            void Animation()
            {
                var from = container.localPosition.x;
                var to = graphicAnchor[index];
                DOTween.To(value =>
                {
                    var pos = container.localPosition;
                    pos.x = Mathf.Lerp(from, to, value);
                    container.localPosition = pos;
                }, 0, 1, 0.5f);
            }
            Animation();
        }

        public void Next()
        {
            switch(index)
            {
                case 0: Index = 2; break;
                case 1: Index = 0; break;
                default: endOfPageEvent.Invoke(); break;
            }
        }
    }
}