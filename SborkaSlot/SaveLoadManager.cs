using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Массив префабов для различных категорий предметов
    public InventorySlot[] inventorySlots; // Массив ячеек инвентаря
    public Shelf[] shelves; // Добавьте массив стеллажей

    private void Start()
    {
        LoadAll(); // Загружаем все при старте
    }

    public void SaveAll()
    {
        SaveInventory(inventorySlots); // Сохраняем инвентарь
        foreach (var shelf in shelves)
        {
            shelf.SaveShelves(shelves); // Сохраняем стеллажи
        }
    }

    public void LoadAll()
    {
        LoadInventory(inventorySlots); // Загружаем инвентарь
        foreach (var shelf in shelves)
        {
            shelf.LoadShelves(shelves); // Загружаем стеллажи
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
                    if (itemCharacteristic.Category == "ОЗУ")
                    {
                        newItem.transform.localScale = new Vector3(0.2f, 1f, 0f);
                    } 
                    else
                    {
                        newItem.transform.localScale = Vector3.one;
                    }
                    
                    newItem.transform.localPosition = Vector3.zero; // Устанавливаем позицию в центр ячейки
                }
            }
        }
    }

    public GameObject CreateItem(string category)
    {
        // Находим индекс префаба по категории
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[i].GetComponent<ItemHerecteristic>().Category == category)
            {
                // Создаем экземпляр префаба и возвращаем его
                return Instantiate(itemPrefabs[i]);
            }
        }

        Debug.LogWarning("Префаб для категории " + category + " не найден.");
        return null;
    }


}