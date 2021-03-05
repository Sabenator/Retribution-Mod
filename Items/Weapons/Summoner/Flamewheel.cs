using System;
using System.IO;
using Microsoft.Xna.Framework;
using Retribution.Projectiles.Minions;
using Terraria;
using Retribution;
using Retribution.Items.Souls;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Retribution.Projectiles.Minions;
using Retribution.Buffs.Summons;

namespace Retribution.Items.Weapons.Summoner
{
	public class Flamewheel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flamewheel Staff");
			Tooltip.SetDefault("Summons a flaming ring to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.knockBack = 3f;
			item.mana = 14;
			item.width = 66;
			item.height = 66;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<flamewheelbuff>();
			item.shoot = ModContent.ProjectileType<flamewheel>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(item.buffType, 2);

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 25);
			recipe.AddIngredient(ModContent.ItemType<flamelingstaff>(), 1);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
