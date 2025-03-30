using UnityEngine;

namespace PlayerController
{
    public class Player2DPlatformController : MonoBehaviour
    {
        // References
        public Rigidbody2D myRigidbody2D;
        public Animator animator;
        
        //Jump
        [SerializeField] private float jumpStrength = 10f;
        private bool canJump = true;
        
        //walk
        [SerializeField] private float walkspeed = 5f;
        
        //Run
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        private float speed;
        private float input;
        
        //Face turn
        private bool isFacingRight = true;
        private bool isRunning = false;
        
        void Start()
        {
            // Find components if not assigned
            if (myRigidbody2D == null)
                myRigidbody2D = GetComponent<Rigidbody2D>();
                
            if (animator == null)
                animator = GetComponent<Animator>();
                
            speed = walkspeed;
        }
        
        void Update()
        {
            // Get horizontal input
            input = Input.GetAxisRaw("Horizontal");
            
            // Update animation parameters
            bool isMoving = Mathf.Abs(input) > 0.1f;
            animator.SetBool("IsMoving", isMoving);
            
            flip();
            Run();
            
            // Jump when the space key is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        
        private void FixedUpdate()
        {
            // Apply movement
            myRigidbody2D.linearVelocity = new Vector2(input * speed, myRigidbody2D.linearVelocity.y);
        }
        
        private void Jump()
        {
            if (canJump)
            {
                myRigidbody2D.linearVelocity = new Vector2(myRigidbody2D.linearVelocity.x, jumpStrength);
                canJump = false;
            }
        }
        
        private void flip()
        {
            if ((isFacingRight && input < 0f) || (!isFacingRight && input > 0f))
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        
        private void Run()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                speed = Mathf.Lerp(speed, runSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                isRunning = false;
                speed = Mathf.Lerp(speed, walkspeed, deceleration * Time.deltaTime);
            }
            
            // Update the IsRunning parameter in animator
            animator.SetBool("IsRunning", isRunning);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                canJump = true;
            }
        }
    }
}