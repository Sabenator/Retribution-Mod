using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Retribution.Items.Summons
{
	public class rottenfang : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rotten Fang");
			Tooltip.SetDefault("Summons Morbus the Corruption Guardian");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 30;
			item.maxStack = 20;
			item.rare = ItemRarityID.Cyan;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ZoneCorrupt && !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Morbus.Morbus>());
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.Morbus.Morbus>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}
}