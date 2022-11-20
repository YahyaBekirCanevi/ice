using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float dashForce = 24f;
    [SerializeField] private float moveSpeed = 16f;
    [SerializeField] private float speedLimit = 30f;
    [SerializeField] private float turnSpeed = 12f;
    public TrailRenderer trail;
    public RectTransform speed;
    public TMPro.TMP_Text speedValue;
    private Rigidbody rb;
    public Vector3 Velocity { get => rb.velocity; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Rotate();
        speed.localRotation = Quaternion.Euler(0, 0,
            -Quaternion.LookRotation(Velocity).eulerAngles.y + 180);
        speedValue.text = Velocity.magnitude.ToString("0");

        if (Velocity.magnitude > speedLimit)
        {
            rb.velocity = Velocity.normalized * speedLimit;
        }
    }
    private void FixedUpdate()
    {
        if (InputManager.Instance.Acceleration.magnitude > 0.06f)
            Move();
    }
    // look with acceleration
    private void Rotate()
    {
        if (InputManager.Instance.Acceleration == Vector3.zero) return;
        transform.forward = Vector3.Lerp(transform.forward, InputManager.Instance.Acceleration, Time.deltaTime * turnSpeed);
    }
    /// backward dash
    private void Dash()
    {
        trail.enabled = false;
        float distance = dashForce;
        if (Physics.Raycast(transform.position, transform.forward * dashForce, out RaycastHit hit, dashForce))
        {
            distance = (hit.distance - 1);
        }
        Vector3 force = transform.forward * distance;
        transform.position += force;
        float speed = rb.velocity.magnitude;
        rb.velocity = transform.forward.normalized * speed;
        trail.enabled = true;
    }
    //move by acceleration
    private void Move()
    {
        rb.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        rb.velocity = Vector3.Lerp(Velocity, Vector3.zero, Time.deltaTime * .05f);
    }
    private void OnEnable()
    {
        InputManager.Instance.OnTap += Dash;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnTap -= Dash;
    }
}
