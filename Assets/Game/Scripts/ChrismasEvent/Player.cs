// using Cinemachine;
// using Photon.Pun;
using UnityEngine;

namespace Game.MiniGame.SnowMan
{
    // public class Player : MonoBehaviourPun
    public class Player : MonoBehaviour
    {
        // const int ROTATE_MOUSE_BUTTON = 0;
        // bool enableRotation = false;
        // public bool EnableRotation
        // {
        //     get => enableRotation;
        //     set
        //     {
        //         enableRotation = value;
        //         Cursor.lockState = enableRotation ? CursorLockMode.Locked : CursorLockMode.None;
        //     }
        // }
        // public float MouseSensitivity = 2;
        // // [SerializeField]
        // // CinemachineVirtualCamera vcam;
        // [SerializeField]
        // Transform body;
        // [SerializeField]
        // Transform head;

        // float vertical;

        // private void Start()
        // {
        //     vcam.Priority = photonView.IsMine ? 10 : 0;
        //     if (TryGetComponent(out global::CMF.Controller controller))
        //     {
        //         controller.enabled = photonView.IsMine;
        //     }
        //     if (TryGetComponent(out global::CMF.Mover mover))
        //     {
        //         mover.enabled = photonView.IsMine;
        //     }
        // }

        // private void Update()
        // {
        //     if (photonView.IsMine)
        //     {
        //         var pressing = Input.GetMouseButton(ROTATE_MOUSE_BUTTON);
        //         if (pressing != EnableRotation)
        //         {
        //             EnableRotation = pressing;
        //         }
        //         UpdateRotation();
        //     }
        // }

        // private void OnDestroy()
        // {
        //     if (photonView.IsMine)
        //     {
        //         if (EnableRotation == true)
        //             EnableRotation = false;
        //     }
        // }

        // private void UpdateRotation()
        // {
        //     if (!EnableRotation)
        //         return;
        //     body.localRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * MouseSensitivity, Vector3.up);
        //     vertical = Mathf.Clamp(vertical - Input.GetAxis("Mouse Y"), -89.9f, 89.9f);
        //     head.localRotation = Quaternion.AngleAxis(vertical, Vector3.right);
        // }

        // private void OnTriggerEnter(Collider other)
        // {
            
        // }
        // private void OnTriggerExit(Collider other)
        // {
            
        // }
    }
}