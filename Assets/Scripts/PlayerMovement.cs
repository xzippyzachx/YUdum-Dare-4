using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxVelocity;

    public Rigidbody rb { get; private set; }
    public InputMaster inputs { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputs = new InputMaster();
    }

    public void OnEnable()
    {
        inputs.Enable();
    }

    public void OnDisable()
    {
        inputs.Disable();
    }
    
    private void FixedUpdate()
    {
        if (Player.player.playerState != Player.PlayerState.Alive)
        {
            return;
        }

        Vector2 moveInputs = inputs.Player.Movement.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(moveInputs.x, 0f, moveInputs.y);

        if (rb.velocity.magnitude < maxVelocity)
        {
           rb.AddForce(moveVector * speed, ForceMode.Force);
        }
    }

}
