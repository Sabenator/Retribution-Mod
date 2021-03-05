using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Retribution.Items.Hooks
{
	public abstract class BaseHook : ModProjectile
	{
		public virtual string ChainTexture
		{
			get
			{
				return this.Texture + "_Chain";
			}
		}

		public virtual void SetHookDefaults()
		{
		}

		public override void SetStaticDefaults()
		{
			Main.projHook[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.aiStyle = 7;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft *= 10;
			this.SetHookDefaults();
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture(this.ChainTexture);
			Vector2 vector = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 vector2 = mountedCenter - vector;
			if (Utils.HasNaNs(vector) || Utils.HasNaNs(vector2))
			{
				return;
			}
			float num = (float)texture.Height;
			float rotation = Utils.ToRotation(vector2) - 1.57079637f;
			while (vector2.Length() >= num + 1f)
			{
				vector += Vector2.Normalize(vector2) * num;
				vector2 = mountedCenter - vector;
				Color alpha = projectile.GetAlpha(Lighting.GetColor((int)vector.X / 16, (int)((double)vector.Y / 16.0)));
				spriteBatch.Draw(texture, vector - Main.screenPosition, null, alpha, rotation, Utils.Size(texture) * 0.5f, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
