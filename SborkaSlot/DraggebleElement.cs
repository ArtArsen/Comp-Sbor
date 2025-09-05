using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggebleElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private bool isDraggable = true; // Флаг для проверки, может ли элемент быть перетаскиваемым
    private bool isSelected = false; // Флаг для отслеживания выбора элемента

    private void Update()
    {
        // Проверяем, нажата ли клавиша G и выбран ли элемент
        if (isSelected && Input.GetKeyDown(KeyCode.G))
        {
            ItemHerecteristic herect = gameObject.GetComponent<ItemHerecteristic>();
            GameObject canvasObject = GameObject.FindWithTag("Canvas");
            MoneyMeneger money = canvasObject.GetComponent<MoneyMeneger>();
            int coin = PlayerPrefs.GetInt("Money", 0);
            coin += (int)(herect.Price * 0.7);
            money.coinText[0].text = "Баланс: " + coin;
            money.coinText[1].text = "Баланс: " + coin;
            PlayerPrefs.SetInt("Money", coin);
            PlayerPrefs.Save();
            Destroy(gameObject); // Удаляем объект
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // Если элемент не перетаскиваемый, выходим из метода

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        isSelected = true; // Устанавливаем флаг выбора
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // Если элемент не перетаскиваемый, выходим из метода

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // Если элемент не перетаскиваемый, выходим из метода

        // Проверяем, был ли элемент помещен в слот
        if (parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag); // Устанавливаем нового родителя
        }
        else
        {
            // Если элемент не был помещен в слот, возвращаем его на исходную позицию
            transform.SetParent(parentAfterDrag);
        }

        image.raycastTarget = true;
        isSelected = false; // Сбрасываем флаг выбора
    }

    public void SetDraggable(bool draggable) // Метод для установки флага перетаскивания
    {
        isDraggable = draggable;
    }
}
