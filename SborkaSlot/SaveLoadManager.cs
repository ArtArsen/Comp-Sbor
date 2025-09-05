using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject[] itemPrefabs; // ������ �������� ��� ��������� ��������� ���������
    public InventorySlot[] inventorySlots; // ������ ����� ���������
    public Shelf[] shelves; // �������� ������ ���������

    private void Start()
    {
        LoadAll(); // ��������� ��� ��� ������
    }

    public void SaveAll()
    {
        SaveInventory(inventorySlots); // ��������� ���������
        foreach (var shelf in shelves)
        {
            shelf.SaveShelves(shelves); // ��������� ��������
        }
    }

    public void LoadAll()
    {
        LoadInventory(inventorySlots); // ��������� ���������
        foreach (var shelf in shelves)
        {
            shelf.LoadShelves(shelves); // ��������� ��������
        }
    }

    public void SaveInventory(InventorySlot[] inventorySlots)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount > 0)
            {
                string category = inventorySlots[i].transform.GetChild(0).GetComponent<ItemHerecteristic>().Category;
                PlayerPrefs.SetString("InventorySlot_" + i, category);
            }
            else
            {
                PlayerPrefs.SetString("InventorySlot_" + i, "Empty");
            }
        }

        PlayerPrefs.Save();
    }

    public void LoadInventory(InventorySlot[] inventorySlots)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            string category = PlayerPrefs.GetString("InventorySlot_" + i, "Empty");
            if (category != "Empty")
            {
                GameObject newItem = CreateItem(category);
                if (newItem != null)
                {
                    newItem.transform.SetParent(inventorySlots[i].transform);
                    ItemHerecteristic itemCharacteristic = newItem.GetComponent<ItemHerecteristic>();
                    if (itemCharacteristic.Category == "���")
                    {
                        newItem.transform.localScale = new Vector3(0.2f, 1f, 0f);
                    } 
                    else
                    {
                        newItem.transform.localScale = Vector3.one;
                    }
                    
                    newItem.transform.localPosition = Vector3.zero; // ������������� ������� � ����� ������
                }
            }
        }
    }

    public GameObject CreateItem(string category)
    {
        // ������� ������ ������� �� ���������
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[i].GetComponent<ItemHerecteristic>().Category == category)
            {
                // ������� ��������� ������� � ���������� ���
                return Instantiate(itemPrefabs[i]);
            }
        }

        Debug.LogWarning("������ ��� ��������� " + category + " �� ������.");
        return null;
    }


}