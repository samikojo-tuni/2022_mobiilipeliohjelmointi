using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace InventorySystem.UI
{
	public class InventoryUI : MonoBehaviour
	{
		[System.Serializable]
		public class ItemImage
		{
			public ItemType Type;
			public Sprite Image;
		}

		[SerializeField]
		private InventoryUIItem template;

		[SerializeField]
		private int slots = 8;

		[SerializeField]
		private TMP_Text weightText;

		[SerializeField]
		private ItemImage[] itemImages;

		[SerializeField] private LocalizedString localizedWeight;

		private InventoryUIItem[] items;
		private Inventory inventory;

		private void Awake()
		{
			items = new InventoryUIItem[slots];
			for (int i = 0; i < slots; i++)
			{
				items[i] = Instantiate(template, template.transform.parent);
				items[i].Setup(this);
			}

			// Deaktivoidaan template object. Sille ei ole enää tarvetta
			template.gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			// Aloittaa eventin kuuntelun. Eventin lauetessa suoritetaan OnLocalizationChanged.
			LocalizationSettings.SelectedLocaleChanged += OnLocalizationChanged;
		}

		private void OnDisable()
		{
			// Lopettaa eventin kuuntelun.
			LocalizationSettings.SelectedLocaleChanged -= OnLocalizationChanged;
		}

		private void OnLocalizationChanged(Locale locale)
		{
			// Kun lokalisaatio muuttuu, päivitetään komponentin tekstit
			SetWeight();
		}

		public void SetInventory(Inventory inventory)
		{
			this.inventory = inventory;
			UpdateInventory();
		}

		public void UpdateInventory()
		{
			foreach (Item item in inventory.Items)
			{
				// GetItem luultavasti bugaa uniikkien itemien kanssa.
				// ItemType:n sijaan välitä koko Item olio ja tarkista itemin uniikkius (stackatuvuus).
				InventoryUIItem uiItem = GetItem(item.Type);
				if (uiItem != null)
				{
					uiItem.SetItem(item);
				}
			}

			SetWeight();
		}

		public Sprite GetImage(ItemType type)
		{
			foreach (ItemImage item in itemImages)
			{
				if (item.Type == type)
				{
					return item.Image;
				}
			}

			return null;
		}

		private void SetWeight()
		{
			weightText.text = $"{localizedWeight.GetLocalizedString()}: {inventory.Weight}";
		}

		private InventoryUIItem GetItem(ItemType type)
		{
			foreach (InventoryUIItem uiItem in items)
			{
				if (uiItem.Type == type)
				{
					return uiItem;
				}
			}

			// Tyyppiä vastaavaa UI itemiä ei löytynyt
			foreach (InventoryUIItem uiItem in items)
			{
				if (uiItem.Item == null)
				{
					return uiItem;
				}
			}

			return null;
		}
	}
}
