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

    [SerializeField] private AudioSource decompressionAudio;
    [SerializeField] private AudioSource heartBeatAudio;

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

        playerAnimator.SetFloat("Speed", Mathf.Lerp(playerAnimator.GetFloat("Speed"), playerMovement.rb.velocity.magnitude, Time.deltaTime * 10f));
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
        decompressionAudio.Play();
        heartBeatAudio.Play();
        
        DOTween.Sequence()
        .AppendInterval(2f)
        .AppendCallback(() => {
            GameUI.Singleton.TransitionOut();
        })
        .AppendInterval(0.5f)
        .OnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

}
