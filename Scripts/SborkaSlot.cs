using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SborkaSlot : MonoBehaviour, IDropHandler
{
    public string allowedCategory = "Материнская плата"; // Укажите категорию, которая разрешена
    public GridLayoutGroup gridLayoutGroup; // Ссылка на компонент GridLayoutGroup

    private Dictionary<string, string> categoryKeys = new Dictionary<string, string>
    {
        { "Процессор", "Процессор" },
        { "Материнская Плата", "Материнская Плата" },
        { "Куллер", "Куллер" },
        { "Видеокарта", "Видеокарта" },
        { "SSDN2", "SSDN2" },
        { "SSDsata", "SSDsata" },
        { "ОЗУ", "ОЗУ" },
        { "Блок питания", "Блок питания" }
    };


    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggebleElement draggebleElement = dropped.GetComponent<DraggebleElement>();
            ItemHerecteristic itemCharacteristic = dropped.GetComponent<ItemHerecteristic>();

            // Проверяем наличие необходимых компонентов
            if (itemCharacteristic != null && itemCharacteristic.Category == allowedCategory)
            {
                draggebleElement.parentAfterDrag = transform;
                dropped.transform.SetParent(transform); // Перемещаем объект в слот
                dropped.transform.localScale = Vector2.one;
                draggebleElement.SetDraggable(false);

                if (itemCharacteristic.Category == "Процессор")
                {
                    CPUcode _cpu = dropped.GetComponent<CPUcode>();
                    _cpu.coolSlot.gameObject.SetActive(true);
                }
                // Сохраняем данные в PlayerPrefs
                if (categoryKeys.TryGetValue(itemCharacteristic.Category, out string key))
                {
                    // Сохраняем цену в PlayerPrefs
                    PlayerPrefs.SetInt(key + "_price", itemCharacteristic.Price);

                    if (itemCharacteristic.Category == "ОЗУ")
                    {
                        int total = PlayerPrefs.GetInt(key);
                        total += 1;
                        PlayerPrefs.SetInt(key, total);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(key, 1);
                    }
                }
            }
            else
            {
                Debug.Log("Этот объект не может быть помещен в слот сборки."); // Сообщение для отладки
            }
        }
    }
}
