using Retribution.Projectiles;
using Retribution.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Items.Materials;
using Microsoft.Xna.Framework;

namespace Retribution.Items.Weapons.Mage
{
	public class TheBass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Bass");
		}

		public override void SetDefaults()
		{
			item.width = 88;
			item.height = 88;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 25;
			item.useAnimation = 25;
			item.damage = 21;
			item.crit = 4;
			item.noMelee = true;
			item.magic = true;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<TundraBall>();
			item.shootSpeed = 10;
			item.mana = 6;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (!Main.dedServ)
			{
				float cursorPosFromPlayer = player.Distance(Main.MouseWorld) / (float)(Main.screenHeight / 2 / 24);
				if (cursorPosFromPlayer > 24f)
				{
					cursorPosFromPlayer = 1f;
				}
				else
				{
					cursorPosFromPlayer = cursorPosFromPlayer / 12f - 1f;
				}

				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot((SoundType)2, "Sounds/Item/TheBass"), 1f, cursorPosFromPlayer);
			}

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<TundraBall>(), damage, knockBack, player.whoAmI, 0f, 0f);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<oceansoul>(), 5);
			recipe.AddIngredient(ModContent.ItemType<brokenstaff>(), 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}