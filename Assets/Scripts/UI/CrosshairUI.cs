using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CrosshairUI : MonoBehaviour
{
    public RectTransform Reticle;
    public CharacterController CharacterController;

    public float OriginalSize;
    public float TargetSize;
    
    private float currentSize;
    
    private void Update()
    {
        // todo：腰射时准心扩散
        // 走路时准心扩散
        bool isMoving = CharacterController.velocity.magnitude > 0;
        if (isMoving)
        {
            currentSize = Mathf.Lerp(currentSize, TargetSize, Time.deltaTime * 5);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, OriginalSize, Time.deltaTime * 5);
        }

        Reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }
}
