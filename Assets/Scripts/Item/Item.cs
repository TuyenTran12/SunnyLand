using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp,Examine }
    // PickUp: Pick up the item into the bag
    // Examine: Show item information
    public enum ItemType { Staic, Consumables }
    // Staic: Cannot be consumed ( Thường là các đối tượng cố định, không thể di chuyển hoặc tiêu thụ. )
    // Consumables: Consumable ( Đây là các vật phẩm có thể sử dụng và biến mất sau khi sử dụng. )

    public InteractionType InteractType;
    public ItemType type;

    [Header("Examine")]
    public string descriptionText;
    Sprite image;

    [Header("Custom Event")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }
    public void Interact()
    {
        switch(InteractType)
        {
            case InteractionType.PickUp:
                FindObjectOfType<InventorySystem>().PickedUpItem(gameObject);
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;
            default:
                break;
        }
        // call the custom event
        customEvent.Invoke();
    }
}
