
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public ItemHerecteristic itemToBuy; // Товар, который нужно купить
    public StelajSlot[] targetSlot; // Слот, куда будет помещен товар
    private int coin;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnBuyButtonClick);
    }


    private void OnBuyButtonClick()
    {
        coin = PlayerPrefs.GetInt("Money");
        if (coin >= itemToBuy.Price)
        {
            PlaceItemInSlot(); // Если покупка успешна, помещаем товар в слот
        }
        else
        {
            Debug.Log("Нехватает средств");
        }
    }

    private void PlaceItemInSlot()
    {
        int itemsToPlace = (itemToBuy.Category == "ОЗУ") ? 2 : 1; // Определяем количество экземпляров для размещения
        int placedItems = 0; // Счетчик размещенных товаров
        for (int i = 0; i < targetSlot.Length && placedItems < itemsToPlace; i++)
        {
            // Проверяем, есть ли свободный слот
            if (targetSlot[i].transform.childCount == 0)
            {
                GameObject itemObject = Instantiate(itemToBuy.gameObject); // Создаем экземпляр товара
                itemObject.transform.SetParent(targetSlot[i].transform); // Помещаем товар в слот

                // Устанавливаем масштаб в зависимости от категории
                if (itemToBuy.Category == "ОЗУ")
                {
                    itemObject.transform.localScale = new Vector3(0.2f, 1f, 0f);
                }
                else
                {
                    itemObject.transform.localScale = Vector3.one;
                }

                placedItems++; // Увеличиваем счетчик размещенных товаров
                if(placedItems == 1)
                {
                    coin -= itemToBuy.Price;
                    PlayerPrefs.SetInt("Money", coin);
                }
            }
        }

        if (placedItems > 0)
        {
            GameObject canvasObject = GameObject.FindWithTag("Canvas");
            SaveLoadManager _save = canvasObject.GetComponent<SaveLoadManager>();
            MoneyMeneger money = canvasObject.GetComponent<MoneyMeneger>();
            coin = PlayerPrefs.GetInt("Money");
            money.coinText[0].text = "Баланс: " + coin;
            money.coinText[1].text = "Баланс: " + coin;
            PlayerPrefs.SetInt("Money", coin);
            PlayerPrefs.Save();
            _save.SaveAll();
        }

    }
}