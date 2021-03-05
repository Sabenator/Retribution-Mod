using Terraria.ModLoader;
using Terraria.ID;
using Retribution;

namespace Retribution.Items.Placeable
{
	public class CursedProtector : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morbus Music Box");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.Furniture.CursedProtectorBox>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}