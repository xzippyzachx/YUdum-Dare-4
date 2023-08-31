using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public enum PlayerState {
        Alive,
        Dead,
        Pause
    }
    public PlayerState playerState { get; private set; }

    public static Player player { get; private set; }

    public PlayerMovement playerMovement { get; private set; }

    [field:SerializeField] public Animator playerAnimator { get; private set; }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = this;
    }

    private void Update()
    {
        if (playerMovement.inputs.Player.Tether.triggered)
        {
            TetherManager.Singleton.AttemptPlaceTetherPole(transform.position);
        }

        playerAnimator.SetFloat("Speed", playerMovement.rb.velocity.magnitude);
        if (playerMovement.moveVector.magnitude != 0f)
        {
            playerAnimator.transform.rotation = Quaternion.Slerp(playerAnimator.transform.rotation, Quaternion.LookRotation(playerMovement.moveVector), Time.deltaTime * 10f);
        }
    }

    public void Die()
    {
        playerState = PlayerState.Dead;
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Die");
        
        DOTween.Sequence()
        .AppendInterval(1.5f)
        .AppendCallback(() => {
            GameUI.Singleton.TransitionOut();
        })
        .AppendInterval(0.5f)
        .OnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

}
