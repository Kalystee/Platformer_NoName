using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour {
    
	void Update ()
    {
        this.transform.forward = Camera.main.transform.forward;
	}
}
