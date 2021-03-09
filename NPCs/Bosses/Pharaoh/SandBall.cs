using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Pharaoh
{
	public class SandBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1f;
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
			counter++;
			projectile.rotation = projectile.rotation + MathHelper.ToRadians(counter * 3);
			if (projectile.ai[1] == 1) {
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(1.5f));
			}
		}
		public override void Kill(int timeLeft)
		{

		}
	}
}
