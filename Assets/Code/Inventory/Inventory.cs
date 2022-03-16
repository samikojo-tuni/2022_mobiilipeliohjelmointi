using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
	// TODO: Can this be generalized?
	// Enum tyypittää inventorioon tallennettavat esineet.
	public enum ItemType
	{
		None = 0,
		Coin,
		Gem,
		Potato,
		Tomato
	}

	public class Inventory
	{
		public List<Item> Items { get; }

		public float WeightLimit { get; }

		// Inventorion tämänhetkinen paino, lasketaan uudestaan joka kerta kysyttäessä
		public float Weight
		{
			get
			{
				float weight = 0;
				foreach (Item item in Items)
				{
					weight += item.TotalWeight;
				}

				return weight;
			}
		}

		public Inventory(float weighLimit)
		{
			// Huom! Read-only propertyn arvon voi asettaa rakentajassa, mutta ei koskaan sen ulkopuolella
			WeightLimit = weighLimit;
			Items = new List<Item>();
		}

		// Lisää uuden itemin inventorioon. Palauttaa true, jos lisäys onnistui.
		// Paluuarvo on false, jos inventorion painoraja ylittyisi lisäyksen myötä.
		public bool AddItem(Item item)
		{
			if (Weight + item.TotalWeight > WeightLimit)
			{
				// Inventorion painoraja ylittyy. Uutta itemiä ei voida lisätä.
				return false;
			}

			// Selvitetään, onko inventoriossa jo uutta itemiä vastaava item (tyypit vastaavat toisiaan)
			Item existing = null;
			foreach (Item current in Items)
			{
				if (current.Type == item.Type)
				{
					existing = current;
					break; // Haettu item löytyi, keskeytetään loop
				}
			}

			if (existing != null && existing.CanStack)
			{
				// Uusi item voidaan tallentaa olemassa olevan kanssa samaan slottiin
				existing.Count += item.Count;
			}
			else
			{
				// Lisätään uusi item inventorioon
				Items.Add(item);
			}

			return true;
		}
	}
}
