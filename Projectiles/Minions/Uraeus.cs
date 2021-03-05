using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs.Summons;

namespace Retribution.Projectiles.Minions
{
	public class Uraeus : HoverShooter
	{
		public override void SetStaticDefaults()
		{
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projFrames[projectile.type] = 7;
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
			inertia = 25f;
			shoot = ModContent.ProjectileType<Venom>();
			shootSpeed = 5f;
			chaseAccel = 6f;
			randomBehavior = true;
			shootCool = Main.rand.Next(45, 120);
		}

		public override void CheckActive()
		{
			Player player = Main.player[projectile.owner];
			RetributionPlayer modPlayer = player.GetModPlayer<RetributionPlayer>();
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<uraeusbuff>());
			}
			if (player.HasBuff(ModContent.BuffType<uraeusbuff>()))
			{
				projectile.timeLeft = 2;
			}
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