using UnityEngine;

namespace Game.MiniGame.SnowMan
{
    public class RotateY : MonoBehaviour
    {
        [SerializeField]
        float SpeedPerSecond = 30f;

        void Update()
        {
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * SpeedPerSecond, Vector3.up);
        }
    }
}