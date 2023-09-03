using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int currentMoney = 0;
    public int currentDebt;
    public UI_Manager UI_M;
    public List<UI_ContractElemetnScript> activeContracts = new List<UI_ContractElemetnScript> ();

    private void Start()
    {
        addMoney(10000);
        UI_M.currentMoneyText.text = currentMoney + "";

    }
    public void addMoney(int amount)
    {
        currentMoney += amount;
        UI_M.currentMoneyText.text = currentMoney + "";
    }

    public void removeMoney(int amount)
    {
        currentMoney -= amount;
        UI_M.currentMoneyText.text = currentMoney + "";

    }

    public void addContract(UI_ContractElemetnScript contract)
    {
        activeContracts.Add(contract);
    }

    public void removeContract(UI_ContractElemetnScript contract)
    {
        activeContracts.Remove(contract);
    }

    public void completeContract(UI_ContractElemetnScript contract, itemTypes item, int amount)
    {
        addMoney(contract.rewardMoney);
        activeContracts.Remove(contract);
        GameManager.Instance.mothership.GetComponent<InventoryScript>().removeItem(item, amount);
        UI_M.currentMoneyText.text = currentMoney + "";

    }
}
