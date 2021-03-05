using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Retribution.Projectiles
{
	public class BubbleEffect : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
		}

		private int rCount = 5;
		private int rSize = 5;
		private int rSpeed = 2;
		private float distortStrength = 800f;

		public override void AI()
		{
			projectile.ai[0] += 8f;
			Player player = Main.player[Main.myPlayer];
			if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
				Filters.Scene.Activate("Shockwave", projectile.Center, new object[0]).GetShader().UseColor((float)this.rCount, (float)this.rSize, (float)this.rSpeed).UseTargetPosition(projectile.Center);
				return;
			}
			else
			{
				projectile.ai[1] += 1f;
				float num = projectile.ai[1] / 60f;
				float num2 = 200f;
				Filters.Scene["Shockwave"].GetShader().UseProgress(num).UseOpacity(num2 * (1f - num / 3f));
			}
		}

		public override void Kill(int timeLeft)
		{
			Filters.Scene["Shockwave"].Deactivate(new object[0]);
		}
	}
}
