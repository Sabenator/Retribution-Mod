using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Retribution.Buffs;

namespace Retribution.Projectiles.Minions
{
	public class IceCrystalProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 0;
			projectile.alpha = 255;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextFloat() < .2f && Main.netMode != 1)
			{
				target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 120);
				projectile.netUpdate = true;
			}
		}

		public override void AI()
		{
			if (projectile.timeLeft == 146)
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
			if (projectile.timeLeft < 146)
			{
				for (int i = 0; i < 3; i++)
				{
					float num4 = projectile.velocity.X / 3f * (float)i;
					float num5 = projectile.velocity.Y / 3f * (float)i;
					int num6 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 185, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num6].position.X = projectile.Center.X - num4;
					Main.dust[num6].position.Y = projectile.Center.Y - num5;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity *= 0f;
				}
			}
			if (projectile.timeLeft < 146)
			{
				projectile.alpha = 50;
			}
			if (projectile.timeLeft > 125)
			{
				float num132 = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
				float num133 = projectile.localAI[0];
				if (num133 == 0f)
				{
					projectile.localAI[0] = num132;
					num133 = num132;
				}
				float num134 = projectile.position.X;
				float num135 = projectile.position.Y;
				float num136 = 300f;
				bool flag3 = false;
				int num137 = 0;
				if (projectile.ai[1] == 0f)
				{
					for (int num138 = 0; num138 < 200; num138++)
					{
						if (Main.npc[num138].CanBeChasedBy(this, false) && (projectile.ai[1] == 0f || projectile.ai[1] == (float)(num138 + 1)))
						{
							float num139 = Main.npc[num138].position.X + (float)(Main.npc[num138].width / 2);
							float num140 = Main.npc[num138].position.Y + (float)(Main.npc[num138].height / 2);
							float num141 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num139) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num140);
							if (num141 < num136 && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
							{
								num136 = num141;
								num134 = num139;
								num135 = num140;
								flag3 = true;
								num137 = num138;
							}
						}
					}
					if (flag3)
					{
						projectile.ai[1] = (float)(num137 + 1);
					}
					flag3 = false;
				}
				if (projectile.ai[1] > 0f)
				{
					int num142 = (int)(projectile.ai[1] - 1f);
					if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
					{
						float num143 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
						float num144 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
						if (Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num143) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num144) < 1000f)
						{
							flag3 = true;
							num134 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
							num135 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
						}
					}
					else
					{
						projectile.ai[1] = 0f;
					}
				}
				if (!projectile.friendly)
				{
					flag3 = false;
				}
				if (flag3)
				{
					float num145 = num133;
					Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num146 = num134 - vector10.X;
					float num147 = num135 - vector10.Y;
					float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
					num148 = num145 / num148;
					num146 *= num148;
					num147 *= num148;
					int num149 = 8;
					projectile.velocity.X = (projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
					projectile.velocity.Y = (projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
