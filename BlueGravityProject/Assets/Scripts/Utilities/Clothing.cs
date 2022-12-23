using UnityEngine;

// Stores the information of each cloth
public class Clothing : MonoBehaviour
{
    public Color color;
    public Wear wear;
    public Material material;
    public int price;
    public bool owned;

    public string GetTypeAnimationClip()
    {
        switch (wear)
        {
            case Wear.ClothSuit:
            case Wear.HatSuit:
                return AnimationClips.Suit;
            default:
                return AnimationClips.Default;
        }

    }
}
