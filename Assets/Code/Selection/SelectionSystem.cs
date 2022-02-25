using PeliprojektiExamples.UI;
using UnityEngine;

namespace PeliprojektiExamples.Selection
{
	public class SelectionSystem : MonoBehaviour
	{
		//  Viittaa scenessä olevaan SelectionSystemiin.
		private static SelectionSystem current;
		public static SelectionSystem Current
		{
			get { return current; }
		}

		private SelectionUI ui;

		// Valittu olio
		private Selectable selected;

		public Selectable Selected
		{
			get { return selected; }
			set
			{
				selected = value;

				// Viittaus olioon voi olla null. Tämä tarkoittaa, että oliota ei ole olemassa.
				// Null-viittaus on laillinen käyttötapaus ja siihen pitää varautua
				if (selected != null)
				{
					Debug.Log(selected.Name + " selected");
				}
				else
				{
					Debug.Log("Selection cleared");
				}

				// Välitetään valinta UI:lle
				ui.SetSelectedObject(selected);
			}
		}

		private void Awake()
		{
			current = this; // this on viittaus tähän olioon.
		}

		private void Start()
		{
			ui = FindObjectOfType<SelectionUI>();
		}
	}
}
