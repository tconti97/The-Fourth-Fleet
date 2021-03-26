using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	const int MAX_PARTY = 10;
	public int days;
	//public int food;
	public float distance;
    public float travelDay;     //distacne traveled per day
    public int ration;          //how much food each member eats per day
    public int rationHorse;     //if horse rations are different
    public Colonist[] party;
	// TOM: used to keep track of Party Members alive
	public int partySize;
	// TOM: used because I could not remember how to get the number of elements in an array
	public int phase;
	// TOM: used to detect which marker the player is at, useful for changing backgrounds
	bool game;

    //holds backgrounds and textboxes to change colors
	public GameObject[] backgrounds;
    public GameObject[] textboxes;

	public int stage;

	public Stats stats;
    public EventHandler events;
    public AnimationManager AM;

	public GameObject wagon;
	public GameObject human;
	public GameObject horse;

	public GameObject bg;
	public Text top;
	public Text bot;
	public GameObject spaceBox;
	public InputField inputBox;
    private int healing;
    private int daysHungry;
    private bool extraSick;

    private Vector3 mapHide = new Vector3 (0f, 0f, 8f);
    private Vector3 mapShow = new Vector3 (0f,0f,-2f);
    public GameObject map;

    bool changeDist;
    bool changeRat;


	bool spacePressed = false;

	bool pressedE = false;
	bool pressedQ = false;
	bool pressedP = false;
    bool pressedR = false;
    bool pressedT = false;

	void Start ()
	{
		stage = 0;
		days = 0;
		distance = 0;
		party = new Colonist[MAX_PARTY];
		phase = 0;

        travelDay = 10;
        ration = 2;
        

		spaceBox.SetActive (false);

		//party [0] = new Colonist ("Adam");
		//party [1] = new Colonist ("Beth");
		//party [2] = new Colonist ("Calvin");
		//party [3] = new Colonist (2, "Daniel", false, false);
		//party [4] = new Colonist ("Eve");


		//stats.inventory[0].quantity = 100; //TOM: this is just food
		partySize = 0;
		top.text = "Food: " + stats.inventory [0].quantity + "\t\tDays: " + days + "\t\tDistance: " + distance + "%\t\tParty: " + partySize;
        bot.text = "Name your 5 Settlers!";
        //bg.GetComponent<Renderer> ().material.color = Color.green; // TOM: Sets the background color, will be used to change material later
		game = true;
        daysHungry = 0;
        extraSick = false;

		Debug.Log ("In Start");
	}

	void Update ()
	{
		if (phase == 0)
        {
			top.gameObject.SetActive (false);
			bot.gameObject.SetActive (true);
            textboxes[0].SetActive(true);
			wagon.SetActive (false);

			if (Input.GetKeyDown (KeyCode.Return)) 
			{
				party [partySize] = new Colonist (inputBox.GetComponentInChildren<Text> ().text.ToString ());
				partySize++;
				inputBox.text = "";
			}

            //display settler names as they are added
                switch (partySize-1)
                {
                    case 0:
                        bot.text = "1. " + party[0].name;
                        break;
                    case 1:
                        bot.text = "1. " + party[0].name + "\t" +
                        "2. " + party[1].name;
                        break;
                    case 2:
                        bot.text = "1. " + party[0].name + "\t" +
                        "2. " + party[1].name + "\n" +
                        "3. " + party[2].name;
                        break;
                    case 3:
                        bot.text = "1. " + party[0].name + "\t" +
                           "2. " + party[1].name + "\n" +
                           "3. " + party[2].name + "\t" +
                           "4. " + party[3].name;
                        break;
                    case 4:
                        bot.text = "1. " + party[0].name + "\t" +
                            "2. " + party[1].name + "\n" +
                            "3. " + party[2].name + "\t" +
                            "4. " + party[3].name + "\n" +
                            "5. " + party[4].name;
                        break;
                }

            if (partySize == 5) 
			{
				phase++;
				inputBox.gameObject.SetActive (false);
				top.gameObject.SetActive (true);
				bot.gameObject.SetActive (true);
				wagon.SetActive (true);
				human.SetActive (false);
				horse.SetActive (false);
                textboxes[0].SetActive(true);

                bot.text = "E) Continue Forward\tQ) Rest\n" +
                             "P) Display Party\t M)Show Map" +
                             "\nR) Change Rations\tT) Change Speed";
            }
				
		}
		if (phase == 1) {
            // Debug.Log("Phase 1");

            //bot.text = "E) Continue Forward\nQ) Rest";
            if (Input.GetKeyDown(KeyCode.E) && !spacePressed)
            {
                spacePressed = true;
                spaceBox.SetActive(true);
                // TOM: Choice 1
                int eventID = Random.Range(1, 100); //Generates a random number

                if (stage == 0)     //Rainforest/Tropical
                {
                    if (eventID <= 25)
                        events.CrocAttack();    //25%
                    else if (eventID <= 40)
                        events.GetLost();       //15%
                    else if (eventID <= 55)
                        events.RandomInjury();  //15%
                    else if (eventID <= 65)
                        events.Positive();      //10%
                    else if (eventID <= 80)
                        events.Sickness();      //15%
                    else if (eventID <= 85 && travelDay == 15)
                        events.Sickness();      //+5% if moving fast
                    else if (eventID <= 90 && extraSick)
                        events.Sickness();      //+5% if hungry
                    else                        //20%
                    {
                        bot.text = "You progressed unharmed!";
                        distance += travelDay;
                        stats.inventory[0].subValue(partySize * ration);
                    }
                }
                else if (stage == 1)      //Grasslands
                {
                    if (eventID <= 0)
                        events.Sickness();      //00%
                    else if (eventID <= 15)
                        events.Positive();      //15%
                    else if (eventID <= 30)
                        events.RandomInjury();  //15%
                    else if (eventID <= 40)
                        events.GetLost();       //10%
                    else if (eventID <= 55)
                        events.KangarooAttack();  //15%
                    else if (eventID <= 70)
                        events.DingoAttack();     //15%
                    else if (eventID <= 80)
                        events.EmuHerd();       //10%
                    else if (eventID <= 85 && travelDay == 15)
                        events.Sickness();      //+5% if moving fast
                    else if (eventID <= 90 && extraSick)
                        events.Sickness();      //+5% if hungry

                    else                        //20%
                    {
                        bot.text = "You progressed unharmed!";
                        distance += travelDay;
                        stats.inventory[0].subValue(partySize * ration); //TOM: Subtracting food 
                    }
                }
                else if (stage == 2)        //Desert
                {
                    if (eventID <= 5)
                        events.Sickness();      //05%
                    else if (eventID <= 15)
                        events.Positive();      //10%
                    else if (eventID <= 20)
                        events.RandomInjury();  //05%
                    else if (eventID <= 30)
                        events.GetLost();       //10%
                    else if (eventID <= 50)
                        events.KangarooAttack();  //20%
                    else if (eventID <= 70)
                        events.DingoAttack();   //20%
                    else if (eventID <= 80)
                        events.EmuHerd();       //10%
                    else if (eventID <= 85 && travelDay == 15)
                        events.Sickness();      //+5% if moving fast
                    else if (eventID <= 90 && extraSick)
                        events.Sickness();      //+5% if hungry
                    else                        //15%
                    {
                        bot.text = "You progressed unharmed!";
                        distance += travelDay;
                        stats.inventory[0].subValue(partySize * ration);
                    }
                }
                else if (stage == 3)        //Temperate
                {
                    if (eventID <= 15)
                        events.EmuHerd();       //15%
                    else if (eventID <= 35)
                        events.GetLost();       //20%
                    else if (eventID <= 55)
                        events.RandomInjury();  //20%
                    else if (eventID <= 70)
                        events.Positive();      //15%
                    else if (eventID <= 75)
                        events.Sickness();      //05%
                    else if (eventID <= 80 && travelDay == 15)
                        events.Sickness();      //+5% if moving fast
                    else if (eventID <= 85 && extraSick)
                        events.Sickness();      //+5% if hungry
                    else                        //20%
                    {
                        bot.text = "You progressed unharmed!";
                        distance += travelDay;
                        stats.inventory[0].subValue(partySize * ration);
                    }
                }

                if (stats.inventory[0].quantity <= 0)
                {
                    stats.inventory[0].quantity = 0;
                    partySize--;
                }

                //party gets hungry after 5 days
                if (ration == 1)
                    daysHungry++;
                else if (ration == 2 && daysHungry != 0)
                    daysHungry--;

                sickCheck();
                if (daysHungry >= 5)
                    HungryCheck();

                days++;
                //break;
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !spacePressed)
            { // TOM: Choice 2
                spacePressed = true;
                spaceBox.SetActive(true);

                bot.text = "You Rested.";
                days++;
                stats.inventory[0].subValue(partySize * ration);

                //heal in resting
                for (int i = 0; i < partySize; i++)
                {
                    healing = Random.Range(1, 10);

                    if (party[i].sickness && healing <= 6)
                    {
                        party[i].sickness = false;
                    }
                    else if (healing <= 3)
                    {
                        if (party[i].health < 3)
                            party[i].health++;
                    }
                    //extra sick heal if going slow
                    else if (travelDay == 5)
                        if (party[i].sickness)
                            party[i].sickness = false;
                }
                if (stats.inventory[0].quantity <= 0)
                {
                    stats.inventory[0].quantity = 0;
                    partySize--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Introduction");
            }
            else if (Input.GetKeyDown(KeyCode.P) && !spacePressed)
            {
                spacePressed = true;
                bot.text = "";
                for (int i = 0; i < partySize; i++)
                {
                    string status;
                    if (party[i].health == 3)
                    {
                        status = "Healthy";
                    }
                    else if (party[i].health == 2)
                    {
                        status = "Injured";
                    }
                    else
                    { //hp==1 b/c 0 is dead
                        status = "Critical";
                    }
                    bot.text += party[i].name + " is " + status + ". ";
                    spaceBox.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.M) && !spacePressed) 
            {
                spacePressed = true;
                map.transform.position = mapShow;
                bot.text = "";

                spaceBox.SetActive(true);
            }
            else if (changeRat || (Input.GetKeyDown(KeyCode.R) && !spacePressed))  //change ration
            {
                spacePressed = true;
                bot.text = "Choose rations per person:" +
                    "\n A) Scraps \n S) Meager \n D) Hearty ";
                changeRat = true;

             
                    if (changeRat && Input.GetKeyDown(KeyCode.A))
                    {
                        ration = 1;
                        spacePressed = true;
                        daysHungry++;
                        spaceBox.SetActive(true);
                        changeRat = false;
                    }
                    else if (changeRat && Input.GetKeyDown(KeyCode.S))
                    {
                        ration = 2;
                        spacePressed = true;
                        spaceBox.SetActive(true);
                        changeRat = false;
                    }
                    else if (changeRat && Input.GetKeyDown(KeyCode.D))
                    {
                        ration = 3;
                        spacePressed = true;
                        daysHungry = 0;
                        spaceBox.SetActive(true);
                        changeRat = false;
                    }
                

            }
            else if (changeDist || (Input.GetKeyDown(KeyCode.T) && !spacePressed))  //Change distance
            {
                bot.text = "Choose speed you will travel:\n" +
                    "A) Slow \n S) Reasonable\n D) Brisk";
                changeDist = true;

                
                    if (changeDist && Input.GetKeyDown(KeyCode.A))
                    {
                        travelDay = 5;
                        //chance of heal?
                        spacePressed = true;
                        spaceBox.SetActive(true);
                        changeDist = false;
                    }
                    else if (changeDist && Input.GetKeyDown(KeyCode.S))
                    {
                        travelDay = 10;
                        spacePressed = true;
                        spaceBox.SetActive(true);
                        changeDist = false;
                    }
                    else if (changeDist && Input.GetKeyDown(KeyCode.D))
                    {
                        travelDay = 15;
                        //extra 5% chance of sickness
                        spacePressed = true;
                        spaceBox.SetActive(true);
                        changeDist = false;
                    }
                
            }
            //Space: Display initial prompts
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                AM.AnimOff();

                map.transform.position = mapHide;
                bot.text = "E) Continue Forward\tQ) Rest\n" +
                            "P) Display Party\t M)Show Map" +
                             "\nR) Change Rations\tT) Change Speed";
                spacePressed = false;
                spaceBox.SetActive(false);
            }	
		}



		for (int i = 0; i < partySize - 1; i++) {
			if (party [i] && party [i].health <= 0)
				removeMember (i);
		}

		if (stage == 0 && distance > 25) {
			backgrounds [stage].SetActive (false);
            textboxes[stage].SetActive(false);
			stage = 1;
			distance = 25;
			backgrounds [stage].SetActive (true);
            textboxes[stage].SetActive(true);
            //change background
        } else if (stage == 1 && distance > 50) {
			backgrounds [stage].SetActive (false);
            textboxes[stage].SetActive(false);
            stage = 2;
			distance = 50;
			backgrounds [stage].SetActive (true);
            textboxes[stage].SetActive(true);
            //change background
        } else if (stage == 2 && distance > 75) {
			backgrounds [stage].SetActive (false);
            textboxes[stage].SetActive(false);
            stage = 3;
			distance = 75;
			backgrounds [stage].SetActive (true);
            textboxes[stage].SetActive(true);
            //change background
        }

		if (partySize < 0) { // This is to prevent negative party size
			partySize = 0;
		}

		if (distance > 100) { // This is to prevent distance above 100
			distance = 100;
		}

		if (partySize == 0 && phase>0) {
			bot.text = "You Lose!";
			//yield return new WaitForSeconds (2.5f);
			SceneManager.LoadScene ("LoseScreen");
		} else if (distance == 100) {
			bot.text = "You Win!";
			//yield return new WaitForSeconds (2.5f);
			SceneManager.LoadScene ("WinScreen");
		}

		top.text = "Food: " + stats.inventory [0].quantity + "\t\tDays: " + days + "\t\tDistance: " + distance + "%\t\tParty: " + partySize;
	}

	public void removeMember (int index)
	{
		//TOM: should remove the selected member of the party and reorganize the array so that size reflects last indice
		bot.text = party [index].name + " has died!";
		party [index] = null;
	 

		for (int i = 0; i < partySize - 2; i++) {
			if (party [i] == null) {
				party [i] = party [i + 1];
				party [i + 1] = null;
			}
		}
		partySize--;
	}

	public void sickCheck ()
	{
		for (int i = 0; i < partySize; i++) {
			if (party [i] && party [i].sickness) {
				int tooSickFiveMe = Random.Range (1, 100);  //Awesome variable name, Tom
				if (tooSickFiveMe <= 60) {
					party [i].health--;
				}
			}
		}
	}

    //if you're hungry, you can get sick
    public void HungryCheck()
    {
        extraSick = true;
    }
}


/*
 * Random rnd = new Random();
 * int eventID= rnd.Next(1,100);
 * renderer.material.Color(0.5f, 1, 1);
 */
