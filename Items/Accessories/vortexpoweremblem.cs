using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Retribution.Items.Accessories
{
	public class vortexpoweremblem : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Power Emblem");
			Tooltip.SetDefault("25% increased ranged damage"
				+ "\n75% decreased all non-ranged damage"
				+ "\n25% Main.rand.NextFloat() < .15f to not consume ammo");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = 1000;
			item.rare = ItemRarityID.Red;
			item.accessory = true;
			item.maxStack = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			var rP = player.GetModPlayer<RetributionPlayer>();

			player.rangedDamage += 0.25f;
			player.meleeDamage -= 0.75f;
			player.minionDamage -= 0.75f;
			player.magicDamage -= 0.75f;
			player.thrownDamage -= 0.75f;
			rP.reaperDamage -= 0.75f;
			player.ammoCost75 = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex, 5);
			recipe.AddIngredient(ItemID.RangerEmblem, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}