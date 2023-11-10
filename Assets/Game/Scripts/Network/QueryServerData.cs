using LightReflectiveMirror;
using UnityEngine;

namespace Game.Net
{
    public class QueryServerData : MonoBehaviour
    {
        protected QueryServerData() { }

        public static void QueryAsync(System.Action<string> callback, Type dataType = Type.Code)
        {
            var obj = new GameObject($"SERVER DATA QUERY").AddComponent<QueryServerData>();
            if (obj == null)
            {
                callback(null);
                return;
            }
            obj.m_Callback = callback;
            obj.m_Type = dataType;
        }

        private Type m_Type;
        private System.Action<string> m_Callback;
        private bool m_Handled = false;

        private void Update()
        {
            var transport = LightReflectiveMirrorTransport.activeTransport as LightReflectiveMirrorTransport;
            if (transport == null)
                return;

            switch (m_Type)
            {
                case Type.Code:
                    if (!string.IsNullOrWhiteSpace(transport.serverId))
                    {
                        m_Handled = true;
                        m_Callback?.Invoke(transport.serverId);
                        Destroy(gameObject);
                    }
                    break;
                case Type.Name:
                    if (!string.IsNullOrWhiteSpace(transport.serverName))
                    {
                        m_Handled = true;
                        m_Callback?.Invoke(transport.serverName);
                        Destroy(gameObject);
                    }
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }

        private void OnDestroy()
        {
            if (!m_Handled)
                m_Callback?.Invoke(null);
        }

        public enum Type
        {
            Name,
            Code
        }
    }
}