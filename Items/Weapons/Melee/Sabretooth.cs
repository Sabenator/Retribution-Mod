using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Weapons.Melee
{
	public class Sabretooth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sabretooth");
			Tooltip.SetDefault("Applies Cursed Ichor to enemies");
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.damage = 18;
			item.width = 38;
			item.height = 38;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.knockBack = 3f;
			item.autoReuse = true;
			item.value = Item.sellPrice(0, 1, 75, 0);
			item.rare = 0;
			item.melee = true;
			item.UseSound = SoundID.Item1;
			item.useTurn = true;
			item.shoot = mod.ProjectileType("sabretoothproj");
			item.shootSpeed = 7f;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[item.shoot] == 2)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddIngredient(ItemID.CrimtaneBar, 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}