using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlintOfLight : MonoBehaviour
{
	static readonly float animationDurationSpeedMultiplier = 1;
	static readonly float animationDuration = 0.583f / animationDurationSpeedMultiplier;
	// static readonly float animationFrames = 7;

    void Start()
    {
        Invoke("KILLYOURSELF", animationDuration);
    }

	void KILLYOURSELF()
	{
		Destroy(gameObject);
	}
}
