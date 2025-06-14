using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Transform cameraTransform;
	Vector3 lastCameraPosition;
	[SerializeField] float parallaxEffectMultiplier;
	
    void Start()
    {
        cameraTransform = Camera.main.transform;
		lastCameraPosition = cameraTransform.position;
    }

    
    void LateUpdate()
    {
        Vector3 changeInCameraPosition = cameraTransform.position - lastCameraPosition;
		transform.position += changeInCameraPosition * parallaxEffectMultiplier;
		lastCameraPosition = cameraTransform.position;
    }
}
