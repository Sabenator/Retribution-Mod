using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Retribution.Projectiles;
using Terraria.ModLoader;
using Retribution.Buffs;

namespace Retribution.Items.Weapons.Reaper.Minions
{
	public class FireWisp : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 24;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 240;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

        public override void AI()
        {
			if (projectile.timeLeft < 60)
			{
				projectile.alpha += 10;
			}

			#region Animation and visuals
			projectile.rotation = projectile.velocity.X * 0.05f;

		    Dust.NewDust(projectile.Center, projectile.width, projectile.height / 2, DustID.Fire);

			int frameSpeed = 5;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			projectile.ai[0]++;

			Player player = Main.player[projectile.owner];

			for (int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
				float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				float shootToY = target.position.Y - projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
				if (distance < 480f && !target.friendly && target.active)
				{
					if (projectile.ai[0] > 30 && Main.rand.NextFloat() < .25f && Main.netMode != 1)
					{
						distance = 3f / distance;
						shootToX *= distance * 5;
						shootToY *= distance * 5;
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<damagebullet>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].netUpdate = true;
						projectile.netUpdate = true;
						projectile.ai[0] = 0f;
					}
				}
			}

			Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 0.78f);
			#endregion
		}
	}
}