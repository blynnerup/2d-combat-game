using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using Scene_Management;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
   private CinemachineVirtualCamera _cinemachineVirtualCamera;
   
   public void SetPlayerCameraFollow()
   {
      _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
      _cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
   }
}
