using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LetterSpawner : NetworkBehaviour
{
    public GameObject LetterMasterPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Vector3 Pos = new Vector3(0, 0, 0);
        Quaternion Rot = new Quaternion(0, 0, 0, 1);
        ServerSpawnLetterMaster(Pos, Rot);
    }

    [Server]
    public void ServerSpawnLetterMaster(Vector3 pos, Quaternion rot)
    {
        GameObject LetterMaster = Instantiate(LetterMasterPrefab, pos, rot);
        NetworkServer.Spawn(LetterMaster);
    }
}
