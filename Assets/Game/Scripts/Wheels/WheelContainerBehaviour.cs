namespace Game.Wheels {
    using Mirror;
    using UnityEngine;
    using System.Collections.Generic;
    using Game.Controller;

    public class WheelContainerBehaviour : NetworkBehaviour {
        [SyncVar] public string Room;
        [SerializeField] Transform m_SeatOffsetGroup;
        [SerializeField] Transform m_ViewOffsetGroup;
        readonly Dictionary<int, NetworkIdentity> m_Seats = new Dictionary<int, NetworkIdentity>();
        readonly Dictionary<int, NetworkIdentity> m_Views = new Dictionary<int, NetworkIdentity>();

        [SerializeField] GameObject m_SeatPrefab;
        [SerializeField] GameObject m_ViewPrefab;

        public override void OnStartServer() {
            base.OnStartServer();

            Room = $"Wheel room identity({System.Guid.NewGuid()})";

            GenerateSeats();
            GenerateViews();

            UpdateRoomNames();
        }

        public override void OnStartClient() {
            base.OnStartClient();
            CmdRequestSync();
        }

        [Server]
        void GenerateSeats() {
            int i = 0;
            foreach (Transform offset in m_SeatOffsetGroup) {
                var childIndex = i++;
                try {
                    var seat = Instantiate(m_SeatPrefab);
                    NetworkServer.Spawn(seat);
                    m_Seats.Add(childIndex, seat.GetComponent<NetworkIdentity>());
                    AttachSeatInternal(seat.transform, childIndex);
                } catch (System.Exception e) {
                    Debug.LogError(e, this);
                }
            }
        }
        [Server]
        void GenerateViews() {
            int i = 0;
            foreach (Transform offset in m_ViewOffsetGroup) {
                var childIndex = i++;
                try {
                    var seat = Instantiate(m_ViewPrefab);
                    NetworkServer.Spawn(seat);
                    m_Views.Add(childIndex, seat.GetComponent<NetworkIdentity>());
                    AttachSeatInternal(seat.transform, childIndex);
                } catch (System.Exception e) {
                    Debug.LogError(e, this);
                }
            }
        }
        [Server]
        void UpdateRoomNames() {
            foreach (var seatTuple in m_Seats) {
                var seat = seatTuple.Value.GetComponent<Seatable>();
                seat.RoomName = Room;
            }

            foreach (var viewTuple in m_Views) {
                var view = viewTuple.Value.GetComponent<ViewPosition>();
                view.RoomName = Room;
            }
        }

        [Command(requiresAuthority = false)]
        void CmdRequestSync(NetworkConnectionToClient sender = null) {
            foreach(var seatTuple in m_Seats) {
                RpcAttachSeat(sender, seatTuple.Value, seatTuple.Key);
            }
            foreach(var viewTuple in m_Views) {
                RpcAttacView(sender, viewTuple.Value, viewTuple.Key);
            }
        }

        [TargetRpc]
        void RpcAttachSeat(NetworkConnection target, NetworkIdentity seat, int childIndex) {
            AttachSeatInternal(seat.transform, childIndex);
        }

        void AttachSeatInternal(Transform seat, int childIndex) {
            var copy = seat.gameObject.AddComponent<TransformExtension.CopyTransform>();
            copy.Source = m_SeatOffsetGroup.GetChild(childIndex);
            copy.CopyMode = TransformExtension.CopyTransform.Mode.PositionAndRotation;
            copy.CopyEvent = TransformExtension.CopyTransform.Event.LateUpdate;
            copy.CopyTarget = TransformExtension.CopyTransform.Target.FromSource;

            var seatComp = seat.GetComponent<Seatable>();
            if (seatComp != null) {
                seatComp.WebcamPosition.localPosition = new Vector3(0, -0.052f, 1.3f);
            }
        }

        [TargetRpc]
        void RpcAttacView(NetworkConnection target, NetworkIdentity seat, int childIndex) {
            AttachViewInternal(seat.transform, childIndex);
        }

        void AttachViewInternal(Transform seat, int childIndex) {
            var copy = seat.gameObject.AddComponent<TransformExtension.CopyTransform>();
            copy.Source = m_ViewOffsetGroup.GetChild(childIndex);
            copy.CopyMode = TransformExtension.CopyTransform.Mode.PositionAndRotation;
            copy.CopyEvent = TransformExtension.CopyTransform.Event.LateUpdate;
            copy.CopyTarget = TransformExtension.CopyTransform.Target.FromSource;
        }
    }
}