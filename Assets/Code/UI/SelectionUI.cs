using System.Collections;
using System.Collections.Generic;
using PeliprojektiExamples.Selection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Koska kahdessa nimiavaruudessa on saman niminen luokka, pitää meidän kertoa
// kääntäjälle kumpaa tässä scriptissä käytetään.
using Selectable = PeliprojektiExamples.Selection.Selectable;

namespace PeliprojektiExamples.UI
{
	public class SelectionUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private Image icon;

		public void SetSelectedObject(Selectable selected)
		{
			if (selected != null)
			{
				nameText.text = selected.Name;
				icon.sprite = selected.Icon;
				icon.color = new Color(1, 1, 1, 1);
			}
			else
			{
				nameText.text = "";
				icon.sprite = null;
				icon.color = new Color(1, 1, 1, 0);
			}
		}

		public void Clear()
		{
			SelectionSystem.Current.Selected = null;
		}
	}
}
