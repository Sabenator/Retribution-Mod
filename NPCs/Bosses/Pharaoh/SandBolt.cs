using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Pharaoh
{
	public class SandBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = ProjectileID.Bullet;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.scale = 1.25f;
			projectile.penetrate = 5;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha =25;
			projectile.timeLeft = 300;
		}
		public int counter = 0;
		public Vector2 target;
		public Vector2 projPos;
		public override void AI()
		{
			counter++;
			Player player = Main.player[Main.myPlayer];
			if (projectile.ai[1] == 0)
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
			}
			if (projectile.ai[1] == 1 && counter < 90) {
				projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			}
			else if (projectile.ai[1] == 1 && counter == 90) {
				target = player.Center;
				projPos = projectile.Center;
			} 
			else if (projectile.ai[1] == 1 && counter > 90) {
				for (int i = 0; i < 10; i++) {
					Dust dust;
					dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.width, DustID.Smoke)];
					dust.noGravity = true;
				}
				AIMethods.DashTowardProj(projPos, target, Main.expertMode ? RetributionWorld.nightmareMode ? 16f : 14f : 10f, projectile);
				projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			}
			if (projectile.ai[1] > 240) {
				projectile.Kill();
			}
			if (projectile.ai[1] == 2 || projectile.ai[1] == 3) {
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(2));
			}
		}
        public override void Kill(int timeLeft)
        {
			if (projectile.ai[1] == 3) {
			}
        }
    }
}
