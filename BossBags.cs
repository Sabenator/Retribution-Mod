using System.IO;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
using Retribution.Items.Weapons.Reaper;
using Retribution.Items.Weapons.Melee;
using Retribution.Items.Weapons.Summoner;
using Retribution.Items.Weapons.Mage;
using Retribution.Items.Weapons.Ranger;
using Retribution.Items.Accessories;

namespace Retribution
{
    public class BossBags : GlobalItem
    {
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
			{
				if (Main.rand.NextFloat() < .33f)
				{
					player.QuickSpawnItem(ModContent.ItemType<snapshot>());
				}

				if (Main.rand.NextFloat() < .33f)
				{
					player.QuickSpawnItem(ModContent.ItemType<peeperstaff>());
				}
			}

			if (context == "bossBag" && arg == ItemID.KingSlimeBossBag)
			{
				if (Main.rand.NextFloat() < .33f)
				{
					player.QuickSpawnItem(ModContent.ItemType<kingskatana>());
				}
			}

			if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag)
			{
				if (Main.expertMode && Main.rand.NextFloat() < .25f)
				{
					player.QuickSpawnItem(ModContent.ItemType<reaperemblem>());
				}
			}
		}
	}
}