using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Tesca
{
	public class tescanattachspawn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tescan Spike");
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.aiStyle = 0;
			projectile.alpha = 100;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (projectile.timeLeft == 180)
			{
				for (int i = 0; i < 15; i++)
				{
					int num = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 185, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 1f);
					Main.dust[num].noGravity = true;
				}
			}
			projectile.ai[1] += 1f;
			if (projectile.timeLeft < 60)
			{
				if (projectile.ai[1] > 10f)
				{
					float num2 = 15f;
					int num3 = 0;
					while ((float)num3 < num2)
					{
						Vector2 vector = Vector2.UnitX * 0f;
						vector += -Utils.RotatedBy(Vector2.UnitY, (double)((float)num3 * (6.28318548f / num2)), default(Vector2)) * new Vector2(20f, 8f);
						vector = Utils.RotatedBy(vector, (double)Utils.ToRotation(projectile.velocity), default(Vector2));
						int num4 = Dust.NewDust(projectile.Center, 0, 0, 185, 0f, 0f, 0, default(Color), 1f);
						Main.dust[num4].noGravity = true;
						Main.dust[num4].position = projectile.Center + vector;
						Main.dust[num4].velocity = projectile.velocity * 0f + Utils.SafeNormalize(vector, Vector2.UnitY) * 0f;
						num3++;
					}
				}
			}
			else if (projectile.timeLeft < 90)
			{
				if (projectile.ai[1] > 20f)
				{
					float num5 = 15f;
					int num6 = 0;
					while ((float)num6 < num5)
					{
						Vector2 vector2 = Vector2.UnitX * 0f;
						vector2 += -Utils.RotatedBy(Vector2.UnitY, (double)((float)num6 * (6.28318548f / num5)), default(Vector2)) * new Vector2(20f, 8f);
						vector2 = Utils.RotatedBy(vector2, (double)Utils.ToRotation(projectile.velocity), default(Vector2));
						int num7 = Dust.NewDust(projectile.Center, 0, 0, 185, 0f, 0f, 0, default(Color), 1f);
						Main.dust[num7].noGravity = true;
						Main.dust[num7].position = projectile.Center + vector2;
						Main.dust[num7].velocity = projectile.velocity * 0f + Utils.SafeNormalize(vector2, Vector2.UnitY) * 0f;
						num6++;
					}
				}
			}
			else if (projectile.timeLeft < 120 && projectile.ai[1] > 30f)
			{
				float num8 = 15f;
				int num9 = 0;
				while ((float)num9 < num8)
				{
					Vector2 vector3 = Vector2.UnitX * 0f;
					vector3 += -Utils.RotatedBy(Vector2.UnitY, (double)((float)num9 * (6.28318548f / num8)), default(Vector2)) * new Vector2(20f, 8f);
					vector3 = Utils.RotatedBy(vector3, (double)Utils.ToRotation(projectile.velocity), default(Vector2));
					int num10 = Dust.NewDust(projectile.Center, 0, 0, 185, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num10].noGravity = true;
					Main.dust[num10].position = projectile.Center + vector3;
					Main.dust[num10].velocity = projectile.velocity * 0f + Utils.SafeNormalize(vector3, Vector2.UnitY) * 0f;
					num9++;
				}
			}
			if (projectile.timeLeft > 50)
			{
				projectile.position.X = player.Center.X - 25f;
				projectile.position.Y = player.Center.Y - 25f + 50f;
			}
			if (projectile.timeLeft == 5)
			{
				Main.PlaySound(SoundID.Item30, (int)projectile.position.X, (int)projectile.position.Y);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 10f, 0f, -7f, ModContent.ProjectileType<tescanattach>(), 25, 0f, 0, 0f, 0f);
			}
		}
	}
}
