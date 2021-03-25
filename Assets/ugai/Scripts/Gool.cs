using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class Gool : MonoBehaviour
    {

        public GameObject GoalText;

        // Use this for initialization
        void Start()
        {

            GoalText.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player") {
                GoalText.SetActive(true);
            }
        }

    }

}