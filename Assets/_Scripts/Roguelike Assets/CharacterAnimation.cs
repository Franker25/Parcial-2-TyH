using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public float swayAngle = 10f; 
    public float swayDuration = 0.5f; 
    
    private int _swayDirection = 1; 

    private void Start()
    {
        StartCoroutine(Sway()); 
    }

    private IEnumerator Sway()
    {
        float elapsedTime = 0; 
        var initialRotation = transform.rotation;
        _swayDirection *= -1; 

        
        float targetAngle = _swayDirection * swayAngle; 
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle); 

        while (elapsedTime < swayDuration) 
        {
            elapsedTime += Time.deltaTime; 
            float progress = elapsedTime / swayDuration;
            float easedProgress = EaseInOutQuad(progress); 
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, easedProgress); 
            yield return null; 
        }

        
        elapsedTime = 0; 
        while (elapsedTime < swayDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / swayDuration;
            float easedProgress = EaseInOutQuad(progress); 
            transform.rotation = Quaternion.Lerp(targetRotation, initialRotation, easedProgress);
            yield return null;
        }
        
        
        transform.rotation = initialRotation;
        
        StartCoroutine(Sway());

    }
    
   
    private float EaseInOutQuad(float progress)
    {
        if (progress < 0.5f)
        {
            return 2 * progress * progress;
        }
        else
        {
            return -1 + (4 - 2 * progress) * progress;
        }
    }
}
