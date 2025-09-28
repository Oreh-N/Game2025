using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop
{   // Prefabs names. Their prices can be found on the same index in _shop_item_prices 
	List<string> _shop_items = new List<string>() { "Spawner0", "Warehouse0", "Tower0" };
	List<int> _shop_item_prices = new List<int>() { 100, 50, 200 };


	/// <summary>
	/// Tries to buy an item
	/// </summary>
	/// <param name="itemName"> The Item you want to buy</param>
	/// <param name="mBuild"> Someone's wallet</param>
	/// <returns>True - if payment was successful, otherwise - false</returns>
	public bool TryBuyItem(string itemName, MainBuilding mBuild)
	{
		if (_shop_items.Contains(itemName))
		{
			int price = _shop_item_prices[_shop_items.IndexOf(itemName)];
			if (mBuild.LootCounter[LootType.Gold] >= price)
			{
				mBuild.Pay(price);
				return true;
			}
			else UIManager.Instance.UpdateWarningPanel("Not enought money");
		}
		else UIManager.Instance.UpdateWarningPanel("There is no such an item");
		return false;
	}

	/// <summary>
	/// Returns item's price by it's name
	/// </summary>
	/// <param name="itemName"></param>
	/// <returns>Returns item's price or -1 if there is no such an item</returns>
	public int GetItemPrice(string itemName)
	{
		if (_shop_items.Contains(itemName))
		{ return _shop_item_prices[_shop_items.IndexOf(itemName)]; }
		return -1;
	}
}
