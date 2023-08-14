using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNode_UI : MonoBehaviour
{
    [SerializeField]
    private Button researchButton;
    [SerializeField]
    private TextMeshProUGUI researchName, researchDesc, researchCost, researchTime, researchLv;
    [SerializeField]
    private Image researchImg;
    [SerializeField]
    private Material lockedMaterial;
    [SerializeField]
    private GameObject researchSlider;

    private string researchLvlText;
    private void updateUI(string name, string desc, int cost, float time, int researchLvl, Sprite img)
    {
        researchName.text = name;
        researchDesc.text = desc;
        researchCost.text = "COST: " + cost;
        researchTime.text = time + " Seconds";
        researchImg.sprite = img;

        switch (researchLvl)
        {
            case 1:
                researchLvlText = "I";
                break;
            case 2:
                researchLvlText = "II";
                break;
            case 3:
                researchLvlText = "III";
                break;
            case 4:
                researchLvlText = "IV";
                break;
            case 5:
                researchLvlText = "V";
                break;
            default:
                researchLvlText = "I";
                break;
        }

        researchLv.text = researchLvlText;
        if (!GetComponent<ResearchNode_Base>().isUnlocked)
        {
            researchImg.material = lockedMaterial;
        }
        else
        {
            researchImg.material = null;
        }
    }

    private ResearchNode_Base node;
    private void Start()
    {
        node = GetComponent<ResearchNode_Base>();
        researchButton.onClick.AddListener(() => { transform.parent.GetComponent<ResearchTree_UI>().researchNodeOnClick(GetComponent<ResearchNode_Base>()); });
        updateUI(node.researchName, node.researchDescription, node.researchCost, node.researchTime, node.researchLvl, node.researchImg);
    }

    public void updateResearchPercUI()
    {
        researchSlider.GetComponent<Slider>().value = node.researchPerc;
    }

    public void unlockMaterial()
    {
        researchImg.material = null;
    }
    public GameObject getResearchSlider { get { return researchSlider; } }
}
