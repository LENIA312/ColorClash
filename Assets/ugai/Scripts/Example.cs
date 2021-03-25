using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace ugai
{
    public class Example : MonoBehaviour
    {
        public CameraShake shake;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                shake.Shake(0.25f, 0.5f);
            }
        }
    }
}