using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MiniGame.SnowMan
{
    public class LookCamera : MonoBehaviour
    {
        Camera cam;

        [SerializeField]
        bool PerfectRotation = false;

        void Update()
        {
            if (cam == null)
                cam = Camera.main;
            if (cam != null)
            {
                if (PerfectRotation)
                    transform.rotation = cam.transform.rotation;
                else
                    transform.LookAt(cam.transform);
            }
        }
    }
}