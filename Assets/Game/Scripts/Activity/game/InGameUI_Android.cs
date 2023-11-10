namespace Game.UI {
    using UnityEngine;

    public class InGameUI_Android : MonoBehaviour {
        // Start is called before the first frame update
        void Awake() {
            Screen.orientation = ScreenOrientation.Landscape;
        }

        private void OnDestroy() {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}