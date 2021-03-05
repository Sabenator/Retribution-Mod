using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Projectiles;

namespace Retribution.NPCs.Bosses.Morbus
{
    public class morbi2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morbi");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 2;
            npc.lifeMax = 150;
            npc.damage = 40;
            npc.defense = 16;
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 22;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 300;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);

            const int NUM_DUSTS = 50;

            for (int i = 0; i < NUM_DUSTS; i++)
            {
                Dust dust;
                Vector2 position = npc.position;
                dust = Terraria.Dust.NewDustDirect(position, 45, 45, 75, 0f, 0f, 0, new Color(255, 226, 0), 1f);
            }

            npc.active = false;
        }

        public override void AI()
        {
			base.npc.TargetClosest(true);
			Vector2 vector = Main.player[base.npc.target].Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
			Vector2 vector2 = base.npc.Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
			base.npc.netUpdate = true;
			if (base.npc.wet)
			{
				NPC npc = base.npc;
				npc.velocity.Y = npc.velocity.Y - 0.5f;
			}
			base.npc.ai[0] += 1f;
			if (base.npc.ai[0] > 19f && base.npc.ai[0] < 21f)
			{
				Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 45, 1f, 0f);
				float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
				Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)(Math.Cos((double)num) * 5.0 * -1.0), (float)(Math.Sin((double)num) * 5.0 * -1.0), ModContent.ProjectileType<IchorBolt>(), 20, 0f, 0, 0f, 0f);
				npc.ai[0] = -180;
			}
		}
	}
	public class IchorBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Bolt");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 18;
			projectile.aiStyle = 0;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 260;
			projectile.light = 0.5f;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.extraUpdates = 2;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Ichor, 120);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			Vector2 usePos = projectile.position;

			Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
			usePos += rotVector * 16f;

			const int NUM_DUSTS = 20;

			for (int i = 0; i < NUM_DUSTS; i++)
			{
				Dust dust;
				dust = Main.dust[Terraria.Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, 0f, 50, new Color(0, 142, 255), 1f)];
				dust.noGravity = true;
			}
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			projectile.velocity.Y = projectile.velocity.Y + 0.001f;
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}

			Dust dust;
			Vector2 position = projectile.position;
			dust = Main.dust[Terraria.Dust.NewDust(position, 1, 1, DustID.Fire, 0f, 0f, 0, default(Color), 2.776316f)];
			dust.noGravity = true;

			Lighting.AddLight(projectile.position, 0.1f, 0.1f, 0.9f);
		}
	}
}