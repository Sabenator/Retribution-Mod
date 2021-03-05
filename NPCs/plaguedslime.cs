using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;

namespace Retribution.NPCs
{
	public class plaguedslime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Crimslime];
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 33;
			npc.damage = 18;
			npc.defense = 6;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 60f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 1;
			aiType = NPCID.Crimslime;
			animationType = NPCID.Crimslime;
			banner = Item.NPCtoBanner(NPCID.Crimslime);
			bannerItem = Item.BannerToItem(banner);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.GetModPlayer<RetributionPlayer>().ZoneCystic ? .4f : 0f;
		}

		public override void NPCLoot()
		{
			for (int d = 0; d < 20; d++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 277, 0f, 0f, 150, default, 1.5f);
			}
		}
	}
}
