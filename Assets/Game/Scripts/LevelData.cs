namespace Game.Scene {
    using System.Collections.Generic;
    using UnityEngine;
    using Mirror;
    using UnityEngine.UI;

    [CreateAssetMenu(menuName = "Levels")]
    public class LevelData : ScriptableObject {
        [SerializeField] List<Level> LevelList;
        private Dictionary<string, Level> m_LevelCache = null;
        public Dictionary<string, Level> Levels {
            get {
                if (m_LevelCache == null) {
                    m_LevelCache = new Dictionary<string, Level>();
                    foreach(var elem in LevelList) {
                        m_LevelCache.Add(elem.Name, elem);
                    }
                }

                return m_LevelCache;
            }
        }

        [System.Serializable]
        public class Level {
            public string Name;
            public Sprite Image;
            [Scene] public string Scene;
        }
    }
}