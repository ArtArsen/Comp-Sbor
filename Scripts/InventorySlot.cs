using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ItemHerecteristic itemCharacteristic = dropped.GetComponent<ItemHerecteristic>();
            DraggebleElement draggebleElement = dropped.GetComponent<DraggebleElement>();
            draggebleElement.parentAfterDrag = transform;
            dropped.transform.localScale = Vector3.one;

            if (itemCharacteristic.Category == "ОЗУ")
            {
                dropped.transform.localScale = new Vector3(0.2f, 1f, 0f);
            }
            else if (itemCharacteristic.Category == "Видеокарта" || itemCharacteristic.Category == "Куллер")
            {
                SmenaKart smena = dropped.GetComponent<SmenaKart>();
                smena.image.sprite = smena.prefab1;
            }
        }
    }


}
