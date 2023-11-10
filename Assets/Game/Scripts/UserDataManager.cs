namespace Game {
    using UnityEngine;

    public class UserDataManager : MonoBehaviour {
        public int VFXRotation = 16;

        public void SetName(string name) => User.Name = name;
        public void SetVFX(int vfx) => User.VFX = vfx;
        public void PreviousVFX() {
            var old = User.VFX;
            if (old - 1 < 0) {
                User.VFX = VFXRotation - 1;
            } else {
                User.VFX = old - 1;
            }
        }
        public void NextVFX() {
            var old = User.VFX;
            if (old + 1 >= VFXRotation) {
                User.VFX = 0;
            } else {
                User.VFX = old + 1;
            }
        }
    }
}