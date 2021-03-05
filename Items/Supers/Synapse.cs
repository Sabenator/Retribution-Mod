using Retribution.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Retribution.Items.Supers
{
	public class Synapse : Super
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sonic Synapse");
		}

		public override void SafeSetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 30;
			item.useAnimation = 30;
			item.damage = 245;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item8;
			item.shoot = ModContent.ProjectileType<snapshotproj>();
			item.shootSpeed = 0f;
			points = 50;
		}
	}
	public class snapshotproj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 1000;
			projectile.height = 1000;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 20;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}

		bool used = false;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.BrokenArmor, 500);
		}

		public override void AI()
		{
			if (used == false)
			{
				Projectile.NewProjectile(projectile.Center, new Vector2(), ModContent.ProjectileType<ShockwaveEffect>(), 0, 0f);
				used = true;
			}
		}
	}
}