using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggebleElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private bool isDraggable = true; // ���� ��� ��������, ����� �� ������� ���� ���������������
    private bool isSelected = false; // ���� ��� ������������ ������ ��������

    private void Update()
    {
        // ���������, ������ �� ������� G � ������ �� �������
        if (isSelected && Input.GetKeyDown(KeyCode.G))
        {
            ItemHerecteristic herect = gameObject.GetComponent<ItemHerecteristic>();
            GameObject canvasObject = GameObject.FindWithTag("Canvas");
            MoneyMeneger money = canvasObject.GetComponent<MoneyMeneger>();
            int coin = PlayerPrefs.GetInt("Money", 0);
            coin += (int)(herect.Price * 0.7);
            money.coinText[0].text = "������: " + coin;
            money.coinText[1].text = "������: " + coin;
            PlayerPrefs.SetInt("Money", coin);
            PlayerPrefs.Save();
            Destroy(gameObject); // ������� ������
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // ���� ������� �� ���������������, ������� �� ������

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        isSelected = true; // ������������� ���� ������
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // ���� ������� �� ���������������, ������� �� ������

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // ���� ������� �� ���������������, ������� �� ������

        // ���������, ��� �� ������� ������� � ����
        if (parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag); // ������������� ������ ��������
        }
        else
        {
            // ���� ������� �� ��� ������� � ����, ���������� ��� �� �������� �������
            transform.SetParent(parentAfterDrag);
        }

        image.raycastTarget = true;
        isSelected = false; // ���������� ���� ������
    }

    public void SetDraggable(bool draggable) // ����� ��� ��������� ����� ��������������
    {
        isDraggable = draggable;
    }
}
