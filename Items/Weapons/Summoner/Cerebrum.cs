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
using Retribution.Buffs.Summons;

namespace Retribution.Items.Weapons.Summoner
{
	public class Cerebrum : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cerebrum");
			Tooltip.SetDefault("Summons a creeper to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 22;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 28;
			item.height = 28;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<creeperbuff>();
			item.shoot = ModContent.ProjectileType<creeper>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(item.buffType, 2);

			return true;
		}
	}
}
