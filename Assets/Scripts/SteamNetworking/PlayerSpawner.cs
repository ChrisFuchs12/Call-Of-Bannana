using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public override void OnStartClient()
    {
        SpawnPlayer(LocalConnection);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayer(NetworkConnection conn)
    {
        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        ServerManager.Spawn(go, conn);
    }
}
