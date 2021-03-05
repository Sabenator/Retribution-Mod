using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs;
using Microsoft.Xna.Framework;
using System;
using Retribution.NPCs.Bosses.Tesca;

namespace Retribution.NPCs.SlimeRain
{
	public class CommandoSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commando Slime");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 30;
			npc.damage = 8;
			npc.defense = 3;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0.5f;
			npc.noGravity = false;
			npc.aiStyle = -1;
		}

		private double counting;
		private bool moving = true;
		private bool startShoot = false;

		private int shootX;

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.slimeRain ? 0.5f : 0f;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 1.6f);
		}
		public override void AI()
        {
            #region Behavior
            Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			Vector2 targetPosition = Main.player[npc.target].position;

			if (player.Center.X > npc.Center.X)
			{
				npc.spriteDirection = -1;
			}
			else
			{
				npc.spriteDirection = 1;
			}

			if (moving == true)
			{
				if (targetPosition.X < npc.position.X)
				{
					npc.velocity.X = -0.5f;
				}

				if (targetPosition.X > npc.position.X)
				{
					npc.velocity.X = 0.5f;
				}
			}
			else if (moving == false)
			{
				npc.velocity.X = 0;
			}
			#endregion

			npc.ai[0]++;

			if (npc.ai[0] > 300)
			{
				moving = false;
				startShoot = true;
				npc.ai[0] = 0;
			}

			if (npc.spriteDirection == -1)
			{
				shootX = 10;
			}
			else if (npc.spriteDirection == 1)
			{
				shootX = -10;
			}

			if (startShoot == true)
			{
				npc.ai[1]++;

				if (npc.ai[1] > 120)
				{
					Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);

					npc.TargetClosest(true);
					npc.netUpdate = true;
					Vector2 vector = Main.player[npc.target].Center + new Vector2(npc.Center.X, npc.Center.Y);
					Vector2 vector2 = npc.Center + new Vector2(npc.Center.X, npc.Center.Y);
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
					float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
					int i = Projectile.NewProjectile(npc.Center.X + shootX, npc.Center.Y + 5, (float)(Math.Cos((double)num) * 2.0 * -1.0), (float)(Math.Sin((double)num) * 2.0 * -1.0), ProjectileID.RocketI, 15, 0f, 0, 0f, 0f);
					Main.projectile[i].friendly = false;
					Main.projectile[i].penetrate = 1;
					Main.projectile[i].hostile = true;

					for (int k = 0; k < 40; k++)
					{
						Dust.NewDust(Main.projectile[i].Center, 1, 1, 31, Main.projectile[i].velocity.X * 2, Main.projectile[i].velocity.Y * 2);
						Dust.NewDust(Main.projectile[i].Center, 1, 1, 31, Main.projectile[i].velocity.X, Main.projectile[i].velocity.Y);
						Dust.NewDust(Main.projectile[i].Center, 1, 1, DustID.Fire, Main.projectile[i].velocity.X, Main.projectile[i].velocity.Y);
					}

					npc.ai[1] = 0;
					moving = true;

					startShoot = false;
				}
			}
        }

        public override void FindFrame(int frameHeight)
		{
			if (moving == true)
			{
				counting += 1.0;
				if (counting < 6.0)
				{
					npc.frame.Y = 0;
				}
				else if (counting < 12.0)
				{
					npc.frame.Y = frameHeight;
				}
				else if (counting < 18.0)
				{
					npc.frame.Y = frameHeight * 2;
				}
				else
				{
					counting = 0.0;
				}
			}
			else if (moving == false)
			{
				counting += 3.0;
				if (counting < 6.0)
				{
					npc.frame.Y = 0;
				}
				else if (counting < 12.0)
				{
					npc.frame.Y = frameHeight;
				}
				else if (counting < 18.0)
				{
					npc.frame.Y = frameHeight * 2;
				}
				else
				{
					counting = 0.0;
				}
			}
		}

		public override void NPCLoot()
		{
			for (int d = 0; d < 20; d++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 101, 0f, 0f, 150);
			}
		}
	}
}
