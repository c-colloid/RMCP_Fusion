using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour, IPlayerJoined
{
    [Header("Avatar Configuration")]
    [SerializeField] private NetworkPrefabRef PlayerPrefab;
    [SerializeField] private SyncMode syncMode = SyncMode.Inharitence; // Default to Inherit from RMCprotocol

    private enum SyncMode
    {
        Inharitence = -1, // Inherit from RMCprotocol
        RPC = 0,
        Streaming = 1
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var playerObject = Runner.Spawn(PlayerPrefab, Vector3.up, Quaternion.identity);
            if (syncMode >= 0)playerObject.GetComponent<RMCprotocol>().SelectedSyncMode = (RMCprotocol.SyncMode)(int)syncMode; // Set the sync mode for the local player

            Debug.Log($"Local avatar spawned for player {player}");
        }
    }
}