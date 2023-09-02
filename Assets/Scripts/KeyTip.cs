using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTip : MonoBehaviour
{
    [SerializeField] private GameObject keyboard;
    [SerializeField] private GameObject controller;

    private void Update()
    {
        if (Player.player.playerMovement.playerInput.currentControlScheme == "keyboard")
        {
            keyboard.SetActive(true);
            controller.SetActive(false);
        }
        else
        {
            keyboard.SetActive(false);
            controller.SetActive(true);
        }
    }
}
