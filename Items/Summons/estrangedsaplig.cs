using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.NPCs.Bosses.Silva;

namespace Retribution.Items.Summons
{
	public class estrangedsapling : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Estranged Sapling");
			Tooltip.SetDefault("Emerges Silva from the ground\nCan only be used on the ground");
		}

		public override void SetDefaults()
		{
			item.width = 56;
			item.height = 56;
			item.maxStack = 20;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item1;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.velocity.Y == 0 && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Silva.Silva>());
		}

		public override bool UseItem(Player player)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 250, ModContent.NPCType<Silva>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}
}