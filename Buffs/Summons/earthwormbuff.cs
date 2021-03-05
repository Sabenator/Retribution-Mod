using Retribution.Dusts;
using Retribution.Projectiles.Minions.Worms;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Buffs.Summons
{	public class earthwormbuff : ModBuff
	{
		public override void SetDefaults()
		{
			base.DisplayName.SetDefault("Earth Worm");
			base.Description.SetDefault("The Earth Worm will eat away at your enemies.");
			Main.buffNoSave[base.Type] = true;
			Main.buffNoTimeDisplay[base.Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			RetributionPlayer rP = (RetributionPlayer)player.GetModPlayer<RetributionPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EarthHead>()] > 0)
			{
				rP.eWormMinion = true;
			}
			if (!rP.eWormMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}