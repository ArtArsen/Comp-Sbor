using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public StelajSlot[] shelfSlots; // Ячейки на стеллаже
    public GameObject[] itemPrefabs; // Массив префабов для различных категорий предметов
    public TextMeshProUGUI RepText;

    // Другие свойства и методы для работы со стеллажом

    public void SaveShelves(Shelf[] shelves)
    {
        for (int s = 0; s < shelves.Length; s++)
        {
            for (int i = 0; i < shelves[s].shelfSlots.Length; i++)
            {
                if (shelves[s].shelfSlots[i].transform.childCount > 0)
                {
                    string category = shelves[s].shelfSlots[i].transform.GetChild(0).GetComponent<ItemHerecteristic>().Category;
                    PlayerPrefs.SetString("Shelf_" + s + "_Slot_" + i, category);
                }
                else
                {
                    PlayerPrefs.SetString("Shelf_" + s + "_Slot_" + i, "Empty");
                }
            }
        }

        PlayerPrefs.Save();
    }

    public GameObject ShopPanel;
    public GameObject OrderPanel;
    public GameObject SborkaPanel;
    public Button SborkaPanelButton;
    private void FixedUpdate()
    {
        if(SborkaPanel != null)
        {
            SborkaPanelButton.gameObject.SetActive(true);
        }
        else
        {
            SborkaPanelButton.gameObject.SetActive(false);
        }
    }
    public void ShopPanelOn()
    {
        ShopPanel.SetActive(true);
        ShopPanel.transform.SetAsLastSibling();
    }

    public void ShopPanelOff()
    {
        ShopPanel.SetActive(false);
    }
    public void SborkaPanellOn()
    {
        SborkaPanel.SetActive(true);
    }
    public void LoadShelves(Shelf[] shelves)
    {
        for (int s = 0; s < shelves.Length; s++)
        {
            for (int i = 0; i < shelves[s].shelfSlots.Length; i++)
            {
                string category = PlayerPrefs.GetString("Shelf_" + s + "_Slot_" + i, "Empty");
                if (category != "Empty")
                {
                    SaveLoadManager _saveload = FindObjectOfType<SaveLoadManager>();
                    GameObject newItem = _saveload.CreateItem(category);
                    if (newItem != null)
                    {
                        float Irep = PlayerPrefs.GetFloat("TotalRep");
                        RepText.text = "Рейтинг: " + Irep;
                        newItem.transform.SetParent(shelves[s].shelfSlots[i].transform);
                        ItemHerecteristic itemCharacteristic = newItem.GetComponent<ItemHerecteristic>();
                        if (itemCharacteristic.Category == "Видеокарта" || itemCharacteristic.Category == "Куллер")
                        {
                            SmenaKart smena = newItem.GetComponent<SmenaKart>();
                            smena.image.sprite = smena.prefab1;
                        }
                        
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
    }

}
