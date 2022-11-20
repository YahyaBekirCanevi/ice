using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public delegate void InputEvents();
    public event InputEvents OnTap;
    public PlayerInput Input;
    public Vector3 Acceleration;
    UnityEngine.InputSystem.GravitySensor gravitySensor;
    /* public Vector2 TouchPosition; */
    private void Awake()
    {
        Instance = this;
        //540, 960
        Resolution currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width / 2, currentResolution.height / 2, true);

        Input = new PlayerInput();
        Input.Enable();
        if (UnityEngine.InputSystem.GravitySensor.current != null)
        {
            gravitySensor = UnityEngine.InputSystem.GravitySensor.current;
            InputSystem.EnableDevice(gravitySensor);
        }
    }
    private void OnApplicationQuit()
    {
        if (gravitySensor != null)
            InputSystem.DisableDevice(gravitySensor);
    }
    private void Start()
    {
        Input.Player.Tap.performed += Tap;
        /*Input.Player.TouchPosition.performed += Touch; */
#if UNITY_EDITOR ///WASD
        Input.Player.Acceleration.performed += Accelerate;
        Input.Player.Acceleration.canceled += Accelerate;
#endif
    }

    private void Accelerate(InputAction.CallbackContext context)
    {
        Vector2 acc = context.ReadValue<Vector2>();
        Acceleration = new Vector3(acc.x, 0, acc.y);
    }

    private void Update()
    {
#if !UNITY_EDITOR
        Vector3 acc = gravitySensor.gravity.ReadValue();
        /// x => horizontal
        /// y => vertical
        float y = acc.y; //(-1,1)
        y = (y * 2) + 1;  ///(-1, 0) most used part 
        y = Mathf.Clamp(y, -1, 1);
        Acceleration = new Vector3(acc.x, 0, y);
#endif
    }
    public void Tap(InputAction.CallbackContext context)
    {
        if (OnTap != null) OnTap();
    }
    /*  private void Touch(InputAction.CallbackContext context)
     {
         TouchPosition = context.ReadValue<Vector2>();
     } */
}
