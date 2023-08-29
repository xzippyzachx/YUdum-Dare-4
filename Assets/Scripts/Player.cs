using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement.inputs.Player.Tether.triggered)
        {
            TetherManager.Singleton.AttemptPlaceTetherPole(transform.position, this);
        }
    }

    

}
