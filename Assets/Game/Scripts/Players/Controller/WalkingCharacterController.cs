namespace Game.Controller.Player {
    using Game.CMF;
    using UnityEngine;
    using Game.Player;
    using Game.TransformExtension;

    public class WalkingCharacterController : IPlayerController {
        public WalkingCharacter Character { get; private set; }
        [SerializeField] CopyTransform Attacher;

        [Header("Controllers")]
        [SerializeField] WalkerController MovementController;
        [SerializeField] CameraController CameraController;

        public override void AssignObject(GameObject obj) {
            Character = obj.GetComponent<WalkingCharacter>();

            // Update controller setting
            MovementController.Mover = Character.Mover;
            MovementController.cameraTransform = Character.HeadOrigin;
            CameraController.TargetTransform = Character.HeadOrigin;

            // Enable all controllers
            MovementController.enabled = true;
            CameraController.enabled = true;

            // Attach to character
            Attacher.Source = Character.HeadOrigin;
        }
    }
}