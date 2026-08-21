using UnityEngine;

namespace DoofusAdventure
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class DoofusController : MonoBehaviour
    {
        private const float Gravity = -24f;
        private const float DeathHeight = -12f;

        private CharacterController characterController;
        private GameSession session;
        private float movementSpeed;
        private float verticalVelocity;
        private bool canMove;

        public void Initialize(GameSession gameSession, float speed)
        {
            session = gameSession;
            movementSpeed = speed;
            characterController = GetComponent<CharacterController>();
            canMove = false;
        }

        public void SetMovementEnabled(bool isEnabled)
        {
            canMove = isEnabled;
            if (!isEnabled)
            {
                verticalVelocity = 0f;
            }
        }

        public void ResetToStartPosition()
        {
            transform.position = new Vector3(0f, 1f, 0f);
            verticalVelocity = 0f;
            canMove = false;
        }

        private void Update()
        {
            if (!canMove || characterController == null)
            {
                return;
            }

            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            verticalVelocity += Gravity * Time.deltaTime;
            var motion = (input * movementSpeed) + (Vector3.up * verticalVelocity);
            characterController.Move(motion * Time.deltaTime);
            RegisterPulpitBelowDoofus();

            if (transform.position.y < DeathHeight)
            {
                session.EndGame();
            }
        }

        private void RegisterPulpitBelowDoofus()
        {
            var origin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(origin, Vector3.down, out var hit, 1.25f))
            {
                var pulpit = hit.collider.GetComponent<Pulpit>();
                if (pulpit != null)
                {
                    session.RegisterPulpitReached(pulpit);
                }
            }
        }
    }
}
