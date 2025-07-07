using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [Header("Avatar Configuration")]
    [SerializeField] private NetworkPrefabRef PlayerPrefab;
    
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, Vector3.up, Quaternion.identity);
            Debug.Log($"Local avatar spawned for player {player}");
        }
    }
}
