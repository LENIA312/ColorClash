using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace matsushima
{

    public class CM_Maneger : MonoBehaviour
    {
        int timer;

        // Start is called before the first frame update
        void Start()
        {
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            timer++;
        }
    }

}