using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Materials
{
	public class Blood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood");
		}

		public override void SetDefaults()
		{
			item.width = 26; //Filler size till sprite
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.maxStack = 999;
		}
	}
}