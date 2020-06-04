using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    public float speed = 8f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    GameObject fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float CameraUpAndDownRotation = 0f;
    private float CurrentCameraUpAndDownRotation = 0f;

    private Rigidbody rb;
    public PlayerMovementController instance = null;

    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (instance==null)
        {
            instance = this;
        }

        //fpsCamera.transform.RotateAround(rb.transform.position, Vector3.up, 10);
        fpsCamera.transform.Translate(new Vector3(-5f, 0f, 7f));
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _xMovement = Input.GetAxis("Horizontal");
        float _zMovement = Input.GetAxis("Vertical");

        Vector3 _movementHorizontal = (transform.right) * _xMovement;
        Vector3 _movementVertical = transform.forward * _zMovement;

        //Final movement velocity
        Vector3 _movementVelocity = (_movementHorizontal + _movementVertical).normalized * speed;

        //Apply movement
        Move(_movementVelocity);

        //calculate rotation as a 3D vector for turning around
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotationVector = new Vector3(0, _yRotation, 0) * lookSensitivity;

        //Apply rotation
        Rotate(_rotationVector);

        //Calculate look up and down camera rotation 
        float _cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;


        //Apply rotation 
        RotateCamera(_cameraUpDownRotation);

    }

    //runs per physics iteration
    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            //Vector3 direction = rb.position - transform.position; 
           // direction.x =90; 
            //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // targetRotation = new Vector3(0, 90, 0);
           // rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation , Time.deltaTime * speed));
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            anim.SetInteger("Walk", 1);
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("Jump");
        }


        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (fpsCamera != null)
        {
            CurrentCameraUpAndDownRotation -= CameraUpAndDownRotation;
            CurrentCameraUpAndDownRotation = Mathf.Clamp(CurrentCameraUpAndDownRotation, 0, 0);//-85
            fpsCamera.transform.localEulerAngles = new Vector3(CurrentCameraUpAndDownRotation, 0, 0);
            //fpsCamera.transform.Translate(new Vector3(-4f, 0f, 5f));
        }
    }

    void Move(Vector3 movementVelocity)
    {
        velocity = movementVelocity;
    }

    void Rotate(Vector3 rotationVector)
    {
        rotation = rotationVector;
    }

    void RotateCamera(float cameraUpDownRotation)
    {
        CameraUpAndDownRotation = cameraUpDownRotation;
    }
}