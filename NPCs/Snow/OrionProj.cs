using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Retribution.Buffs;

namespace Retribution.NPCs.Snow
{
	public class OrionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Bolt");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.alpha = 255;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
			aiType = 0;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (RetributionWorld.nightmareMode == true)
			{
				target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 120);
			}
			else
			{
				target.AddBuff(BuffID.Frostburn, 180);
			}
		}

		public override void AI()
		{
			if (projectile.timeLeft == 140)
			{
				float num = 30f;
				int num2 = 0;
				while ((float)num2 < num)
				{
					Vector2 vector = Vector2.UnitX * 0f;
					vector += -Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.28318548f / num)), default(Vector2)) * new Vector2(6f, 16f);
					vector = Utils.RotatedBy(vector, (double)Utils.ToRotation(projectile.velocity), default(Vector2));
					int num3 = Dust.NewDust(projectile.Center, 0, 0, 185, 0f, 0f, 0, default(Color), 1.25f);
					Main.dust[num3].noGravity = true;
					Main.dust[num3].position = projectile.Center + vector;
					Main.dust[num3].velocity = projectile.velocity * 0f + Utils.SafeNormalize(vector, Vector2.UnitY) * 1f;
					num2++;
				}
			}
			if (projectile.timeLeft < 140)
			{
				for (int i = 0; i < 3; i++)
				{
					int num6 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 185, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num6].position.X = projectile.Center.X;
					Main.dust[num6].position.Y = projectile.Center.Y;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity *= 0f;
				}
			}
			if (projectile.timeLeft < 140)
			{
				projectile.alpha = 50;
			}
		}
	}
}
