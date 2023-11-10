using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace iLLi
{
    /// <summary>
    /// Handle events with inspector
    /// </summary>
    public class NetworkEvents : NetworkBehaviour
    {
        [SerializeField] UnityEvent startClientEvent = new UnityEvent();
        [SerializeField] UnityEvent stopClientEvent = new UnityEvent();

        [SerializeField] UnityEvent startServerEvent = new UnityEvent();
        [SerializeField] UnityEvent stopServerEvent = new UnityEvent();

        [SerializeField] UnityEvent startLocalPlayerEvent = new UnityEvent();

        [SerializeField] UnityEvent startAuthorityEvent = new UnityEvent();
        [SerializeField] UnityEvent stopAuthorityEvent = new UnityEvent();

        public override void OnStartClient()
        {
            base.OnStartClient();
            startClientEvent.Invoke();
            Debug.Log("Start client", this);
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            stopClientEvent.Invoke();
            Debug.Log("Stop client", this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            startServerEvent.Invoke();
            Debug.Log("Start server", this);
        }
        public override void OnStopServer()
        {
            base.OnStopServer();
            stopServerEvent.Invoke();
            Debug.Log("Stop server", this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            startLocalPlayerEvent.Invoke();
            Debug.Log("Start local payer", this);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            startAuthorityEvent.Invoke();
            Debug.Log("Start authority", this);
        }
        public override void OnStopAuthority()
        {
            base.OnStopAuthority();
            stopAuthorityEvent.Invoke();
            Debug.Log("Stop authority", this);
        }
    }
}