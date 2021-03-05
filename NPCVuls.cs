using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.NPCs;
using Retribution.NPCs.Bosses.Silva;
using Retribution.NPCs.Bosses.Tesca;

namespace Retribution
{
	public class NPCVuls : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public static bool toFire = false;
		public static bool toWater = false;
		public static bool toIce = false;
		public static bool toPoison = false;

		public static bool fireImmune = false;
		public static bool waterImmune = false;
		public static bool iceImmune = false;
		public static bool confuseImmune = false;

		public override void SetDefaults(NPC npc)
        {
			#region toFire
			if (npc.type == ModContent.NPCType<Silva>() || npc.type == ModContent.NPCType<Tesca>() || npc.type == NPCID.IceSlime || npc.type == NPCID.ZombieEskimo || npc.type == NPCID.IceElemental || npc.type == NPCID.Wolf || npc.type == NPCID.IceGolem || npc.type == NPCID.IceBat || npc.type == NPCID.SnowFlinx || npc.type == NPCID.SpikedIceSlime || npc.type == NPCID.IceTortoise || npc.type == NPCID.IcyMerman)
			{
				toFire = true;
				toWater = false;
				toIce = false;
				toPoison = false;
				iceImmune = true;
			}
			#endregion

			#region toWater
			if (npc.type == NPCID.Hellbat || npc.type == NPCID.LavaSlime || npc.type == NPCID.FireImp || npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.Lavabat || npc.type == NPCID.RedDevil || npc.type == NPCID.HellArmoredBones || npc.type == NPCID.HellArmoredBonesMace || npc.type == NPCID.HellArmoredBonesSpikeShield || npc.type == NPCID.HellArmoredBonesSword)
			{
				toWater = true;
				toFire = false;
				toIce = false;
				toPoison = false;
				fireImmune = true;
			}
			#endregion

			#region toIce
			if (npc.type == NPCID.PinkJellyfish || npc.type == NPCID.BlueJellyfish || npc.type == NPCID.GreenJellyfish || npc.type == NPCID.Crab || npc.type == NPCID.Squid || npc.type == NPCID.SeaSnail || npc.type == NPCID.Shark || npc.type == NPCID.DukeFishron || npc.type == NPCID.Sharkron || npc.type == NPCID.Sharkron2)
			{
				toIce = true;
				toWater = false;
				toFire = false;
				toPoison = false;
				waterImmune = true;
			}
			#endregion

			#region toPoison
			if (npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.BoneSerpentBody || npc.type == NPCID.BoneSerpentTail || npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle || npc.type == NPCID.ArmoredSkeleton || npc.type == NPCID.ArmoredViking || npc.type == NPCID.BlueArmoredBones || npc.type == NPCID.BlueArmoredBonesMace || npc.type == NPCID.BlueArmoredBonesNoPants || npc.type == NPCID.BlueArmoredBonesSword || npc.type == NPCID.BoneLee || npc.type == NPCID.CursedSkull || npc.type == NPCID.DarkCaster || npc.type == NPCID.DiabolistRed || npc.type == NPCID.DiabolistWhite || npc.type == NPCID.GiantCursedSkull || npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored || npc.type == NPCID.RaggedCaster || npc.type == NPCID.RuneWizard || npc.type == NPCID.RustyArmoredBonesAxe || npc.type == NPCID.RustyArmoredBonesFlail || npc.type == NPCID.RustyArmoredBonesSword || npc.type == NPCID.RustyArmoredBonesSwordNoArmor || npc.type == NPCID.SkeletonArcher || npc.type == NPCID.SkeletonCommando || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.SkeletronHand || npc.type == NPCID.SkeletronHead || npc.type == NPCID.TacticalSkeleton || npc.type == NPCID.Tim || npc.type == NPCID.UndeadMiner || npc.type == NPCID.UndeadViking || npc.type == NPCID.Skeleton || npc.type == NPCID.SmallSkeleton || npc.type == NPCID.BigSkeleton || npc.type == NPCID.HeadacheSkeleton || npc.type == NPCID.SmallHeadacheSkeleton || npc.type == NPCID.BigHeadacheSkeleton || npc.type == NPCID.MisassembledSkeleton || npc.type == NPCID.SmallMisassembledSkeleton || npc.type == NPCID.BigMisassembledSkeleton || npc.type == NPCID.PantlessSkeleton || npc.type == NPCID.SmallPantlessSkeleton || npc.type == NPCID.BigPantlessSkeleton || npc.type == NPCID.SkeletonTopHat || npc.type == NPCID.SkeletonAstonaut || npc.type == NPCID.SkeletonAlien)
			{
				toPoison = true;
				toWater = false;
				toIce = false;
				toFire = false;
				iceImmune = true;
			}
			#endregion

			if (fireImmune)
			{
				npc.buffImmune[BuffID.OnFire] = true;
			}
            if (toFire)
            {
				npc.buffImmune[BuffID.OnFire] = false;
			}

			if (waterImmune)
			{
				npc.buffImmune[BuffID.Wet] = true;
			}
			if (toWater)
			{
				npc.buffImmune[BuffID.Wet] = false;
			}

			if (iceImmune)
			{
				npc.buffImmune[BuffID.Frostburn] = true;
			}
			if (toIce)
			{
				npc.buffImmune[BuffID.Frostburn] = false;
			}

			if (confuseImmune)
			{
				npc.buffImmune[BuffID.Confused] = true;
			}
			if (toPoison)
			{
				npc.buffImmune[BuffID.Poisoned] = false;
				npc.buffImmune[BuffID.Venom] = false;
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
			if (RetributionWorld.nightmareMode == true)
			{
				if (toFire == true && npc.HasBuff(BuffID.OnFire))
				{
					if (npc.lifeRegen < 0)
					{
						npc.lifeRegen = 0;
					}
					npc.lifeRegen -= 50;
				}

				if (toWater == true && npc.HasBuff(BuffID.Wet))
				{
					if (npc.lifeRegen < 0)
					{
						npc.lifeRegen = 0;
					}
					npc.lifeRegen -= 50;
				}

				if (toIce == true && npc.HasBuff(BuffID.Frostburn))
				{
					if (npc.lifeRegen < 0)
					{
						npc.lifeRegen = 0;
					}
					npc.lifeRegen -= 50;
				}

				if (toPoison == true && npc.HasBuff(BuffID.Poisoned) || npc.HasBuff(BuffID.Venom))
				{
					if (npc.lifeRegen < 0)
					{
						npc.lifeRegen = 0;
					}
					npc.lifeRegen -= 50;
				}
			}
		}
    }
}