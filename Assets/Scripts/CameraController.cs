using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float tilt = 1f;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform cam;
    private Vector3 cameraOffset;
    private Vector3 cameraDirection;
    float time = 0;
    Vector3 acc;
    private void Start()
    {
        cameraOffset = cam.localPosition;
        cameraDirection = cam.localRotation.eulerAngles;
    }
    private void LateUpdate()
    {
        FollowPlayer();
    }
    // lineer interpolation between current position and players position
    private void FollowPlayer()
    {
        transform.position = player.transform.position;

        time += Time.deltaTime * movementSpeed *
            (player.Velocity == Vector3.zero ? -1 : 1);
        time = Mathf.Clamp(time, 0, 1);

        acc = Vector3.Lerp(acc, InputManager.Instance.Acceleration, Time.deltaTime * 8f);

        cam.localPosition = Vector3.Lerp(cameraOffset,
            cameraOffset + (acc * -moveDistance), time);

        Vector3 tilted = new Vector3(player.Velocity.normalized.z, player.Velocity.normalized.x, 0) * tilt;
        Vector3 eulerAngles = Vector3.Lerp(cameraDirection, cameraDirection + tilted, time);
        cam.localRotation = Quaternion.Euler(eulerAngles);
    }
}
