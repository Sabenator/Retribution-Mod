using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Retribution.Tiles;

namespace Retribution.Tiles
{
    public class Cyaton : ModTile
    {
        public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 410;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1000;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cyaton");
			AddMapEntry(new Color(200, 200, 200), name);

			dustType = 84;
			drop = ModContent.ItemType<Items.Blocks.Hematite>();
			soundType = SoundID.Tink;
			soundStyle = 1;
			minPick = 180;
		}
	}
}