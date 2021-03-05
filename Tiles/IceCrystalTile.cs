using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;

namespace Retribution.Tiles
{
	public class IceCrystalTile : ModTile
	{
		public override void SetDefaults()
		{
			AddMapEntry(new Color(55, 84, 125));

			Main.tileBrick[Type] = true;
			Main.tileValue[Type] = 410;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 600;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;

			dustType = DustID.Ice;
			drop = ModContent.ItemType<Items.Blocks.IceCrystalBlock>();
			soundType = SoundID.Item;
			soundStyle = 50;
			minPick = 180;
		}
	}
}