using Retribution.Dusts;
using Retribution.Buffs;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Projectiles
{
	public class Sabretoothproj : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 5;
			projectile.penetrate = -1;
			projectile.CloneDefaults(ProjectileID.LightDisc);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Ichor, 60);
		}

        public override void AI()
        {
			Dust dust;
			Vector2 position = projectile.position;
			dust = Main.dust[Terraria.Dust.NewDust(position, projectile.width, projectile.height, 6, 0f, 0f, 0, new Color(255, 255, 255), 1.710526f)];
			dust.noGravity = true;
		}
	}
}
