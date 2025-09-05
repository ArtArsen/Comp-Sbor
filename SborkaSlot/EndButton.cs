using TMPro;
using UnityEngine;
using System;

public class EndButton : MonoBehaviour
{
    public TMP_InputField inputField; // Ссылка на InputField
    public GameObject InputPanel;
    public TextMeshProUGUI errorText;

    private void Start()
    {
        // Инициализация значений PlayerPrefs при первом запуске
        for (int i = 0; i < 25; i++)
        {
            if (!PlayerPrefs.HasKey($"IsOver{i + 1}"))
            {
                PlayerPrefs.SetInt($"IsOver{i + 1}", 0); // Устанавливаем значение по умолчанию
            }
        }
    }

    // Объявляем переменные для компонентов
    private readonly string[] componentKeys = {
        "Процессор", "Материнская Плата", "Куллер", "Видеокарта", "SSDN2", "SSDsata", "ОЗУ", "Блок питания"
    };
    private readonly int[] Bibliotecprice =
    {
        70000, 90000, 120000
    };

    private readonly string[] positiveReviews = {
        "Комп собрал сбалансированный, советую. Оценка: ",
        "Сборщик хорошо поработал, комп топ, всем рекомендую. Оценка: ",
        // Добавьте другие положительные отзывы
    };

    private readonly string[] negativeReviews = {
        "Заказывал комп с бюджетом в 100к, но мне прислали какой-то дешманский комп. Оценка: ",
        "Ком плохой, сборщик научись подбирать комплектующие. Оценка: ",
        // Добавьте другие отрицательные отзывы
    };

    public void Proverka()
    {
        // Получаем количество и цену для каждого компонента
        int[] componentCounts = new int[componentKeys.Length];
        int[] componentPrices = new int[componentKeys.Length];
        for (int i = 0; i < componentKeys.Length; i++)
        {
            componentCounts[i] = PlayerPrefs.GetInt(componentKeys[i]);
            componentPrices[i] = PlayerPrefs.GetInt(componentKeys[i] + "_price");
            Debug.Log($"{componentKeys[i]}: {componentCounts[i]}");
        }

        if (componentCounts[0] == 1 && componentCounts[1] == 1 && componentCounts[2] == 1 &&
            componentCounts[3] == 1 &&
            (componentCounts[4] == 1 || componentCounts[5] == 1 || (componentCounts[4] + componentCounts[5] == 2)) &&
            componentCounts[6] == 2 && componentCounts[7] == 1)
        {
            PlayerPrefs.SetInt("Allsbor", 1);
            int totalPrice = 0;
            for (int i = 0; i < componentCounts.Length; i++)
            {
                totalPrice += componentCounts[i] * componentPrices[i];
            }
            PlayerPrefs.SetInt("Pricesbor", totalPrice);
            Debug.Log(totalPrice);
        }
    }

    public void InputPanelOn()
    {
        InputPanel.SetActive(true);
    }

    public void InputPanelOff()
    {
        InputPanel.SetActive(false);
    }

    public void SborkaPanelOff()
    {
        gameObject.SetActive(false);
        GameObject canvasObject = GameObject.FindWithTag("Canvas");
        Shelf sheif = canvasObject.GetComponent<Shelf>();
        sheif.SborkaPanel = gameObject;
    }

    public void OnSubmit()
    {
        // Получаем текст из TMP_InputField
        string userInput = inputField.text;

        // Пробуем преобразовать текст в число
        if (float.TryParse(userInput, out float number) && number > 0 && number <= 25)
        {
            Proverka();
            HandleOrder((int)number - 1); // Вызываем обработчик заказа, передавая индекс (0-24)
        }
        else
        {
            errorText.text = "Вы ввели что-то не то."; // Обновляем текст в TMP_Text
        }
    }

