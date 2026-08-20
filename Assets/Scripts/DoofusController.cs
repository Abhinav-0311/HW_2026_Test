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
            canMove = true;
        }

        public void Stop()
        {
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

            if (transform.position.y < DeathHeight)
            {
                session.EndGame();
            }
        }
    }
}
