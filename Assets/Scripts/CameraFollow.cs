using UnityEngine;

namespace DoofusAdventure
{
    public sealed class CameraFollow : MonoBehaviour
    {
        private readonly Vector3 offset = new Vector3(0f, 14f, -14f);
        private Transform target;

        public void Initialize(Transform followTarget)
        {
            target = followTarget;
            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, target.position + offset, 6f * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }
}
