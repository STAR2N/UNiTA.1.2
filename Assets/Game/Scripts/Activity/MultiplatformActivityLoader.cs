namespace Game.Activity.Loader {
    using UnityEngine;

    public class MultiplatformActivityLoader : MonoBehaviour {
        public GameObject StandaloneActivity;
        public GameObject AndroidActivity;
        public GameObject WebGLActivity;
        public GameObject UWPActivity;
        public GameObject TVOSActivity;
        public GameObject PS4Activity;
        public GameObject iOSActivity;
        public GameObject XboxOneActivity;

        private void Awake() {
#if UNITY_STANDALONE
            Instantiate(StandaloneActivity);
#elif UNITY_ANDROID
            Instantiate(AndroidActivity);
#elif UNITY_WEBGL
            Instantiate(WebGLActivity);
#elif UNITY_WSA
            Instantiate(UWPActivity);
#elif UNITY_TVOS
            Instantiate(TVOSActivity);
#elif UNITY_PS4
            Instantiate(PS4Activity);
#elif UNITY_IOS
            Instantiate(iOSActivity);
#elif UNITY_XBOXONE
            Instantiate(XboxOneActivity);
#else
            Instantiate(StandaloneActivity);
#endif
            Destroy(gameObject);
        }
    }
}