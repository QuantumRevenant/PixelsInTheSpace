using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class Item_Script : MonoBehaviour
{
    public ItemData itemData;

    void Update()
    {
        RefreshItem();
        Move(itemData.ItemSpeed);
    }
    
    void Move(float speed)
    {
        Vector3 movementVector=-transform.up*speed*Time.deltaTime;
        transform.Translate(movementVector,Space.Self);
    }
    void RefreshItem()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite=itemData.ItemSprite;
    }

}
