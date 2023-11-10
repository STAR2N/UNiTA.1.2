using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MiniGame.SnowMan
{
    public class FollowCamera : MonoBehaviour
    {
        private Camera cam;
        public Camera Camera
        {
            get
            {
                if (cam == null)
                    cam = Camera.main;
                return cam;
            }
        }

        private void Update()
        {
            var cam = Camera;
            if (cam != null) {
                transform.position = cam.transform.position;
            }
        }
    }
}