using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Pharaoh
{
	public class SandSpikeWarn : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
		}

		float lineWidth = 25f;

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}

		public override void AI()
		{
			lineWidth -= 1f;

			projectile.ai[0] += 1f;
			if (projectile.ai[0] <= 20f)
			{
				if (projectile.alpha <= 255)
				{
					projectile.alpha -= 12;
					return;
				}
			}
			else
			{
				if (projectile.ai[0] == 22f)
				{
					int i = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ModContent.ProjectileType<SandSpike>(), 55, 0f, Main.myPlayer, 0f, 40f);
					Main.projectile[i].localAI[1] = 125f;
					Main.projectile[i].Center = projectile.Center;
					return;
				}
				if (projectile.ai[0] >= 22f)
				{
					projectile.alpha += 10;
					if (projectile.alpha >= 255)
					{
						projectile.Kill();
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.Draw(Main.magicPixel, projectile.position - new Vector2(3f, 4000f) - Main.screenPosition, null, new Color(237, 187, 38) * (1f - (float)projectile.alpha / 255f), projectile.rotation, Vector2.One, new Vector2(lineWidth, 4f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Main.magicPixel, projectile.position - new Vector2(3f, 4000f) - Main.screenPosition, null, new Color(237, 187, 38) * (1f - (float)projectile.alpha / 255f), projectile.rotation, Vector2.Zero, new Vector2(lineWidth, 4f), SpriteEffects.None, 0f);

			return false;
		}
	}
}