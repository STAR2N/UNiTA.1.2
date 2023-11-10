using UnityEngine;

public class SeatableInteractor : MonoBehaviour
{
    [SerializeField] Game.Seatable seatable;
    [SerializeField] KeyCode standKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(standKey))
        {
            if (seatable != null)
            {
                seatable.CmdTryStand();
            }
        }
    }

    public void TrySit()
    {
        if (seatable != null)
        {
            seatable.CmdTrySit();
        }
    }
}
