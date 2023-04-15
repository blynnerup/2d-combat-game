using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            
            UiFade.Instance.FadeToClear();
        }
    }
}
