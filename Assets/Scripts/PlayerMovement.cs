using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxVelocity;

    private Rigidbody rb;
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
        Vector2 moveInputs = inputs.Player.Movement.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(moveInputs.x, 0f, moveInputs.y);

        if (rb.velocity.magnitude < maxVelocity)
        {
           rb.AddForce(moveVector * speed, ForceMode.VelocityChange);
        }
    }

}
