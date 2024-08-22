using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class BF_PlayerMovement : MonoBehaviour
{
    public static BF_PlayerMovement Instance;

    public Camera cam;
    private Rigidbody rb;
    private Quaternion camRot;
    private Vector3 moveDirection;
    private Vector3 inputDirection;

    public float torqueSpeed;
    public float rollSpeed;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        if(cam == null)
        {
            cam = Camera.main;
        }
    }    // Start is called before the first frame update
    void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputDirection = Vector3.zero;
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            inputDirection += new Vector3(0, 0, 1);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            inputDirection += new Vector3(0, 0, -1);
        }
        if (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed)
        {
            inputDirection += new Vector3(1, 0, 0);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            inputDirection += new Vector3(-1, 0, 0);
        }
#else
        if (Input.GetKey(KeyCode.A))
        {
            inputDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputDirection += Vector3.back;
        }
#endif
        MoveBall();
    }

    private void MoveBall()
    {
        camRot = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
        rb.AddTorque(new Vector3(inputDirection.z, 0, -inputDirection.x) * torqueSpeed);
        rb.AddForce(inputDirection * rollSpeed, ForceMode.Force);
    }
}
