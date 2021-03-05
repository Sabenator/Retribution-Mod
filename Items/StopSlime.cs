using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using SubworldLibrary;

namespace Retribution.Items
{
	public class StopSlime : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti-Slimy Insignia");
			Tooltip.SetDefault("Summons the no Slime Rain");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 34;
			item.maxStack = 20;
			item.rare = ItemRarityID.Cyan;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

        public override bool UseItem(Player player)
        {
			Main.StopSlimeRain(true);
			return true;
        }
    }
}