using COSMOS.Player;
using UnityEngine;

namespace COSMOS.Character {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class CharacterController : MonoBehaviour, IControllable
    {
#warning Need reFactoring
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] float runCycleLegOffset = 0.2f;
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float groundCheckDistance = 0.3f;
        public bool crouch;
        public bool jump;

        Rigidbody rigidbody;
        Animator animator;
        bool isGrounded;
        float origGroundCheckDistance;
        const float k_Half = 0.5f;
        public float TurnAmount;
        public float ForwardAmount;
        public float RightAmount;
        Vector3 groundNormal;
        float capsuleHeight;
        Vector3 capsuleCenter;
        CapsuleCollider capsule;
        bool crouching;
        public float Movement;
        public bool lastUse;


        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
            capsuleHeight = capsule.height;
            capsuleCenter = capsule.center;

            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            origGroundCheckDistance = groundCheckDistance;
        }

        private void FixedUpdate()
        {
            if (lastUse)
            {
                Move(Vector2.zero);
                Rotate(transform.eulerAngles.y);
                UpdateAnimator();
            }
            lastUse = true;

        }

        public bool Exist()
        {
            return this != null;
        }
        public void Move(Vector2 dir)
        {
            Vector3 move = new Vector3(dir.x, 0, dir.y);
            if (move.magnitude > 1f) move.Normalize();
            //move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, groundNormal);
            if (move.magnitude > 0)
            {
                Movement = 1;
            }
            else
            {
                Movement = 0;
            }
            ForwardAmount = move.z;
            RightAmount = move.x;

            if (isGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            ScaleCapsuleForCrouching(crouch);
            PreventStandingInLowHeadroom();

            // send input and other state parameters to the animator
        }
        public Vector3 GetPos()
        {
            return transform.position;
        }
        public void Rotate(float angle)
        {
            float target = (Quaternion.Inverse(transform.rotation) * Quaternion.Euler(0, angle, 0)).eulerAngles.y;
            if (target > 180)
            {
                target -= 360;
            }

            TurnAmount = Mathf.Clamp(target / 10, -1, 1);

            ApplyExtraTurnRotation();
        }
        public void Selected()
        {
            lastUse = false;
            jump = Input.GetKeyDown(KeyCode.Space);
            ForwardAmount = (Input.GetKey(KeyCode.LeftShift) ? ForwardAmount : Mathf.Clamp(ForwardAmount, -0.5f, 0.5f));


            UpdateAnimator();
        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (isGrounded && crouch)
            {
                if (crouching) return;
                capsule.height = capsule.height / 2f;
                capsule.center = capsule.center / 2f;
                crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(rigidbody.position + Vector3.up * capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = capsuleHeight - capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    crouching = true;
                    return;
                }
                capsule.height = capsuleHeight;
                capsule.center = capsuleCenter;
                crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!crouching)
            {
                Ray crouchRay = new Ray(rigidbody.position + Vector3.up * capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = capsuleHeight - capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    crouching = true;
                }
            }
        }


        void UpdateAnimator()
        {
            // update the animator parameters
            animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Right", RightAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Movement", Movement, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", TurnAmount, 0.1f, Time.deltaTime);

            //m_Animator.SetBool("Crouch", m_Crouching);
            //m_Animator.SetBool("OnGround", m_IsGrounded);
            if (!isGrounded)
            {
                //m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * ForwardAmount;
            if (isGrounded)
            {
                //m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            //if (m_IsGrounded && move.magnitude > 0)
            {
                animator.speed = animSpeedMultiplier;
            }
            //else
            {
                // don't use that while airborne
                //m_Animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rigidbody.AddForce(extraGravityForce);

            groundCheckDistance = rigidbody.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            //if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                //m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                //m_IsGrounded = false;
                //m_Animator.applyRootMotion = false;
                //m_GroundCheckDistance = 0.1f;
            }
        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, ForwardAmount);
            transform.Rotate(0, TurnAmount * turnSpeed * Time.deltaTime, 0);
        }


        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (isGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = rigidbody.velocity.y;
                rigidbody.velocity = v;
            }
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
            {
                groundNormal = hitInfo.normal;
                isGrounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                isGrounded = false;
                groundNormal = Vector3.up;
                animator.applyRootMotion = false;
            }
        }

    }
}