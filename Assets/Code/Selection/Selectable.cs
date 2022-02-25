using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeliprojektiExamples.Selection
{
	public class Selectable : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField]
		private string objectName;

		[SerializeField]
		private Sprite icon;

		// Property, jossa vain get on määritetty. Käyttäytyy kuin read-only
		// muuttuja.
		public string Name
		{
			get { return objectName; }
			// set { objectName = value; }
		}

		public Sprite Icon
		{
			get
			{
				// 1. Onko icon määritetty? Jos on, palauta se
				if (icon != null)
				{
					return icon;
				}

				// 2. icon ei ole määritetty, uudelleenkäytä spriterendererin sprite
				SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
				if (spriteRenderer != null)
				{
					return spriteRenderer.sprite;
				}

				// 3. SpriteRenderer-komponenttia ei löytynyt
				return null;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			SelectionSystem.Current.Selected = this;
		}
	}
}
