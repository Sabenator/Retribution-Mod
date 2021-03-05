using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Accessories
{
	public class VoodooDoll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voodoo Doll");
			Tooltip.SetDefault("Sets the success rate of Necromancy to 65%");
		}

		public override void SetDefaults()
		{
			item.accessory = true;
			item.width = 32;
			item.height = 32;
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(0, 0, 0, 80);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			RetributionPlayer.soulPercent = .65f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 30);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}