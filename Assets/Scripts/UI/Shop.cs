using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop
{	// Prefabs names
	List<string> _shop_items = new List<string>() { "Spawner1", "Warehouse0", "DefBuild1" };     // Their prices can be found on the same index in _shop_item_prices 
	List<int> _shop_item_prices = new List<int>() { 100, 50, 200 };


	/// <summary>
	/// Tries to buy item
	/// </summary>
	/// <param name="itemName"> The Item you want to buy</param>
	/// <param name="wallet"> Someone's wallet</param>
	/// <returns>True - if payment was successful, otherwise - false</returns>
	public bool TryBuyItem(string itemName, Wallet wallet)
	{
		if (_shop_items.Contains(itemName))
		{
			int price = _shop_item_prices[_shop_items.IndexOf(itemName)];
			if (wallet.Money >= price)
			{
				wallet.Pay(price);
				return true;
			}
			else UIManager.Instance.UpdateWarningPanel("Not enought money");
		}
		else UIManager.Instance.UpdateWarningPanel("There is no such an item");
		return false;
	}
}
