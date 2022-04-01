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

			// Voi olla huono idea tehdä tässä. Mitä jos pelissä pitäisi olla kaksi inventoriota?
			Load();
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

		public void Save()
		{
			PlayerPrefs.SetInt("Inventory_Item_Count", Items.Count);
			for (int i = 0; i < Items.Count; i++)
			{
				Item item = Items[i];
				PlayerPrefs.SetString($"Item_Name_{i}", item.Name);
				PlayerPrefs.SetFloat($"Item_Weight_{i}", item.Weight);
				PlayerPrefs.SetInt($"Item_Count_{i}", item.Count);

				// Koska PlayerPrefs ei suoraan tallenna enumeita, pitää suorittaa tyyppimuunnos ennen
				// tallennusta.
				int typeInt = (int)item.Type;
				PlayerPrefs.SetInt($"Item_Type_{i}", typeInt);

				// If-else ja muuttujaan sijoitus yhdellä rivillä
				int canStackInt = !item.CanStack ? 0 : 1;
				PlayerPrefs.SetInt($"Item_CanStack_{i}", canStackInt);
			}

			// Kirjoittaa tallennetut tiedot PlayerPrefs:iin
			PlayerPrefs.Save();
		}

		public void Load()
		{
			int itemCount = PlayerPrefs.GetInt("Inventory_Item_Count", 0);
			for (int i = 0; i < itemCount; i++)
			{
				string name = PlayerPrefs.GetString($"Item_Name_{i}", string.Empty);
				float weight = PlayerPrefs.GetFloat($"Item_Weight_{i}", 0);
				int count = PlayerPrefs.GetInt($"Item_Count_{i}", 0);

				int typeInt = PlayerPrefs.GetInt($"Item_Type_{i}", 0);
				ItemType type = (ItemType)typeInt; // Eksplisiittinen tyyppimuunnos

				int canStackInt = PlayerPrefs.GetInt($"Item_CanStack_{i}", 0);
				bool canStack = canStackInt != 0;

				Item item = new Item()
				{
					Name = name,
					Weight = weight,
					Count = count,
					Type = type,
					CanStack = canStack
				};

				AddItem(item);
			}
		}
	}
}
