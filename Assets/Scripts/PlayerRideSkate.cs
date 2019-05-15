using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSkateSimulator {
    public class PlayerRideSkate : MonoBehaviour {

        public GameObject skate;

        public bool overSkate;

        // Use this for initialization
        void Start() {
            overSkate = true;
        }

        // Update is called once per frame
        void Update() {
            if (overSkate) {
                transform.position = skate.transform.position;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, skate.transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }
}
