using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Blocks
{
	public class IceCrystalBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Crystal Block");
			ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.IceCrystalTile>();
			item.width = 16;
			item.height = 16;
			item.value = 300;
		}
	}
}