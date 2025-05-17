using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

namespace PlayerController
{
    public class Player2DPlatformController : MonoBehaviour
    {
        // References
        public Rigidbody2D myRigidbody2D;
        public Animator animator;
        TouchingDirections touchingDirections;
        private AudioManager audioManager; // ✅ AudioManager reference

        // Jump
        [SerializeField] private float jumpStrength = 10f;
        private bool canJump = true;
        public Damage playerDamage; // Reference to Damage script

        // Walk & Run
        [SerializeField] private float walkspeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        private float speed;
        private bool isRunning = false;

        // Input
        private Vector2 movementInput;

        // Face turn
        private bool isFacingRight = true;
        public float jumpImpulse = 10f;
        public bool IsFacingRight => isFacingRight;


        void Start()
        {
            if (myRigidbody2D == null)
                myRigidbody2D = GetComponent<Rigidbody2D>();

            if (animator == null)
                animator = GetComponent<Animator>();

            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // ✅ Initialize AudioManager

            speed = walkspeed;
        }

        void Update()
        {
            bool isMoving = Mathf.Abs(movementInput.x) > 0.1f;
            animator.SetBool(AnimationStrings.isMoving, isMoving);

            flip();
            Run();
        }

        private void FixedUpdate()
        {
            myRigidbody2D.linearVelocity = new Vector2(movementInput.x * speed, myRigidbody2D.linearVelocity.y);
            animator.SetFloat(AnimationStrings.yVelocity, myRigidbody2D.linearVelocity.y);
        }

        private void Jump()
        {
            if (canJump)
            {
                myRigidbody2D.linearVelocity = new Vector2(myRigidbody2D.linearVelocity.x, jumpStrength);
                canJump = false;

                animator.SetTrigger(AnimationStrings.jump);
                myRigidbody2D.linearVelocity = new Vector2(myRigidbody2D.linearVelocity.x, jumpImpulse);

                // ✅ Play jump sound
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.jump);
                }
            }
        }

        private void flip()
        {
            if ((isFacingRight && movementInput.x < 0f) || (!isFacingRight && movementInput.x > 0f))
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        private void Run()
        {
            if (isRunning)
            {
                speed = Mathf.Lerp(speed, runSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, walkspeed, deceleration * Time.deltaTime);
            }

            animator.SetBool(AnimationStrings.isRunning, isRunning);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                canJump = true;
            }
        }

        // 👇 New Input System methods below this line

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();

            // When player presses down, find and notify platforms
            if (context.performed && movementInput.y < -0.5f)
            {
                NotifyPlatformsOfDownInput(context);
            }
        }

        // This method will help connect the player's down input to the platform
        private void NotifyPlatformsOfDownInput(InputAction.CallbackContext context)
        {
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                ContactPoint2D[] contacts = new ContactPoint2D[10]; // Adjust array size as needed
                int contactCount = playerCollider.GetContacts(contacts);

                for (int i = 0; i < contactCount; i++)
                {
                    GameObject contactObject = contacts[i].collider.gameObject;
                    PlatformPassThrough platform = contactObject.GetComponent<PlatformPassThrough>();

                    if (platform != null)
                    {
                        platform.OnDownInput(context);
                        break;
                    }
                }
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Jump();
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            isRunning = context.ReadValueAsButton();
        }
        
    }
}
