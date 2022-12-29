using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int moneyCount;


    public void MoneyUp(int money)
    {
        moneyCount += money;
    }
}
