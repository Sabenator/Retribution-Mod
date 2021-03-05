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
    public class tanzanite : ModTile
    {
        public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 410;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 500;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;


			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tanzanite");
			AddMapEntry(new Color(255, 0, 174), name);

			dustType = 84;
			drop = ModContent.ItemType<Items.Blocks.rubidiumore>();
			soundType = SoundID.Tink;
			soundStyle = 1;
			minPick = 35;
		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.5f;
			g = 0.5f;
			b = 0.8f;
		}
    }
}