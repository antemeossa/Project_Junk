using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BlackMarketListElement : MonoBehaviour
{

    [SerializeField] private Image blackMarketItemImg;
    [SerializeField] private Image rarityBorder;
    [SerializeField] private TextMeshProUGUI blackMarketItemText;
    [SerializeField] private int reqItemAmount;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyBtn, sellBtn;
    [SerializeField] private Slider activeSlider;
    [SerializeField] private TextMeshProUGUI sliderMaxText, sliderCurrentText;

    public int itemCost;
    public bool isActive;
    public itemTypes itemType;

    private int buyMaxValue = 1000;
    private int sellMaxValue;
    private CraftRecipe blackMarketItemRecipe;


    private void Start()
    {
        activeSlider.maxValue = buyMaxValue;
        sliderMaxText.text = "" + buyMaxValue;
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        //updateActiveSlider();
    }
    public void setBlackMarketItemDetails(CraftRecipe rec,
        Sprite blackMarketItemImg, Rarity rarity, itemTypes cntrctName, int priceAmount)
    {
        blackMarketItemRecipe = rec;
        this.blackMarketItemImg.sprite = blackMarketItemImg;
        blackMarketItemText.text = Utils.enumToString(cntrctName);
        priceText.text = "ITEM PRICE: " + priceAmount;
        itemCost = priceAmount;
        setRarityBorder(rarity, rarityBorder);
        itemType = rec.outputProduct.outputType;
    }


    private void setRarityBorder(Rarity rarity, Image img)
    {
        switch (rarity)
        {
            case Rarity.Primitive:
                img.color = Color.gray;
                break;
            case Rarity.Standard:
                img.color = Color.yellow;
                break;
            case Rarity.Advanced:
                img.color = Color.green;
                break;
            case Rarity.Prototype:
                img.color = Color.cyan;
                break;
            case Rarity.CuttingEdge:
                img.color = Color.magenta;
                break;
            default: break;
        }



    }

    public void acceptOnClick()
    {

        isActive = true;
        transform.DOScale(1.1f, .5f);
        transform.DOScale(1f, .5f);
        sellBtn.gameObject.SetActive(false);
        activeSlider.gameObject.SetActive(true);
        buyBtn.gameObject.SetActive(false);
    }


    
    

    public void calculateCurrentPrice()
    {
        priceText.text = "ITEM PRICE: " + itemCost * (int)activeSlider.value;
        sliderCurrentText.text = activeSlider.value + "";
    }
    public void buyBtnOnClick()
    {
        activeSlider.maxValue -= activeSlider.value;
        sliderMaxText.text = activeSlider.maxValue.ToString();
        GameManager.Instance.mothership.GetComponent<InventoryScript>().addItem(blackMarketItemRecipe.outputProduct.outputType, (int)activeSlider.value);
        GameManager.Instance.economyManager.removeMoney((int)activeSlider.value);
        activeSlider.value = 0;
    }

    public void sellBtnOnClick()
    {
        activeSlider.maxValue -= activeSlider.value;
        sliderMaxText.text = activeSlider.maxValue.ToString();
        GameManager.Instance.mothership.GetComponent<InventoryScript>().removeItem(blackMarketItemRecipe.outputProduct.outputType, (int)activeSlider.value);
        GameManager.Instance.economyManager.addMoney(itemCost * (int)activeSlider.value);
        activeSlider.value = 0;
    }

    public void switchToBuyMode()
    {
        buyBtn.SetActive(true);
        sellBtn.SetActive(false);
        activeSlider.maxValue = buyMaxValue;
        activeSlider.value = 0;
    }

    public void switchToSellMode()
    {
        buyBtn.SetActive(false);
        sellBtn.SetActive(true);
        sellMaxValue = GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory[blackMarketItemRecipe.outputProduct.outputType];
        activeSlider.maxValue = sellMaxValue;
        activeSlider.value = 0;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, .5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, .5f);
    }
}
