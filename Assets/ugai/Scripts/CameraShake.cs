using System.Collections;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace ugai
{

    public class CameraShake : MonoBehaviour
    {
        public void Shake(float duration, float magnitude)
        {
            StartCoroutine(DoShake(duration, magnitude));
        }

        private IEnumerator DoShake(float duration, float magnitude)
        {
            var pos = transform.localPosition;

            var elapsed = 0f;

            while (elapsed < duration)
            {
                var x = pos.x + Random.Range(-1f, 1f) * magnitude;
                var z = pos.z + Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, pos.y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = pos;
        }
    }
}