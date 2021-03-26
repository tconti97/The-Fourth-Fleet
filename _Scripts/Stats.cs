using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public GameManager GM;
    //public Colonist CL;

	public int carryWeight;
	public int carryCapacity;

	public Items[] inventory;

	//initalize items

	Items food;
	Items ammo;
	Items wheels;
	Items horses;
	Items medkit;
    

	void Awake()
	{
		food = new Items (1, "Food", 100); // TOM: Food has a weight of 1 and quantity of 100 here, will have player set these at some point
		ammo = new Items (1, "Ammo", 10);
		wheels = new Items (15, "Wheels", 2);
		horses = new Items (0, "Horses", 2);
		medkit = new Items (5, "MedKit", 0);

        inventory = new Items[5]{food, ammo, wheels, horses, medkit}; 

		carryCapacity = horses.quantity * 60 + GM.partySize * 30;
		carryWeight = 0;
		for (int i = 0; i < 5; i++) 
		{
			carryWeight += inventory[i].weight * inventory[i].quantity;
		}
	}

	void Update()
	{
		
	}
	
	//function to give initial items could be either here or in events?
}
