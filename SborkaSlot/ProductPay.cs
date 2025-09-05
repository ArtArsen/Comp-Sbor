
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public ItemHerecteristic itemToBuy; // �����, ������� ����� ������
    public StelajSlot[] targetSlot; // ����, ���� ����� ������� �����
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
            PlaceItemInSlot(); // ���� ������� �������, �������� ����� � ����
        }
        else
        {
            Debug.Log("��������� �������");
        }
    }

    private void PlaceItemInSlot()
    {
        int itemsToPlace = (itemToBuy.Category == "���") ? 2 : 1; // ���������� ���������� ����������� ��� ����������
        int placedItems = 0; // ������� ����������� �������
        for (int i = 0; i < targetSlot.Length && placedItems < itemsToPlace; i++)
        {
            // ���������, ���� �� ��������� ����
            if (targetSlot[i].transform.childCount == 0)
            {
                GameObject itemObject = Instantiate(itemToBuy.gameObject); // ������� ��������� ������
                itemObject.transform.SetParent(targetSlot[i].transform); // �������� ����� � ����

                // ������������� ������� � ����������� �� ���������
                if (itemToBuy.Category == "���")
                {
                    itemObject.transform.localScale = new Vector3(0.2f, 1f, 0f);
                }
                else
                {
                    itemObject.transform.localScale = Vector3.one;
                }

                placedItems++; // ����������� ������� ����������� �������
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
            money.coinText[0].text = "������: " + coin;
            money.coinText[1].text = "������: " + coin;
            PlayerPrefs.SetInt("Money", coin);
            PlayerPrefs.Save();
            _save.SaveAll();
        }

    }
}