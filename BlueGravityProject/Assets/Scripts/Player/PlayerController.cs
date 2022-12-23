using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AnimController animController;
    [SerializeField] SkinController skin;
    [SerializeField] Transform castCenter;
    public int money = 9999;
    [SerializeField][Range(0, 5)] float movementSpeed;
    Vector3 movement;
    Vector3 lastDirection;

    int lastFace;
    public bool canMove = true;
    Clothing hatToBuy;
    Clothing clothToBuy;
    BuyCloth buyCloth;

    // This was pre-made
    public int facing()
    {
        if (movement == Vector3.down)
        {
            lastFace = 0;
            return 0;
        }
        if (movement == Vector3.up)
        {
            lastFace = 1;
            return 1;
        }
        if (movement == Vector3.right)
        {
            lastFace = 2;
            return 2;
        }
        if (movement == Vector3.left)
        {
            lastFace = 3;
            return 3;
        }
        return lastFace;
    }

    // Some of this was pre-made
    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }
        movement = context.ReadValue<Vector2>();
        if (context.performed)
        {
            if (movement.sqrMagnitude != 0)
            {
                animController.PlayAnimation(AnimationClips.Walk, facing(), skin.activeHat.GetTypeAnimationClip(), skin.activeCloth.GetTypeAnimationClip());
            }
        }
        if (context.canceled)
        {
            animController.PlayAnimation(AnimationClips.Idle, facing(), skin.activeHat.GetTypeAnimationClip(), skin.activeCloth.GetTypeAnimationClip());
        }
    }

    // Some of this was pre-made
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit2D hit = Physics2D.BoxCast(castCenter.position + lastDirection / 2, Vector2.one, 0, lastDirection, 0, LayerMask.GetMask("Interactable"));
            if (hit.transform != null)
            {
                if (hit.transform.tag.Contains("NPC"))
                {

                    hit.transform.GetComponent<NPCController>().Talk();
                }
                if (hit.transform.tag.Contains("Cloth"))
                {
                    buyCloth = hit.transform.GetComponent<BuyCloth>();
                    hatToBuy = buyCloth.hat;
                    clothToBuy = buyCloth.cloth;
                    buyCloth.BuyText();
                }
            }
        }
    }

    // Checks what the player wants to buy and ensures that it's possible
    public void BuyCloth(int option)
    {
        if (option == 0)
        {
            clothToBuy = skin.activeCloth;
        }
        if (option == 1)
        {
            hatToBuy = skin.activeHat;
        }

        int totalSpending = 0;

        if (!hatToBuy.owned)
        {
            totalSpending += hatToBuy.price;
        }
        if (!clothToBuy.owned)
            totalSpending += clothToBuy.price;

        if (money - totalSpending < 0)
        {
            UIController.Instance.CantBuy();
            return;
        }
        money -= totalSpending;
        UIController.Instance.SetMoney();
        buyCloth.SetOwned(option);
        hatToBuy.owned = true;
        clothToBuy.owned = true;
        skin.ChangeSkin(hatToBuy, clothToBuy, () => { animController.PlayAnimation(AnimationClips.Idle, facing(), skin.activeHat.GetTypeAnimationClip(), skin.activeCloth.GetTypeAnimationClip()); });
    }

    // This was pre-made
    void Walk()
    {
        if (movement != Vector3.zero && Mathf.Abs(movement.x) == 0 || Mathf.Abs(movement.x) == 1)
        {
            lastDirection = movement;
        }
        Vector3 target = transform.position + movement;
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
    }

    // Sets the initial money on the UI and the default skin
    void Start()
    {
        skin.ChangeSkin(skin.activeHat, skin.activeCloth, () =>
        {
            animController.PlayAnimation(AnimationClips.Idle, facing(), skin.activeHat.GetTypeAnimationClip(), skin.activeCloth.GetTypeAnimationClip());
        });
        UIController.Instance.SetMoney();
    }

    void FixedUpdate()
    {
        Walk();
    }

    // I use this to debug where the interaction box is facing, also pre-made
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(castCenter.position + lastDirection / 2, Vector3.one);
    }
}
