using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the player's clothes
public class SkinController : MonoBehaviour
{
    [SerializeField] SpriteRenderer hat;
    [SerializeField] SpriteRenderer cloth;
    public Clothing activeHat;
    public Clothing activeCloth;

    // Sets new clothes, changes the animation using a callback to avoid referencing multiple times the animation controller
    public void ChangeSkin(Clothing hatType, Clothing clothType, Action SetAnimation = null)
    {
        activeHat = hatType;
        activeCloth = clothType;

        hat.material = hatType.material;
        cloth.material = clothType.material;

        hat.color = hatType.color;
        cloth.color = clothType.color;

        if (SetAnimation != null)
        {
            SetAnimation();
        }
    }
}
