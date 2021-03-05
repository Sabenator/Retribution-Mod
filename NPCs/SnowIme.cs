using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;
using Microsoft.Xna.Framework;

namespace Retribution.NPCs
{
	public class SnowIme : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SnowIme");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width = 4;
			npc.height = 4;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.alpha = 255;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.noTileCollide = true;
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
			return spawnInfo.player.ZoneSnow ? 100f : 0f;
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

			if (Main.rand.NextFloat() < .01f)
			{
				npc.velocity.X += Main.rand.NextFloat(0.5f, 1f);
			}

			if (Main.rand.NextFloat() < .01f)
			{
				npc.velocity.X -= Main.rand.NextFloat(0.5f, 1f);
			}

			if (Main.rand.NextFloat() < .01f)
			{
				npc.velocity.Y -= Main.rand.NextFloat(1f, 2f);
			}

			if (npc.velocity.X >= 2f && doneSpawning == true)
			{
				npc.velocity.X = 1.5f;
			}

			if (npc.velocity.X <= -2f && doneSpawning == true)
			{
				npc.velocity.X = 1.5f;
			}

			if (npc.velocity.Y <= -2f && doneSpawning == true)
			{
				npc.velocity.Y = 1.5f;
			}
			#endregion

			#region Lighting
			Lighting.AddLight(npc.Center, 0.1f, 0.2f, 0.6f);
			#endregion

			#region Reproduce

			npc.ai[2]++;

			if (npc.ai[2] > 1000 && reproCount < 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SnowIme>());
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

			if (!Main.LocalPlayer.ZoneSnow)
			{
				npc.life = 0;
			}

			Dust dust;
			Vector2 position = npc.position;
			dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
			dust.noGravity = true;
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
				Dust.NewDust(Main.screenPosition + new Vector2(Main.rand.Next(0, 100)), 1, 1, DustID.AmberBolt, 0f, 0f, 150, default, 1.5f);
			}
		} 
	}
}
