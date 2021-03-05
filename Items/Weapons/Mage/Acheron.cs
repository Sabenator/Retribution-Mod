using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Projectiles;

namespace Retribution.Items.Weapons.Mage
{
	public class Acheron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archareon");
			Tooltip.SetDefault("Unleashes a fiery waterfall");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = 35;
			item.useAnimation = 35;
			item.damage = 45;
			item.crit = 4;
			item.noMelee = true;
			item.magic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item8;
			item.rare = ItemRarityID.Orange;
			item.shoot = ModContent.ProjectileType<bow>();
			item.shootSpeed = 12f;
			item.mana = 8;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -8, ProjectileID.BallofFire, 45, 0f, Main.myPlayer, player.whoAmI, 40);
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 2, -8, ProjectileID.BallofFire, 45, 0f, Main.myPlayer, player.whoAmI, 40);
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 4, -8, ProjectileID.BallofFire, 45, 0f, Main.myPlayer, player.whoAmI, 40);
			Projectile.NewProjectile(player.Center.X, player.Center.Y, -2, -8, ProjectileID.BallofFire, 45, 0f, Main.myPlayer, player.whoAmI, 40);
			Projectile.NewProjectile(player.Center.X, player.Center.Y, -4, -8, ProjectileID.BallofFire, 45, 0f, Main.myPlayer, player.whoAmI, 40);

			return false;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 25);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}