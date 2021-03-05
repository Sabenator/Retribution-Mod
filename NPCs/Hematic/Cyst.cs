using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;

namespace Retribution.NPCs.Hematic
{
	public class Cyst : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyst");
		}

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 46;
			npc.damage = 25;
			npc.defense = 0;
			npc.lifeMax = 120;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			banner = Item.NPCtoBanner(NPCID.Crimslime);
			bannerItem = Item.BannerToItem(banner);
			RetributionNPC.hematicEnemy = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.GetModPlayer<RetributionPlayer>().ZoneHematic ? 2f : 0f;
		}

		public override void AI()
        {
			npc.ai[0]++;

			if (npc.ai[0] > 300)
			{
				NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-20, 20), (int)npc.Center.Y - 40, ModContent.NPCType<Enzyme>());
				npc.ai[0] = 0;
			}
        }
	}
}
