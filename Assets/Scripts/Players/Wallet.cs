using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public int Money {  get; private set; }


    public Wallet(int start_money)
    { Money = start_money; }

    public void Pay(int price) { Money -= price; }

    public void Earn(int money) { Money += money; }
}
