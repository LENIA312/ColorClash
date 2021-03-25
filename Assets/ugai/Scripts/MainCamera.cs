using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uagi
{

    public class MainCamera : MonoBehaviour
    {
        public float movementSpeed;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.gameObject.transform.Translate(0, 0, movementSpeed);
        }
    }

}