using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Retribution.Buffs;
using Terraria.ID;

namespace Retribution.NPCs.Bosses.Silva
{
	public class SharpenedBranch : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sharpened Branch");
		}

		public override void SetDefaults()
		{
			base.projectile.width = 58;
			base.projectile.height = 106;
			base.projectile.aiStyle = -1;
			base.projectile.hostile = true;
			base.projectile.tileCollide = false;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 45;
			this.aiType = 14;
		}

		public override void AI()
		{
			base.projectile.velocity *= 0.965f;
			if (base.projectile.timeLeft < 20)
			{
				base.projectile.alpha += 10;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (RetributionWorld.nightmareMode == true)
			{
				target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 60);
			}
			else
			{
				target.AddBuff(BuffID.Frostburn, 180);
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item17, (int)base.projectile.position.X, (int)base.projectile.position.Y);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1, -2, ModContent.ProjectileType<RazorLeaf>(), 10, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1, -2, ModContent.ProjectileType<RazorLeaf>(), 10, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1, 2, ModContent.ProjectileType<RazorLeaf>(), 10, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1, 2, ModContent.ProjectileType<RazorLeaf>(), 10, 0f);


			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, DustID.Dirt, base.projectile.velocity.X * 0.2f, base.projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
				Main.dust[num].noGravity = true;
			}
		}
	}
}
