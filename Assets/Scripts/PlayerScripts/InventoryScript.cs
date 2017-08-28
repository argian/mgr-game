﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {


	//connecting scripts
	public static InventoryScript instance;

	private int itemsInRow = 1;
	private int buttonSize;
	private int buttonMargin;

	public Texture2D emptyItemIcon;
	public Texture2D rightMouseIcon;

	private bool openInventory;

	private List<ItemAbstract> itemsList;

	private List<Rect> buttonsList = new List<Rect> (); //zmienna do przechowywania ikon
	private int[] itemsCounter = new int[0]; //ilość przedmiotów na danej pozycji
	private int itemsInInventory = 0;

	private PlayerData playerData;


	//--------------------------------------------------------------------------------------------------------//

	void Awake(){

		if (instance == null) {

			DontDestroyOnLoad (gameObject);
			instance = this;

		} else if(instance != this) {
			Destroy (gameObject);
		}

	}

	//--------------------------------------------------------------------------------------------------------//
	void Start() {
		openInventory = false;
		itemsList = new List<ItemAbstract> ();

		buttonSize = (int)(Screen.height * 0.09);
		buttonMargin = (int)(Screen.height * 0.01);
	}

	//--------------------------------------------------------------------------------------------------------//
	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			openInventory = !openInventory;
		}
	}

	//--------------------------------------------------------------------------------------------------------//
	void OnGUI() {

		GUI.Button(new Rect ((buttonMargin), (Screen.height - buttonSize - buttonMargin), buttonSize, buttonSize), rightMouseIcon);

		if (openInventory) {

			int numberOfItems = (Screen.height / (buttonSize + (2 * buttonMargin)));

			//jeżeli inwentarz jest pusty, rysujemy puste ikony

			if (itemsList.Count == 0) {

				for (int i = 0; i < numberOfItems; i++) {
					GUI.Button (new Rect ((buttonMargin),
						(buttonMargin + buttonMargin * i + buttonSize * i),
						buttonSize, buttonSize), emptyItemIcon);
				}

			} 
			//jeżeli nie jest to nie rysujemy pustych ;)
			else if (itemsList.Count != 0) {

				for (int i = 0; i < itemsList.Count; i++) {
					
					if (GUI.Button (new Rect ((buttonMargin),
						   (buttonMargin + buttonMargin * i + buttonSize * i),
						   buttonSize, buttonSize), itemsList [i].getItemIcon ())) {

						//zmniejszenie ilości
						itemsList[i].setItemCounter(itemsList[i].itemCounter - 1);
						Debug.Log ("After" + " --- " + itemsList [i].itemCounter);

						if (itemsList [i].itemCounter < 1) {
							//Debug.Log ("Counter mniejszy od 1!");
							itemsList.Remove (itemsList [i]);
						}

						/*    --------------- kod podjęcia akcji przez przycisk ---------------    */

						//Debug.Log ("CLICKING ON " + itemsList[i].getItemName());
					}


				}
				for(int i = itemsList.Count; i < numberOfItems; i++){
					GUI.Button (new Rect ((buttonMargin),
						(buttonMargin + buttonMargin * i + buttonSize * i),
						buttonSize, buttonSize), emptyItemIcon);
				}
			}

		} 
	}

	//--------------------------------------------------------------------------------------------------------//

	public void addItem(ItemAbstract item){

		Predicate<ItemAbstract> itemFind = (ItemAbstract i) => {
			return i.itemName == item.getItemName ();
		};

		// Predicate jest to klasa upraszczająca wyszukiwanie obiektu o zadanym kryterium w liście/tablicy obiektów	
		//pozwala określić co z czym trzeba porównać

		int index = itemsList.FindIndex (itemFind); //funkcja FindIndex przelatuje po tablicy i porównuje nazwy zwraca indeks

		if (index > -1) { //czyli przedmiot o danej nazwie jest już w inwentarzu
			item.setItemCounter (item.itemCounter + 1);

			//Debug.Log ("ITEM NAME: " + item.getItemName() + " --- " + "AMOUNT: " + item.itemCounter);

		} else {

			itemsList.Add (item);
			itemsInInventory++;
			item.setItemCounter (item.itemCounter + 1);

			//Debug.Log ("ITEM NAME: " + item.getItemName() + " --- " + "AMOUNT: " + item.itemCounter);
		}

		/*
		if (index > -1) {
			itemsCounter++;
		} else {

			itemsList.Add (item);
			itemsInInventory++;
			int[] tmp = new int[itemsInInventory];

			for (int i = 0; i < itemsCounter.Length; i++) {
				tmp [i] = itemsCounter [i];
			}

			tmp [itemsInInventory - 1] = 1;
			itemsCounter = new int[itemsInInventory];
			itemsCounter = tmp;
		}


		itemsList.Add (item);
		Debug.Log ("FIRST: " + itemsList[0]);
		//Debug.Log ("GETTER: " + item);
		*/
	}

}