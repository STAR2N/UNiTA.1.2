using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Game
{
    /// <summary>
    /// Acquiring start params and provide it
    /// </summary>
    public class StartupManager : MonoBehaviour
    {
        public static StartupManager Instance { get; private set; }

        [SerializeField, Scene] string nextScene;
        public InitializeParams Params = new InitializeParams
        {
            ServerURL = "127.0.0.1"
        };

        bool isInitialized = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            StartInternal();
        }

        private void StartInternal()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // Wait for communication
#else
            FinishStartupInternal();
#endif
        }

        private void InitializeWebGL(string json)
        {
            if (isInitialized)
                return;
            isInitialized = true;

            Params = JsonUtility.FromJson<InitializeParams>(json);
            FinishStartupInternal();
        }

        private void FinishStartupInternal()
        {
            SceneManager.LoadScene(nextScene);
        }

        [System.Serializable]
        public struct InitializeParams {
            public string Room;
            public string Map;
            public string ServerURL;
            public string Email;
        }
    }
}