    private void HandleOrder(int orderNumber)
    {
        int isOver = PlayerPrefs.GetInt($"IsOver{orderNumber + 1}");
        int all = PlayerPrefs.GetInt("Allsbor");


        if (isOver == 0)
        {
            if (all == 1)
            {
                int price = PlayerPrefs.GetInt("Pricesbor");
                if (price > Bibliotecprice[orderNumber])
                {
                    Polojitelno(positiveReviews[orderNumber], orderNumber); // Используем положительный отзыв
                }
                else
                {
                    Oticatelno(negativeReviews[orderNumber], orderNumber); // Используем отрицательный отзыв
                }
                PlayerPrefs.SetInt($"IsOver{orderNumber + 1}", 1);
            }
            else
            {
                errorText.text = "Не все компоненты собраны.";
            }
        }
        else
        {
            errorText.text = "Этот заказ уже был выполнен.";
        }
    }

    public void Polojitelno(string otziv, int orderNomer)
    {
        ProcessOrder(otziv, orderNomer, true);
    }

    public void Oticatelno(string otziv, int orderNomer)
    {
        ProcessOrder(otziv, orderNomer, false);
    }

    private void ProcessOrder(string otziv, int orderNomer, bool isPositive)
    {
        GameObject peopleObject = GameObject.FindWithTag($"{orderNomer + 1}");
        GameObject canvasObject = GameObject.FindWithTag("Canvas");
        PeopleMasage _people = peopleObject.GetComponent<PeopleMasage>();
        ScrollControl canvas = canvasObject.GetComponent<ScrollControl>();
        Shelf sheif = canvasObject.GetComponent<Shelf>();

        int currentRep = isPositive ? 5 : UnityEngine.Random.Range(1, 5);
        PlayerPrefs.SetInt($"Rep_{orderNomer}", currentRep);
        string orderText = otziv + currentRep; // Используем индекс для доступа к массиву
        canvas.OrderText[orderNomer] = orderText;
        PlayerPrefs.SetString($"OrderKeys_{orderNomer}", orderText);
        _people.MasageTxt.text = orderText;

        Rep(); // Обновляем репутацию
        float Irep = PlayerPrefs.GetFloat("TotalRep");
        sheif.RepText.text = "Рейтинг: " + Irep;

        // Удаляем все ключи в цикле
        foreach (var key in componentKeys)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(key + "_price");
        }

        PlayerPrefs.DeleteKey("Allsbor");
        PlayerPrefs.DeleteKey("Pricesbor");

        // Сохранение всех данных после завершения заказа
        SaveLoadManager saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager != null)
        {
            saveLoadManager.SaveAll(); // Сохраняем все данные
        }

        MoneyMeneger moneyText = canvasObject.GetComponent<MoneyMeneger>();
        int money = PlayerPrefs.GetInt("Money");
        money += (Bibliotecprice[orderNomer] + 30000);
        moneyText.coinText[0].text = "Баланс: " + money;
        moneyText.coinText[1].text = "Баланс: " + money;
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
        errorText.text = "";
        Destroy(gameObject);
    }

    public void Rep()
    {
        int[] repCurrent = new int[25];
        for (int i = 0; i < 25; i++)
        {
            repCurrent[i] = PlayerPrefs.GetInt($"Rep_{i}");
        }

        int count = 0;
        int sum = 0;

        // Считаем сумму и количество допустимых значений
        for (int i = 0; i < 25; i++)
        {
            if (repCurrent[i] > 0 && repCurrent[i] <= 5) // Проверяем, что значение в допустимом диапазоне
            {
                sum += repCurrent[i];
                count++; // Увеличиваем счетчик, если значение допустимо
            }
        }

        // Вычисляем среднее арифметическое от всех 25 значений
        double totalAverage = (double)sum / 25;

        // Округляем до одного знака после запятой
        float rounded = (float)Math.Round(totalAverage, 1);

        PlayerPrefs.SetFloat("TotalRep", rounded);
        Debug.Log(rounded);
    }
}