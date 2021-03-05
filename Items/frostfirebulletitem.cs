using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Projectiles;
using Retribution.Items.Weapons.Ranger;

namespace Retribution.Items
{
	public class frostfirebulletitem : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostfire Bullet");
			Tooltip.SetDefault("A Bullet infused with the souls of frost and fire");
		}

		public override void SetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 10;
			item.height = 18;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1.5f;
			item.value = 10;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<frostfirebullet>();
			item.shootSpeed = 16f;
			item.ammo = ModContent.ProjectileType<frostfirebullet>();
		}
	}
}