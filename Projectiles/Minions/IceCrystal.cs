using Retribution.Dusts;
using Retribution.Buffs;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Retribution.Projectiles.Minions;

namespace Retribution.Projectiles.Minions
{
	public class IceCrystal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Crystal");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 58;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
			projectile.sentry = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.Center = player.Center;
			projectile.position.Y = player.position.Y - 70;

			#region Animation and visuals
			projectile.rotation = projectile.velocity.X * 0.05f;

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

			for (int i = 0; i < 200; i++)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC target = Main.npc[player.MinionAttackTargetNPC];
					float shootToX = target.position.X + (float)target.width * 0.8f - projectile.Center.X;
					float shootToY = target.position.Y + (float)target.height * 0.8f - projectile.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					if (distance < 700f && !target.friendly && target.active)
					{
						if (projectile.ai[0] > 60f)
						{
							distance = 1.6f / distance;

							shootToX *= distance * 10;
							shootToY *= distance * 10;
							int damage = 25;

							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<IceCrystalProj>(), damage, 0, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item30, projectile.Center);
							projectile.ai[0] = 0f;
						}
					}
				}
				else
				{
					NPC target = Main.npc[i];
					float shootToX = target.position.X + (float)target.width * 0.8f - projectile.Center.X;
					float shootToY = target.position.Y + (float)target.height * 0.8f - projectile.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					if (distance < 700f && !target.friendly && target.active)
					{
						if (projectile.ai[0] > 60f)
						{
							distance = 1.6f / distance;

							shootToX *= distance * 10;
							shootToY *= distance * 10;
							int damage = 45;

							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<IceCrystalProj>(), damage, 0, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item30, projectile.Center);
							projectile.ai[0] = 0f;
						}
					}
				}
			}
			projectile.ai[0] += 1f;

			Lighting.AddLight(projectile.Center, Color.Blue.ToVector3() * 0.78f);
			#endregion
		}
	}
}