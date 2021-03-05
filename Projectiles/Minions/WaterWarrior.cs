using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs.Summons;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution.Projectiles.Minions
{
	public class WaterWarrior : HoverShooter
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 24;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			inertia = 20f;
			shoot = ProjectileID.WaterBolt;
			shootSpeed =12f;
			chaseAccel = 4f;
			shootCool = 120f;
		}

        public override void CheckActive()
		{
			Player player = Main.player[projectile.owner];
			RetributionPlayer modPlayer = player.GetModPlayer<RetributionPlayer>();
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<waterwarriorbuff>());
			}
			if (player.HasBuff(ModContent.BuffType<waterwarriorbuff>()))
			{
				projectile.timeLeft = 2;
			}
		}

		public override void CreateDust()
		{
			if (projectile.ai[0] == 0f)
			{
				if (Main.rand.NextBool(5))
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height / 2, DustID.BlueCrystalShard);
					Main.dust[dust].velocity.Y -= 1.2f;
				}
			}
			else
			{
				if (Main.rand.NextBool(3))
				{
					Vector2 dustVel = projectile.velocity;
					if (dustVel != Vector2.Zero)
					{
						dustVel.Normalize();
					}
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard);
					Main.dust[dust].velocity -= 1.2f * dustVel;
				}
			}
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0f, 0f, 1f);
		}

		public override void SelectFrame()
		{
			int frameSpeed = 6;
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
		}
	}
}