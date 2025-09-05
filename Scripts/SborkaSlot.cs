using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SborkaSlot : MonoBehaviour, IDropHandler
{
    public string allowedCategory = "����������� �����"; // ������� ���������, ������� ���������
    public GridLayoutGroup gridLayoutGroup; // ������ �� ��������� GridLayoutGroup

    private Dictionary<string, string> categoryKeys = new Dictionary<string, string>
    {
        { "���������", "���������" },
        { "����������� �����", "����������� �����" },
        { "������", "������" },
        { "����������", "����������" },
        { "SSDN2", "SSDN2" },
        { "SSDsata", "SSDsata" },
        { "���", "���" },
        { "���� �������", "���� �������" }
    };


    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggebleElement draggebleElement = dropped.GetComponent<DraggebleElement>();
            ItemHerecteristic itemCharacteristic = dropped.GetComponent<ItemHerecteristic>();

            // ��������� ������� ����������� �����������
            if (itemCharacteristic != null && itemCharacteristic.Category == allowedCategory)
            {
                draggebleElement.parentAfterDrag = transform;
                dropped.transform.SetParent(transform); // ���������� ������ � ����
                dropped.transform.localScale = Vector2.one;
                draggebleElement.SetDraggable(false);

                if (itemCharacteristic.Category == "���������")
                {
                    CPUcode _cpu = dropped.GetComponent<CPUcode>();
                    _cpu.coolSlot.gameObject.SetActive(true);
                }
                // ��������� ������ � PlayerPrefs
                if (categoryKeys.TryGetValue(itemCharacteristic.Category, out string key))
                {
                    // ��������� ���� � PlayerPrefs
                    PlayerPrefs.SetInt(key + "_price", itemCharacteristic.Price);

                    if (itemCharacteristic.Category == "���")
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
                Debug.Log("���� ������ �� ����� ���� ������� � ���� ������."); // ��������� ��� �������
            }
        }
    }
}
