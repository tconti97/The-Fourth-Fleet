using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour 
{
	public int weight;
	public string itemName;
	public int quantity;

	public Items (int inWeight, string inName, int inNum)
	{
		weight = inWeight;
		itemName = inName;
		quantity = inNum;
	}

	public void addValue(int num)
	{
		quantity += num;
	}

	public void subValue(int num)
	{
		quantity -= num;
	}
}
