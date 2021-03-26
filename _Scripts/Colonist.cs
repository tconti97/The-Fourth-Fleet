using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonist : MonoBehaviour 
{
	public int health;
	public string name;

	public bool sickness;
    public bool hungry;

	public Colonist(int inHealth, string inName, bool isSick, bool isHungry)
	{
		health = inHealth;
		name = inName;
		sickness = isSick;
        hungry = isHungry;
	}

	public Colonist(string inName)
	{
		health = 3;
		name = inName;
		sickness = false;
        hungry = false;
	}
}