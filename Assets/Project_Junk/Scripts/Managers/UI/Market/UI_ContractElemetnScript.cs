using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ContractElemetnScript : MonoBehaviour
{
    [SerializeField] private Image factionImg;
    [SerializeField] private TextMeshProUGUI factionName;
    [SerializeField] private TextMeshProUGUI factionRelation;
    [SerializeField] private Image contractImg;
    [SerializeField] private Image rarityBorder;
    [SerializeField] private TextMeshProUGUI contractName;
    [SerializeField] private int reqItemAmount;
    [SerializeField] private TextMeshProUGUI reward;
    [SerializeField] private GameObject accptBtn, dclnBtn, handinBtn;
    [SerializeField] private Slider activeSlider;

    public int rewardMoney;
    public bool isActive;
    private bool canHandIn;

    private CraftRecipe contractRecipe;
    private Faction contractFaction;


    private void Start()
    {
        activeSlider.minValue = 1;
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        updateActiveSlider();
    }
    public void setContractElementDetails(CraftRecipe rec, Sprite factionImage, string fctnName, FactionRelationsEnum fctnRelation,
        Sprite contractImage, Rarity rarity, itemTypes cntrctName, int req, int rewardAmount)
    {
        contractRecipe = rec;
        factionImg.sprite = factionImage;
        factionName.text = fctnName;
        factionRelation.text = "Relation: " + Utils.enumToString(fctnRelation);
        contractImg.sprite = contractImage;
        contractName.text = Utils.enumToString(cntrctName) + " x" + req;
        reqItemAmount = req;
        reward.text = "REWARD: " + rewardAmount;
        rewardMoney = rewardAmount;
        setRarityBorder(rarity, rarityBorder);
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
        GameManager.Instance.soundManager.playBtnSound();

        GameManager.Instance.economyManager.addContract(this);
        isActive = true;
        transform.DOScale(1.1f, .5f);
        transform.DOScale(1f, .5f);
        dclnBtn.gameObject.SetActive(false);
        activeSlider.gameObject.SetActive(true);
        accptBtn.gameObject.SetActive(false);
    }

    public void updateActiveSlider()
    {
        activeSlider.maxValue = reqItemAmount;
        
        if (GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory != null && isActive)
        {
            if (GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory.ContainsKey(contractRecipe.outputProduct.outputType))
            {
                activeSlider.value = Mathf.Lerp(activeSlider.value, GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory[contractRecipe.outputProduct.outputType], Time.deltaTime);
                if (activeSlider.value >= activeSlider.maxValue)
                {
                    handinBtn.SetActive(true);
                    activeSlider.gameObject.SetActive(false);
                }
                else
                {
                    handinBtn.SetActive(false);
                    activeSlider.gameObject.SetActive(true);
                    activeSlider.value = GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory[contractRecipe.outputProduct.outputType];
                }
            }
            
        }


    }
    public void handInBtnOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();

        if (GameManager.Instance.mothership.GetComponent<InventoryScript>().getAmountOfSelectedProduct(contractRecipe.outputProduct.outputType) >= reqItemAmount)
        {
            GameManager.Instance.economyManager.completeContract(this, contractRecipe.outputProduct.outputType, reqItemAmount);
            isActive = true;
            transform.parent.transform.parent.transform.parent.transform.parent.GetComponent<UI_ContractPanelScript>().contractsList.Remove(this);
            transform.DOScaleY(0f, .5f).OnComplete(() =>
            {                
                Destroy(gameObject);
            });
            
        }
    }
    public void declineOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();

        gameObject.SetActive(false);
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
