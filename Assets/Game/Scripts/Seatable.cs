namespace Game {
    using Game.Controller;
    using Mirror;
    using UnityEngine;
    using Game.Player;
    using Game.Utility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Seatable : NetworkBehaviour {
        static readonly ListMap<string, Seatable> Categories = new ListMap<string, Seatable>();

        // [DllImport("__Internal")]
        // private static extern void ChangeSeat_React(string visibleCharacterNames);

        [UnityEngine.Serialization.FormerlySerializedAs("m_RoomName")]
        [SyncVar(hook = nameof(Hook_RoomName))] public string RoomName;
        private void Hook_RoomName(string @old, string @new) {
            if (!string.IsNullOrEmpty(@old)) {
                Categories.RemoveElement(@old, this);
            }
            if (!string.IsNullOrEmpty(@new)) {
                Categories.AddElement(@new, this);
            }
        }
        [SerializeField] GameObject m_WalkingCharacterPrefab;
        [SerializeField] GameObject m_SeatedCharacterPrefab;

        [SerializeField] Transform ExitPosition;
        [SerializeField] Transform m_WebcamPosition;
        public Transform WebcamPosition => m_WebcamPosition;
        [SyncVar, HideInInspector] public NetworkIdentity CurrentSeatCharacter;

        #region Unity events
        private void Awake() {
            Hook_RoomName(null, RoomName);
        }
        private void OnDestroy() {
            Hook_RoomName(RoomName, null);
        }
        #endregion

        #region Client
        public override void OnStartClient() {
            base.OnStartClient();

            var interactor = gameObject.AddComponent<SeatInteractor>();
            interactor.Seat = this;
        }
        #endregion

        #region Command
        [Command(requiresAuthority = false)]
        public void CmdTrySit(NetworkConnectionToClient sender = null) {
            if (CurrentSeatCharacter != null)
                return;

            GameNetworkManager.RoomNames[sender.connectionId] = RoomName;
            CurrentSeatCharacter = SpawnSeatCharacter(sender);
            NoticeRoomPlayerJoin(sender);
        }

        [Command(requiresAuthority = false)]
        public void CmdTryStand(NetworkConnectionToClient sender = null) {
            if (sender == null)
                return;
            if (CurrentSeatCharacter == null || !sender.clientOwnedObjects.Contains(CurrentSeatCharacter))
                return;

            SpawnWalkingCharacter(sender);
            CurrentSeatCharacter = null;
            GameNetworkManager.RoomNames[sender.connectionId] = string.Empty;
            NoticeRoomPlayerLeave(sender);
        }
        #endregion

        #region Room player dispatcher
        private void NoticeRoomPlayerJoin(NetworkConnectionToClient sender) {
            var selfSeat = CurrentSeatCharacter.GetComponent<SeatCharacter>();
            if (Categories.TryGetValue(RoomName, out var seats)) {
                foreach(var seat in seats) {
                    if (seat.CurrentSeatCharacter == null)
                        continue;

                    var seatCharacter = seat.CurrentSeatCharacter.GetComponent<SeatCharacter>();
                    selfSeat.RpcSetVisible(seatCharacter.connectionToClient, sender.connectionId);
                    selfSeat.RpcSetName(seatCharacter.connectionToClient, "SeatCharacter-" + sender.connectionId);
                    seatCharacter.RpcSetVisible(sender, seatCharacter.connectionToClient.connectionId);
                    seatCharacter.RpcSetName(sender, "SeatCharacter-" + seatCharacter.connectionToClient.connectionId);
                }
            }
        }
        private void NoticeRoomPlayerLeave(NetworkConnectionToClient sender) {
            if (Categories.TryGetValue(RoomName, out var seats)) {
                foreach (var seat in seats) {
                    if (seat.CurrentSeatCharacter == null)
                        continue;

                    var seatCharacter = seat.CurrentSeatCharacter.GetComponent<SeatCharacter>();
                    seatCharacter.RpcSetInvisible(sender);
                    seatCharacter.RpcSetName(sender, "SeatCharacter");
                }
            }
        }
        #endregion

        #region Character management
        public NetworkIdentity SpawnSeatCharacter(NetworkConnectionToClient sender) {
            var characterObject = Instantiate(m_SeatedCharacterPrefab, transform.position, transform.rotation);

            NetworkServer.RemovePlayerForConnection(sender, true);
            NetworkServer.AddPlayerForConnection(sender, characterObject);

            var character = characterObject.GetComponent<SeatCharacter>();
            character.RelatedSeat = netIdentity;
            character.RpcNotifyRoom(RoomName);
            character.name = "SeatCharacter-" + sender.connectionId;

            var roomCode = Network.ServerData.Instance.ServerId;
            character.RpcJoinRoom(sender, roomCode, RoomName, sender.connectionId);

            // Debug.Log("SpawnSeatCharacter");

            return characterObject.GetComponent<NetworkIdentity>();
        }
        public void SpawnWalkingCharacter(NetworkConnectionToClient sender) {
            var characterObject = Instantiate(m_WalkingCharacterPrefab, ExitPosition.position, ExitPosition.rotation);

            NetworkServer.RemovePlayerForConnection(sender, true);
            NetworkServer.AddPlayerForConnection(sender, characterObject);

//            var character = characterObject.GetComponent<WalkingCharacter>();
            var character = characterObject.GetComponent<VideoUtils>();
            var roomCode = Network.ServerData.Instance?.ServerId;
            character.RpcExitRoom(sender, roomCode, RoomName);
            // Debug.Log("SpawnWalkingCharacter");
        }
        #endregion

        [Serializable]
        public class Serialization<T>
        {
            [SerializeField]
            List<T> target;
            public List<T> ToList() { return target; }

            public Serialization(List<T> target) 
            {
                this.target = target;
            }
        }

        // public void CheckVisibleCharacters(Camera cam) {
        //      if (Categories.TryGetValue(RoomName, out var seats)) {
        //         string visibleCharacterNames = "";
        //         foreach (var seat in seats) {
        //             if (seat.CurrentSeatCharacter == null)
        //                 continue;

        //             var seatCharacter = seat.CurrentSeatCharacter.GetComponent<SeatCharacter>();
        //             Vector3 viewPos = cam.WorldToViewportPoint(seatCharacter.transform.position);
        //             if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        //             {
        //                 string[] splitStr = seat.CurrentSeatCharacter.name.Split('-');
        //                 if(splitStr.Length != 2) continue;

        //                 visibleCharacterNames += splitStr[1];
        //                 visibleCharacterNames += ';';
        //             }
        //         }

        //         ChangeSeat_React(visibleCharacterNames);
        //     }
        // }

        public sealed class SeatInteractor : InteractionController.Interactable {
            public Seatable Seat;

            public override void Interact(InteractionController controller) {
                var characterController = controller.GetComponent<VideoUtils>();
                Debug.Log($"Trysit {characterController}");
                if (characterController != null) {
                    Seat.CmdTrySit();
                }
            }
        }
    }
}