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

    public Rigidbody rb;
    public PlayerMovementController instance = null;

    public float jumpForce = 200;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;

    protected float m_JumpStart;
    protected bool m_Jumping;

    protected bool m_Moving;
    protected bool m_Sliding;
    protected float m_SlideStart;
    protected bool backward;
    protected bool left;
    protected bool right;
    protected bool origin;
    // public CharacterCollider characterCollider;
    public Animator animator;
    static int s_JumpingHash = Animator.StringToHash("PolyAnim|Run_Jump");

    static int s_JumpingSpeedHash = Animator.StringToHash("PolyAnim|Run_Forward");
    static int s_left = Animator.StringToHash("PolyAnim|Run_Left");
    static int s_right = Animator.StringToHash("PolyAnim|Run_Right");
    static int s_SlidingHash = Animator.StringToHash("PolyAnim|Run_Backward");
    static int s_jump_origin = Animator.StringToHash("jump_origin");
    //static int s_MovingHash = Animator.StringToHash("Moving");

    public float jumpHeight = 1.2f;

    public float jumpLength = 2.0f;

    protected const float k_TrackSpeedToJumpAnimSpeedRatio = 0.6f;
    public float slideLength = 2.0f;


    private float m_Timer;
    private float m_Timer2;
    private int m_Second;
    private int gravity;
    //protected const float k_GroundingSpeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -10;

        //gravity = -35;//-35;
        Physics.gravity = new Vector3(0, gravity, 0);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        }

        //fpsCamera.transform.RotateAround(rb.transform.position, Vector3.up, 10);
        fpsCamera.transform.Translate(new Vector3(-5f, 1.5f, 7f));//-5f, 0f, 7f

        m_Sliding = false;
        m_SlideStart = 0.0f;

        
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
    {       //Calculate movement velocity as a 3D vector

        Vector3 verticalTargetPosition = rb.position;
        if (velocity != Vector3.zero)
        {
            //Vector3 direction = rb.position - transform.position; 
            // direction.x =90; 
            //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // targetRotation = new Vector3(0, 90, 0);
            // rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation , Time.deltaTime * speed));
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            anim.SetInteger("Walk", 1);
            m_Moving = true;
        }
        else
        {
            anim.SetTrigger("idel");
            anim.SetInteger("Walk", 0);
            m_Moving = false;
        }

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            gravity = 0;

            //gravity = -35;//-35;
            Physics.gravity = new Vector3(0, gravity, 0);
            //  Physics.gravity.false;
            verticalTargetPosition.y = Mathf.Sin(Mathf.PI) * jumpHeight;
            canJump = Time.time + timeBeforeNextJump;
            rb.velocity += new Vector3(0, 5, 0);
            rb.AddForce(0, jumpForce, 0);
            //Physics.gravity = new Vector3(0, gravity, 0);

            anim.SetTrigger("falldown");
            Invoke("fall", 0.3f);
             //anim.SetTrigger("TurnLeft");
            anim.SetTrigger("Jump");
            anim.SetTrigger("falldown");
           Jump();
            m_Jumping = true;
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity += new Vector3(0, 1, 0);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            anim.SetInteger("Walk", 1);
            m_Moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("left");
            anim.SetBool("RunLeft", true);
            left = true;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("right");
            anim.SetBool("RunRight", true);
            right = true;
        }


        else if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("Backward");
            anim.SetBool("RunBackward",true);
            anim.SetInteger("Back", 1);
            backward = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //anim.SetTrigger("RunLeft");

            anim.SetTrigger("TurnLeft");
            anim.SetBool("RunLeft", true);
            animator.SetBool(s_left, true);

            //animator.SetBool(s_left, false);


            
            left = true;
            //Slide();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //anim.SetTrigger("RunRight");
            // Slide();

            anim.SetTrigger("TurnRight");
            animator.SetBool(s_right, true);
           // anim.SetBool("RunRight", true);
            right = true;
            //animator.SetBool(s_right, false);
            //Slide();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            /*verticalTargetPosition.y = Mathf.Sin(Mathf.PI) * jumpHeight;
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("fall"); 
            Jump();

            m_Jumping = true;*/


            // anim.SetTrigger("jump_origin");
            //animator.SetBool(s_jump_origin, true);
            //origin = true;
            gravity = -10;

            //gravity = -35;//-35;
            Physics.gravity = new Vector3(0, gravity, 0);
            //  Physics.gravity.false;
            verticalTargetPosition.y = Mathf.Sin(Mathf.PI) * jumpHeight;
            canJump = Time.time + timeBeforeNextJump;
            rb.velocity += new Vector3(0, 15, 0);
            rb.AddForce(0, jumpForce, 0);
            //Physics.gravity = new Vector3(0, gravity, 0);

            //anim.SetTrigger("falldown");
            Invoke("fall", 0.3f);
            //anim.SetTrigger("TurnLeft");
            anim.SetTrigger("Jump2");
            //anim.SetTrigger("falldown");
            //Jump();
           // m_Jumping = true;



        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetTrigger("Backward");
            anim.SetBool("RunBackward", true);
            //anim.SetTrigger("RunBackward");
            animator.SetBool(s_SlidingHash, true);
            //anim.SetBool("RunBackward", true);
            backward = true;
            //if (!m_Sliding)
              //  Slide();
        }
       Invoke("stop", 1.0f);

        if (m_Sliding)
        {
            StopSliding();
            // Slide time isn't constant but the slide length is (even if slightly modified by speed, to slide slightly further when faster).
            // This is for gameplay reason, we don't want the character to drasticly slide farther when at max speed.
            float correctSlideLength = slideLength * (1.0f + speed);
            float ratio = (1.0f - m_SlideStart) / correctSlideLength;
            if (ratio >= 1.0f)
            {
                // We slid to (or past) the required length, go back to running
                StopSliding();
            }
        }

        if (m_Jumping)
        {
            m_Jumping = false;
            if (m_Moving)
            {
                // Same as with the sliding, we want a fixed jump LENGTH not fixed jump TIME. Also, just as with sliding,
                // we slightly modify length with speed to make it more playable.
                float correctJumpLength = jumpLength * (1.0f + speed);
                float ratio = (1.0f - m_JumpStart) / correctJumpLength;
                if (ratio >= 1.0f)
                {
                    m_Jumping = false;
                    animator.SetBool(s_JumpingHash, false);
                }
                else
                {
                    verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
                }
            }
            else
            {
                //verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, k_GroundingSpeed * Time.deltaTime);
                if (Mathf.Approximately(verticalTargetPosition.y, 0f))
                {
                    animator.SetBool(s_JumpingHash, false);
                    m_Jumping = false;
                }
            }

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
    public void StopSliding()
    {
        if (m_Sliding)
        {
            animator.SetBool(s_SlidingHash, false);
            m_Sliding = false;

           // characterCollider.Slide(false);
        }
    }

    void fall()
    {

        anim.SetTrigger("falldown");
        gravity = -30;//-35;
        Physics.gravity = new Vector3(0, gravity, 0);
        //rb.AddForce(0, jumpForce, 0);
    }
    public void Slide()
    {
        if (!m_Sliding)
        {
            float correctSlideLength = slideLength * (1.0f + speed);
            m_SlideStart = 1;
            float animSpeed = k_TrackSpeedToJumpAnimSpeedRatio * (speed / correctSlideLength);

            animator.SetFloat(s_JumpingSpeedHash, animSpeed);
            animator.SetBool(s_SlidingHash, true);
            m_Sliding = true;

           // characterCollider.Slide(true);
        }
    }
    public void Jump()
    {
        if (!m_Jumping)
        {
            if (m_Sliding)
                StopSliding();

            float correctJumpLength = jumpLength * (1.0f + speed);
            m_JumpStart = 1;
            float animSpeed = k_TrackSpeedToJumpAnimSpeedRatio * (speed / correctJumpLength);

            animator.SetFloat(s_JumpingSpeedHash, animSpeed);
            animator.SetBool(s_JumpingHash, true);
            m_Jumping = true;
        }
    }

    void stop()
    {
        //m_Timer2 = m_Timer;
        //m_Timer += Time.deltaTime;
        //m_Second = (int)m_Timer;

        if (backward)
        {
            //if (m_Timer - m_Timer2 >= 1.0f)
            {
                backward = false;
                anim.SetBool("RunBackward", false);
                animator.SetBool(s_SlidingHash, false);
            }

        }
        if (right)
        {
            //if (m_Timer - m_Timer2 >= 1.0f)
            {
                right = false;
                anim.SetBool("RunRight", false);
                animator.SetBool(s_right, false);
            }
        }
        if (left)
        {
            //if (m_Timer - m_Timer2 >= 1.0f)
            {
                left = false;
                anim.SetBool("RunLeft", false);
                animator.SetBool(s_left, false);
            }
        }
        if(origin)
        {

            origin = false;

            anim.SetTrigger("jump_origin");
            animator.SetBool(s_jump_origin, false);
        }
    }
}
