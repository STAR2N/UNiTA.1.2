namespace Game.Network {
    using System.Collections;
    using Mirror;
    using UnityEngine;

    public class VersionAuthenticator : NetworkAuthenticator {
        #region Messages

        public struct AuthRequestMessage : NetworkMessage {
            public string networkVersion;
            public string applicationVersion;
        }

        public struct AuthResponseMessage : NetworkMessage {
            public string message;
            public int code;
        }

        #endregion

        #region Server

        /// <summary>
        /// Called on server from StartServer to initialize the Authenticator
        /// <para>Server message handlers should be registered in this method.</para>
        /// </summary>
        public override void OnStartServer() {
            // register a handler for the authentication request we expect from client
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
        }

        /// <summary>
        /// Called on server from OnServerAuthenticateInternal when a client needs to authenticate
        /// </summary>
        /// <param name="conn">Connection to client.</param>
        public override void OnServerAuthenticate(NetworkConnection conn) { }

        /// <summary>
        /// Called on server when the client's AuthRequestMessage arrives
        /// </summary>
        /// <param name="conn">Connection to client.</param>
        /// <param name="msg">The message payload</param>
        public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg) {
            // check the credentials by calling your web server, database table, playfab api, or any method appropriate.
            if (msg.networkVersion == ExtraServerData.NETWORK_VERSION && msg.applicationVersion == ExtraServerData.APPLICATION_VERSION) {
                // create and send msg to client so it knows to proceed
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 100,
                    message = "Success"
                };

                conn.Send(authResponseMessage);

                // Accept the successful authentication
                ServerAccept(conn);
            } else {
                // create and send msg to client so it knows to disconnect
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 200,
                    message = "Invalid version"
                };

                conn.Send(authResponseMessage);

                // must set NetworkConnection isAuthenticated = false
                conn.isAuthenticated = false;

                // disconnect the client after 1 second so that response message gets delivered
                StartCoroutine(DelayedDisconnect(conn, 1));
            }
        }

        IEnumerator DelayedDisconnect(NetworkConnection conn, float waitTime) {
            yield return new WaitForSeconds(waitTime);

            // Reject the unsuccessful authentication
            ServerReject(conn);
        }

        #endregion

        #region Client

        /// <summary>
        /// Called on client from StartClient to initialize the Authenticator
        /// <para>Client message handlers should be registered in this method.</para>
        /// </summary>
        public override void OnStartClient() {
            // register a handler for the authentication response we expect from server
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }

        /// <summary>
        /// Called on client from OnClientAuthenticateInternal when a client needs to authenticate
        /// </summary>
        public override void OnClientAuthenticate() {
            AuthRequestMessage authRequestMessage = new AuthRequestMessage();
            authRequestMessage.networkVersion = ExtraServerData.NETWORK_VERSION;
            authRequestMessage.applicationVersion = ExtraServerData.APPLICATION_VERSION;
            NetworkClient.Send(authRequestMessage);
        }

        /// <summary>
        /// Called on client when the server's AuthResponseMessage arrives
        /// </summary>
        /// <param name="msg">The message payload</param>
        public void OnAuthResponseMessage(AuthResponseMessage msg) {
            if (msg.code == 100) {
                // Debug.LogFormat(LogType.Log, "Authentication Response: {0}", msg.message);

                // Authentication has been accepted
                ClientAccept();
            } else {
                Debug.LogError($"Authentication Response: {msg.message}");

                // Authentication has been rejected
                ClientReject();
            }
        }

        #endregion
    }
}