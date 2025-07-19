using Fusion;
using UnityEngine;
using Meta.XR.MultiplayerBlocks.Fusion;

public class PlayerSpawner : SimulationBehaviour, IDespawned, IPlayerJoined
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
    
    private void Awake()
    {
        FusionBBEvents.OnConnectedToServer += RegisterOnRunner;
    }

    public void RegisterOnRunner(NetworkRunner runner)
    {
        // var runner = NetworkRunner.GetRunnerForGameObject(gameObject);
        if (runner.IsRunning)
        {
            runner.AddGlobal(this);
        }
    }
    
    public void Despawned(NetworkRunner runner, bool hasState)
    {
        // var runner = NetworkRunner.GetRunnerForGameObject(gameObject);
        if (runner.IsRunning)
        {
            runner.RemoveGlobal(this);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var playerObject = Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity);
            if (syncMode >= 0)playerObject.GetComponent<RMCprotocol>().SelectedSyncMode = (RMCprotocol.SyncMode)(int)syncMode; // Set the sync mode for the local player

            Debug.Log($"Local avatar spawned for player {player}");
        }
    }
}