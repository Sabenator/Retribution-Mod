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
	public class Crucifix : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crucifix");
			Tooltip.SetDefault("Pretty plain if you ask me.");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 34;
			item.maxStack = 20;
			item.rare = ItemRarityID.White;
		}
    }
}