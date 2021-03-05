using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.NPCs.Bosses.Pharaoh
{
	public class SandSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Spike");
		}

		public override void SetDefaults()
		{
			projectile.width = 58;
			projectile.height = 106;
			projectile.scale = 1.2f;
			projectile.penetrate = 1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 0;
			projectile.timeLeft = 360;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}
	}
}
