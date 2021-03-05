using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;
using Microsoft.Xna.Framework;

namespace Retribution.NPCs.Hematic
{
	public class Enzyme : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enzyme");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width = 4;
			npc.height = 4;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			banner = Item.NPCtoBanner(NPCID.Crimslime);
			bannerItem = Item.BannerToItem(banner);
			RetributionNPC.hematicEnemy = true;
		}

		bool light = true;
		bool flickerPass = false;
		bool doneSpawning = false;
		bool stopUp = false;

		int lightTimer = 0;
		int spawnTimer = 0;
		int lightCount = 0;
		int lightChangeCount = 0;
		int reproCount = 0;

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.GetModPlayer<RetributionPlayer>().ZoneHematic ? 50f : 0f;
		}

		public override void AI()
        {
			if (doneSpawning == false)
			{
				spawnTimer++;
				if (stopUp == false)
				{
					npc.velocity.Y += 0.01f;
				}

				if (spawnTimer > 30)
				{
					npc.velocity.Y += 0.01f;
					stopUp = true;
				}

				if (spawnTimer > 60)
				{
					doneSpawning = true;
				}
			}

            #region Behaviour
            npc.rotation += 0.03f;

			if (Main.rand.NextFloat() < .05f)
			{
				npc.velocity.X += Main.rand.NextFloat(0.03f, 0.1f);
			}

			if (Main.rand.NextFloat() < .05f)
			{
				npc.velocity.X -= Main.rand.NextFloat(0.03f, 0.1f);
			}

			if (Main.rand.NextFloat() < .05f)
			{
				npc.velocity.Y += Main.rand.NextFloat(0.03f, 0.1f);
			}

			if (Main.rand.NextFloat() < .05f)
			{
				npc.velocity.Y -= Main.rand.NextFloat(0.03f, 0.1f);
			}

			if (npc.velocity.X >= 0.5f && doneSpawning == true)
			{
				npc.velocity.X = 0.05f;
			}

			if (npc.velocity.Y >= 0.5f && doneSpawning == true)
			{
				npc.velocity.Y = 0.05f;
			}

			if (npc.velocity.X <= -0.5f && doneSpawning == true)
			{
				npc.velocity.X = 0.05f;
			}

			if (npc.velocity.Y <= -0.5f && doneSpawning == true)
			{
				npc.velocity.Y = 0.05f;
			}
            #endregion

            #region Lighting
            if (light == true)
			{
				Lighting.AddLight(npc.Center, 0.81f, 0.71f, 0.23f);
			}

			if (light == false && lightCount == 0)
			{
				lightTimer++;

				if (lightTimer < 10)
				{
					Lighting.AddLight(npc.Center, 0.71f, 0.61f, 0.13f);
				}

				if (lightTimer < 30)
				{
					Lighting.AddLight(npc.Center, 0.61f, 0.51f, 0f);
				}

				if (lightTimer < 45)
				{
					Lighting.AddLight(npc.Center, 0.41f, 0.31f, 0f);
				}

				if (lightTimer < 60)
				{
					Lighting.AddLight(npc.Center, 0f, 0f, 0f);
					lightTimer = 0;
					lightCount = 1;
				}
			}

			npc.ai[0]++;
			if (npc.ai[0] > 80 && Main.rand.NextFloat() < .4f)
			{
				flickerPass = true;
			}
			if (npc.ai[0] > 81)
			{
				npc.ai[0] = 0;
			}

			if (flickerPass == true)
			{
				npc.ai[1]++;
				light = false;
				lightCount = 0;

				if (npc.ai[1] > Main.rand.Next(60, 120))
				{
					light = true;
					flickerPass = false;
					npc.ai[0] = 0;
					npc.ai[1] = 0;
				}
			}
			#endregion

			#region Reproduce

			npc.ai[2]++;

			if (npc.ai[2] > 1000 && reproCount < 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Enzyme>());
				reproCount++;
			}
			#endregion

			#region Die
			if (npc.ai[2] > 1400)
			{
				npc.alpha -= 5;
			}

			if (npc.ai[2] > 1500)
			{
				npc.life = 0;
			}
            #endregion
        }

        private double counting;

		public override void FindFrame(int frameHeight)
		{
			counting += 0.5;
			if (counting < 3.0)
			{
				npc.frame.Y = 0;
			}
			else if (counting < 6.0)
			{
				npc.frame.Y = frameHeight;
			}
			else if (counting < 8.0)
			{
				npc.frame.Y = frameHeight * 2;
			}
			else if (counting < 12.0)
			{
				npc.frame.Y = frameHeight * 3;
			}
			else if (counting < 15.0)
			{
				npc.frame.Y = frameHeight * 4;
			}
			else if (counting < 18.0)
			{
				npc.frame.Y = frameHeight * 5;
			}
			else if (counting < 21.0)
			{
				npc.frame.Y = frameHeight * 6;
			}
			else if (counting < 24.0)
			{
				npc.frame.Y = frameHeight * 7;
			}
			else if (counting < 27.0)
			{
				npc.frame.Y = frameHeight * 8;
			}
			else if (counting < 30.0)
			{
				npc.frame.Y = frameHeight * 9;
			}
			else
			{
				counting = 0.0;
			}
		}


		public override void NPCLoot()
		{
			for (int d = 0; d < 20; d++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.AmberBolt, 0f, 0f, 150, default, 1.5f);
			}
		}
	}
}
