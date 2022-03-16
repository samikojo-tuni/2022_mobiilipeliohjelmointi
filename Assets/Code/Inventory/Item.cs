namespace InventorySystem
{
	[System.Serializable]
	public class Item
	{
		public string Name;
		public float Weight;
		public ItemType Type;
		public bool CanStack;
		public int Count;

		// Computable property, arvo lasketaan joka kerta kun sitä kysytään.
		public float TotalWeight
		{
			get { return Weight * Count; }
		}
	}
}
