using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

// I divided the character into multiple layers, this controllers coordinates all of them
public class AnimController : MonoBehaviour
{
    [SerializeField] Animator hat;
    [SerializeField] Animator cloth;
    [SerializeField] Animator body;

    public void PlayAnimation(string clipName, int facing, string hatType, string clothType)
    {
        // If the player is moving left, it plays the animation moving right and flips the object using its scale
        if (facing == 3)
        {
            facing = 2;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.y);
        }
        // If not moving left, resets the scale to always be 1 in the x axis, which is the one used to flip the object
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.y);
        }


        hat.Play(clipName + facing.ToString() + hatType);
        cloth.Play(clipName + facing.ToString() + clothType);
        body.Play(clipName + facing.ToString() + AnimationClips.Default);
    }
}
