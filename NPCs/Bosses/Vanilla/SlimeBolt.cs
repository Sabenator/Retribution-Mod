using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Retribution.Buffs;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution.NPCs.Bosses.Vanilla
{
	public class SlimeBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 100;
			projectile.timeLeft = 120;
			aiType = 0;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Slimed, 60);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item17, (int)projectile.position.X, (int)projectile.position.Y);

			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.t_Slime, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(133, 196, 255), 2f);
				Main.dust[num].noGravity = true;
			}
		}

        public override void AI()
        {
			Lighting.AddLight(projectile.Center, 0f, 0.258f, 1f);
			int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.t_Slime, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(133, 196, 255));
			Main.dust[i].noGravity = true;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
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
