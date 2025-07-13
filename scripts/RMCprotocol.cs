using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPC.MoCap;
using System;
using Fusion;
using Fusion.Sockets;

public class RMCprotocol : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Header("Motion Capture Settings")]
    public float framesPerSecond = 30f;
    public Animator sourceAnimator;  // Made public to be set in the Inspector
    public Transform rootTransform;
    
    private float timePerFrame;
    private float timer;

    private ReliableKey motionDataKey;
    
    private Dictionary<PlayerRef, byte[]> receivedMotionData = new Dictionary<PlayerRef, byte[]>();

    public override void Spawned()
    {
        if (sourceAnimator == null)
        {
            Debug.LogError("Source Animator is not set in the Inspector.");
            return;
        }

        timePerFrame = 1f / framesPerSecond;
        timer = 0f;

        motionDataKey = ReliableKey.FromInts((int)Object.Id.Raw, 1, 0, 0);

        Runner.AddCallbacks(this);

        Debug.Log($"Spawned for player {Object.StateAuthority}, key: {motionDataKey}");
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (runner != null)
        {
            runner.RemoveCallbacks(this);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && sourceAnimator != null)
        {
            timer += Runner.DeltaTime;
            if (timer >= timePerFrame)
            {
                timer -= timePerFrame;
                SendMotionData();
            }
        }
    }

    private void SendMotionData()
    {
        try
        {
            var coombinedData = calc_funcs.GetByteArrayFromAnimatorState(sourceAnimator, rootTransform);

            foreach (var player in Runner.ActivePlayers)
            {
                if (player != Object.StateAuthority)
                {
                    Runner.SendReliableDataToPlayer(player, motionDataKey, coombinedData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send motion data: {e.Message}");
        }
    }

    public override void Render()
    {
        if (!Object.HasStateAuthority && Object.StateAuthority.IsRealPlayer)
        {
            if (receivedMotionData.TryGetValue(Object.StateAuthority, out byte[] data))
            {
                try
                {
                    calc_funcs.SetAnimatorStateFromByteArray(sourceAnimator, rootTransform, data);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to set animator state from player {Object.StateAuthority}: {e.Message}");
                }
            }
        }
    }


    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        if (key.Equals(motionDataKey))
        {
            byte[] motionData = new byte[data.Count];
            Array.Copy(data.Array, data.Offset, motionData, 0, data.Count);
            receivedMotionData[Object.StateAuthority] = motionData;

            Debug.Log($"Received motion data from {player}: {data.Count} bytes");
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        if (key.Equals(motionDataKey) && progress < 1.0f)
        {
            Debug.Log($"Motion data transfer progress from {player}: {progress *  100:F1}%");
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnSceneLoadDone(NetworkRunner runner){}
    public void OnSceneLoadStart(NetworkRunner runner){}
}