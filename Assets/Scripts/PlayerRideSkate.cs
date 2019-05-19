using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSkateSimulator {
    public class PlayerRideSkate : MonoBehaviour {

        public GameObject skate;

        public bool overSkate;

        private Quaternion smoothTurnQuaternion;
        public float smoothValue;

        // Use this for initialization
        void Start() {
            overSkate = true;
            smoothTurnQuaternion = transform.rotation;
        }

        // Update is called once per frame
        void Update() {
            if (overSkate) {
                smoothTurnQuaternion = Quaternion.RotateTowards(smoothTurnQuaternion, skate.transform.rotation, smoothValue * Time.deltaTime);
                transform.position = skate.transform.position;
                transform.eulerAngles = new Vector3(smoothTurnQuaternion.eulerAngles.x, skate.transform.eulerAngles.y, smoothTurnQuaternion.eulerAngles.z);
            }
        }
    }
}
