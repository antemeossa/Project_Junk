using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StorageListElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image, rarityBorder;
    [SerializeField]
    private TextMeshProUGUI  itemAmountText;
    private string itemName;
    [SerializeField]
    private int itemAmount;

    [SerializeField]
    private GameObject hoverPanel;

    private void Start()
    {
        
        hoverPanel.SetActive(false);

    }



    public void setStorageListElement(string itemName, int itemAmount, Sprite image, CraftRecipe rcp)
    {
        this.itemName = itemName;
        this.itemAmountText.text = itemAmount + "";
        this.image.sprite = image;
        setRarityBorder(rcp);
    }

    public int getItemAmount() { return itemAmount; }
    public void setItemAmount(int amount) { itemAmount = amount; this.itemAmountText.text = itemAmount + ""; }

    private void setRarityBorder(CraftRecipe obj)
    {
        if (obj.craftRarity.Equals(Rarity.Primitive))
        {
            rarityBorder.color = Color.gray;
        }
        else if (obj.craftRarity.Equals(Rarity.Standard))
        {
            rarityBorder.color = Color.yellow;
        }
        else if (obj.craftRarity.Equals(Rarity.Advanced))
        {
            rarityBorder.color = Color.green;
        }
        else if (obj.craftRarity.Equals(Rarity.Prototype))
        {
            rarityBorder.color = Color.cyan;
        }
        else if (obj.craftRarity.Equals(Rarity.CuttingEdge))
        {
            rarityBorder.color = Color.magenta;
        }
    }

    [SerializeField]
    GameObject hoverPanelText;

    private Vector3 localScale = Vector3.one;
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        hoverPanel.SetActive(true);
        hoverPanelText.GetComponent<TextMeshProUGUI>().text = itemName;
        localScale = transform.localScale;
        transform.DOScale(transform.localScale * 1.1f, .5f);
        /*
        transform.parent.transform.parent.transform.parent.GetComponent<UI_StorageDetails>().getHoverPanel.SetActive(true);
        transform.parent.transform.parent.transform.parent.GetComponent<UI_StorageDetails>().getHoverPanel.transform.position = hoverPanel.transform.position;
        transform.parent.transform.parent.transform.parent.GetComponent<UI_StorageDetails>().setHoverPanelName(itemName);*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverPanel.SetActive(false);
        transform.DOScale(localScale, .5f);
        //transform.parent.transform.parent.transform.parent.GetComponent<UI_StorageDetails>().getHoverPanel.SetActive(false);
    }
}
