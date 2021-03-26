using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour {

    public Text eventText;

    public Stats ST;
    public GameManager gm;
    public AnimationManager AM;

    //ammo needed per enemy
    private int dingoAmmo = 2;
    private int kangAmmo = 3;
    private int crocAmmo = 2;
    private int needAmmo;

    //hold random gen nums
    private int dingo;
    private int kang;
    private int croc;
    private int emu;
    private int days;
    private int foodAdd;
    private int injury;
    private int sick;
    private int positive;

    private int missRand;

    //settler to be injured
    //or which of damage choices occures
    private int settler;
    private int damageChoice;

    //set true if not all animal attackers are killed
    private bool ouch;


    public void DingoAttack()
    {
        Debug.Log("Enter DingoAttack()");
        AM.dingoAnim.SetActive(true);

        //number of dingos(1-5)
        dingo = Random.Range(2, 5);

        //check ammo once Tom finishes that stuff Mary didn't get
        needAmmo = dingoAmmo * dingo;

		if (needAmmo <= ST.inventory [1].quantity) {
			ST.inventory [1].subValue (needAmmo);
			ouch = false;
		}
		else 
		{
			ST.inventory [1].quantity = 0;
			ouch = false;
		}

        //25% chance to miss and get hurt anyway
        ouch = MissShot(ouch);

        //if no injuries occur, else injure someone
        if (ouch == false)
        {
                eventText.text = dingo + " dingos attack! \n" +
                "The party escapes safely.";
        }
        else if (ouch == true)
        {
			settler = Random.Range(0, gm.partySize-1);
			eventText.text = dingo + " dingos attack! \n" +
                gm.party [settler].name + " took damage!";
            //injure someone
            gm.party [settler].health--;
        }
        else {
            //error
            Debug.Log("Error- Dingo Attack");
        }

        MoveEat();
    }

    public void CrocAttack()
    {
        Debug.Log("Enter CrocAttack()");
        AM.crocAnim.SetActive(true);

        //number of crocs(1-3)
        croc = Random.Range(1, 3);

        //check ammo
        needAmmo = crocAmmo * croc;

		if (needAmmo <= ST.inventory [1].quantity) {
			ST.inventory [1].subValue (needAmmo);
			ouch = false;
		}
		else 
		{
			ST.inventory [1].quantity = 0;
			ouch = false;
		}

        //25% chance to miss and get hurt anyway
        ouch = MissShot(ouch);

        //if no injuries occur
        if (ouch == false)
        {
            if (croc == 1)
                eventText.text = "A crocodile appears in the brush! \n" +
                    "The party escapes safely.";
            else
                eventText.text = croc + " crocodiles appear in the brush! \n" +
                    "The party escapes safely.";
        }
        //attack horses or wheels
        else if (ouch == true)
        {
            damageChoice = Random.Range(0, 1);
            switch (damageChoice)
            {
                case 0:
                    eventText.text = croc + " crocodiles appear in the brush! \n" + 
                        "They have attacked your horses!";
                    //damage one horse
                    break;
                case 1:
                    eventText.text = croc + " crocodiles appear in the brush! \n" +
                        "They have attacked your wagon wheel.";
                    //attack one wheel
                    break;
                default:
                    //error
                    Debug.Log("Error- default croc");
                    break;
            }
        }
        else
        {
            //error
            Debug.Log("Error- CrocAttack");
        }

        MoveEat();
    }

    public void KangarooAttack()
    {
        Debug.Log("Enter KangarooAttack()");
        AM.kangAnim.SetActive(true);

        //gen random number(1-3)
        kang = Random.Range(1, 3);

        //check ammo
        needAmmo = kangAmmo * kang;

		if (needAmmo <= ST.inventory [1].quantity) {
			ST.inventory [1].subValue (needAmmo);
			ouch = false;
		}
		else 
		{
			ST.inventory [1].quantity = 0;
			ouch = false;
		}

        //25% chance to miss and get hurt anyway
        ouch = MissShot(ouch);

        if (ouch == false)
        {
            if (kang == 1)
                eventText.text = "An angry kangaroo! \n" +
                    "The party escapes safely.";
            else
                eventText.text = kang + " angry kangaroos! \n" +
                    "The party escapes safely.";
        }
        else if (ouch == true)
        {
            damageChoice = Random.Range(0, 1);

            switch (damageChoice)
            {
                case 0:
                    //kick off wagon wheel
                    eventText.text = kang + " angry kangaroos! \n" +
                        "The kangaroos kick off your wagon wheel!";
                    break;
                case 1:
                    // inflict 2 injuries to a person
					settler = Random.Range(0, gm.partySize-1);
					eventText.text = kang + " angry kangaroos! \n" + 
                        gm.party [settler].name + " was kicked!";
					gm.party [settler].health-=2;
                    break;
                default:
                    //error
                    Debug.Log("Error- default Kangaroo");
                    break;
            }

        }
        else
        {
            //error
            Debug.Log("Error- Kangaroo Attack");
        }

        MoveEat();
    }

    public void EmuHerd()
    {
        Debug.Log("Enter EmuHerd()");
        AM.emuAnim.SetActive(true);

        //gen range of herd (10 to 45)
        emu = Random.Range(10, 45);

        //lose 1 day per 15 emu
        if (emu>0 && emu <= 10)
                days = 1;
        else if (emu > 10 && emu <= 20)
                days = 2;   
        else if (emu > 20 && emu <= 30)
                days = 3;   
		else if (emu > 30)
			days = 4;  
        else
                Debug.Log("Error- default Herd");

        eventText.text = "Woah! A herd of " + emu + " blasted emu! \n" +
            "You lose " + days + " days to get past them.";

        //distance and food
        //gm.distance += gm.;
        ST.inventory[0].subValue(gm.partySize*gm.ration*days);
    }

    public void GetLost()
    {
        Debug.Log("Enter GetLost()");

        //gen random num (1-5)
        days = Random.Range(1, 5);

        //lose that many days
        eventText.text = "You lose the trail, \n " +
            "adding " + days + " days to your trip.";

        //distance and food
        //gm.distance += gm.travelDay;
		ST.inventory[0].subValue(gm.partySize*gm.ration*(days));
    }

    public void RandomInjury()
    {
        Debug.Log("Enter RandomInjury()");

        //rand num
        injury = Random.Range(1,3);
        settler = Random.Range(1, gm.partySize);

        switch (injury)
        {
            //breaks arm - 1 injury
            case 1:
            case 2:
                eventText.text = gm.party[settler].name + "'s arm is broken!";
                gm.party[settler].health--;
                break;
            //shoots self- 1 injury, 1 bullet lost
            case 3:
                eventText.text = gm.party[settler].name + " shoots him/herself with the rifle!";
                gm.party[settler].health--;
                ST.inventory[1].subValue(1);
                break;
            default:
                //error
                Debug.Log("Error- default Injury");
                break;
        }

        MoveEat();
    }

    public void Sickness()
    {
        Debug.Log("Enter Sickness()");

        //rand num
        sick = Random.Range(1, 5);
        settler = Random.Range(1, gm.partySize);

        //dysentary - least likely
        //small pox
        //clyomidia
        switch (sick)
        {
            case 1:
            case 2:
                eventText.text = gm.party[settler].name + " was scratched by a koala!"
                    + "\n He/She has contracted chlamydia.";
                gm.party[settler].sickness= true;
                break;
            case 3:
            case 4:
                eventText.text = gm.party[settler].name + " has small pox";
                gm.party[settler].sickness = true;
                break;
            case 5:
                eventText.text = gm.party[settler].name + " has dysentary.";
                gm.party[settler].sickness = true;
                break;
            default:
                 Debug.Log("Error- default sickness");
                break;
        }

        MoveEat();
    }

    public void Positive()
    {
        Debug.Log("Enter Positive()");

        //rand num
        positive = Random.Range(1,2);
        //cactus juice (+ food)
        //bush tomatos (+ food)
        //abandoned supplies (+ wheels)
        //rare items (ex: bandages, boomerangs, etc.)???
        switch (positive)
        {
            case 1:
                eventText.text = "Your party stumbles across some cacti! \n"
                    + "The juice inside is delicious! \n It'll quench ya!";
                ST.inventory[0].addValue(5);
                break;
            case 2:
                eventText.text = "Your party finds a couple of \n bush tomatoes!"
                    + "\n They have been added \n to your food supply.";
                ST.inventory[0].addValue(7);
                break;
            case 3:
                eventText.text = "You find a tipped wagon just off the path."
                    + "\n The owners are long gone, \n so you take the wheels.";
                ST.inventory[3].addValue(2);
                break;
            default:
                Debug.Log("Error- default Possitive");
                break;
        }

        MoveEat();
    }


    //25% chance that a shot will just miss and an animal gets you anyway
    private bool MissShot(bool inj)
    {
        Debug.Log("Enter MissShot()");

        if (inj == true)
        {
            return true;
        }
        else if (inj == false)
        {
            missRand = Random.Range(1, 4);

            if(missRand==1)
                    return true;
            else if (missRand==2||missRand==3||missRand==4)
                    return false;
            else
            { 
                    Debug.Log("Error- Missed Shot");
                    return inj;
            }
        }
        else
        {
            Debug.Log("Error- Missed Shot");
            return inj;
        }

    }

    public void MoveEat()
    {
        //distance and food
        gm.distance += gm.travelDay;
        ST.inventory[0].subValue(gm.partySize*gm.ration);
    }
}


/* 
 * POSSIBLE EVENTS:
 *  DingoAttack()
 *  CrocAttack()
 *  KangarooAttack()
 *  EmuHerd()
 *  GetLost()
 *  RandomInjury()
 *  Sickness()
 *  Positive()
 */