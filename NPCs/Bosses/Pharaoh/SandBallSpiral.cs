using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Pharaoh
{
	public class SandBallSpiral : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 5;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 0;
			projectile.timeLeft = 300;
		}
		public int counter = 0;
		public Vector2 target;
		public Vector2 projPos;
		public override void AI()
		{
			if (projectile.ai[0] != 0 && counter == 0) {
				projectile.timeLeft = (int)projectile.ai[0];
			}
			counter++;
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(1));

		}
		public override void Kill(int timeLeft)
		{

		}
	}
}
