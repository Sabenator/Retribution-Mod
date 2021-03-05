using Retribution.Dusts;
using Retribution.Projectiles.Minions;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Items.Accessories.Special;

namespace Retribution.Buffs
{	public class StormCloudBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Storm Cloud");
			Description.SetDefault("The Storm Clouds will protect you");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RetributionPlayer>().orbThunder = true;
		}
	}
}