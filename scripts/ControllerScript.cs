using Fusion;
using UnityEngine;

public class ControllerScript : NetworkBehaviour
{
    [Header("Motion Capture Scripts Control")]
    // 対象のスクリプトを保持するリスト
    public MonoBehaviour[] motionCaptureScripts;

    // 接続時に呼び出されるメソッド
    public override void Spawned()
    {
        // このObjectが自分のものでない場合
        if (!Object.HasStateAuthority)
        {
            DisableMotionCaptureScripts();
        }
    }

    private void DisableMotionCaptureScripts()
    {
        foreach (MonoBehaviour motionCaptureScript in motionCaptureScripts)
        {
            Debug.Log($"Found script: {motionCaptureScript.GetType().Name} (enabled: {motionCaptureScript.enabled})");
            
            if (motionCaptureScript != null)
            {
                Debug.Log($"Disabling script: {motionCaptureScript}");
                motionCaptureScript.enabled = false;
                Debug.Log($"Script {motionCaptureScript} disabled: {motionCaptureScript.enabled}");
            }
        }
    }
}
