using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Retribution;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Retribution.Items.Weapons.Ranger
{
	public class Hydra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra");
		}

		public override void SetDefaults()
		{
			item.damage = 64;
			item.ranged = true;
			item.width = 34;
			item.height = 62;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4; 
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 40f;
			item.useAmmo = AmmoID.Arrow;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Main.PlaySound(SoundID.Item5, player.Center);
			float numberProjectiles = 20;
			float pi = 3.14159265f;
			Vector2 vector1 = new Vector2(speedX, speedY);
			vector1.Normalize();
			vector1 *= 40f;
			bool flag = Collision.CanHit(position, 0, 0, position + vector1, 0, 0);
			for (int i = 0; i < numberProjectiles; i++)
			{
				float num = (float)i - ((float)numberProjectiles - 1f) / 2f;
				Vector2 vector2 = vector1.RotatedBy((double)(pi * num / 10), default(Vector2));
				if (!flag)
				{
					vector2 -= vector1;
				}
				Projectile.NewProjectile(position.X + vector2.X, position.Y + vector2.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}