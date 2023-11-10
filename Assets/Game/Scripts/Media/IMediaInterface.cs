namespace Game.Media {
    public interface IMediaInterface {
        void StartMedia();
        void StopMedia();
        void OnDesireValueChanged(bool enabled);
        void Initialize();
    }
}