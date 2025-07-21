# Remote Motion Capture Protocol (RMCP) for Unity
## Description
This is a fork of [RMCP_Dokkyo](https://github.com/shun-irie/RMCP_Dokkyo) that supports PhotonFusion2 shared mode.

# How to install RMCP
In this repository, you can install RMCP using expanded Editor functions in Unity.
## Requirements
| OS | Unity version |
|---|---|
| Windows 10 and 11| later 2021.3.45.f1 |
| Mac OS later 13.4.1| later 2021.3.45.f1 |
| Android (>API level 22)| later 2021.3.45.f1 |

* Not using development build for Android.
* For iOS or WebGL, the system requirements for Photon Server includes these environments but I never tried. Please tell me while you succeeded to build in iOS and WebGL.
* Asset Serialization MUST be set to text (Project Settings > Editor > Asset Serialization, Mode = Force Text)

### 1. Install unity package file
The unity package file is available [here](https://github.com/c-colloid/RMCP_Fusion/raw/main/RemoteMotionCaptureProtocol.unitypackage).
### 2. Import Photon Fusion via Unity Asset Store or Package Manager
This unity package requires Photon Fusion from Photon Engine for shared mode networking. After installation of Photon Fusion, you must input the App ID for Photon Fusion.
Import Options:

- Option A: Get Photon Fusion from [Unity Asset Store (FREE)](https://assetstore.unity.com/packages/tools/network/photon-fusion-267958?locale=ja-JP)
- Option B: Download the Fusion SDK from [Photon Engine's SDK Download page](https://doc.photonengine.com/fusion/current/getting-started/sdk-download) and import via Assets > Import Package > Custom Package

#### Setup Process:

1. After importing Photon Fusion, the Fusion Hub wizard will automatically appear
1. Create a new Photon Fusion App ID at the [Photon Engine Dashboard](https://dashboard.photonengine.com)
   * Click "Create a New App"
   * Select "Fusion" in the Select Photon SDK dropdown
   * Select "Fusion 2" in the Select SDK Version dropdown
   * Fill out the form and click "Create"

3. Copy the generated App ID from the dashboard
1. Insert the App ID in the Fusion Hub Welcome tab that appeared in Unity
> [!IMPORTANT]
> Configure your project for **Shared Mode** topology, as this system uses cloud rooms with shared state authority rather than client-host architecture

### 3. Import unity package on Unity
You can import the unitypackage file from Assets>Import package>Custom package.
### 4. Create Server Prefab
At first, you should create server prefab from the Menu (RMC>Create Server Prefab).
### 5. Import avatar object with motion capture scripts
You should import and add avatar object with motion capture scripts. The avatar rig must be set to humanoid.
### 6. Avatar prefab settings
First, you should select the avatar object and run the auto setting function (RMC>Auto Setting Avatar Prefab). After setting the prefab, you should delete the avatar object from hierarchy. The avatar prefab is stored in Assets/Resources. In the inspector, you should set sampling frequency, the source animator, and root position of an avatar in the component of RMCprotocol.
### 7. Deactivate setting for motion capture scripts in remote place
Sometimes, the motion capture scripts interfered the other motion capture system. Thus, the motion capture scripts must be deactivated in the remote places. You can deactivate the motion capture scripts in remote places using the component of Controller Script and input the names of MonoBehaviours related to motion capture functions.
### 8. ServerPrefab settings
Last, you should modify the ServerPrefab in the hierarchy, such as room name and avatar prefab name in the resource folder. Then, you can build remote motion capture environments!!

The detail information about API should be refered [here](https://github.com/c-colloid/RMCP_Fusion/blob/main/API.md).
