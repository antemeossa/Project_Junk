using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int currentMoney = 1000;
    public int currentDebt;

    private void Start()
    {
        addMoney(100000);
    }
    public void addMoney(int amount)
    {
        currentMoney += amount;
    }

    public void removeMoney(int amount)
    {
        currentMoney -= amount;
    }
}
