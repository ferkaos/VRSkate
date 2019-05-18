using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public bool withRotation = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = objectToFollow.transform.position;
        if (withRotation) {
            this.transform.rotation = objectToFollow.transform.rotation;
        }
    }
}
