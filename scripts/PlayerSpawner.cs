using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Meta.XR.MultiplayerBlocks.Fusion;

public class PlayerSpawner : SimulationBehaviour, IDespawned, IPlayerJoined
{
    [Header("Avatar Configuration")]
    [SerializeField] private NetworkPrefabRef PlayerPrefab;
    
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
            Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log($"Local avatar spawned for player {player}");
        }
    }
}
