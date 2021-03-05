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
using Retribution.Buffs;

namespace Retribution.Items.Weapons.Summoner
{
	public class IceBreaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Breaker");
			Tooltip.SetDefault("Summons an Ice Crystal above your head");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 44;
			item.height = 44;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

			item.noMelee = true;
			item.summon = true;
			item.shoot = ModContent.ProjectileType<IceCrystal>();
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<IceCrystal>()] < 1)
			{
				player.UpdateMaxTurrets();
				return true;
			}
			else
			{
				return false;
			}
		}
    }
}
