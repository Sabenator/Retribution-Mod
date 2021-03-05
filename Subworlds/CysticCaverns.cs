using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;
using System.Collections.Generic;
using Terraria.World.Generation;
using Retribution.Tiles;
using Terraria.DataStructures;
using StructureHelper;

public class CysticCaverns : Subworld
{
	public override int width => 600;
	public override int height => 600;

	public override ModWorld modWorld => ModContent.GetInstance <RetributionWorld>();

	public override bool saveSubworld => true;
	public override bool disablePlayerSaving => true;
	public override bool saveModData => true;

	public override List<GenPass> tasks => new List<GenPass>()
	{
		new SubworldGenPass(crimstone =>
		{
			crimstone.Message = "Generating Diseased Land";
			Main.rockLayer = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					crimstone.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
					Main.tile[i, j].active(true);
					Main.tile[i, j].type = (ushort)ModContent.TileType<DiseasedSoil>();
				}
			}
		}),

		new SubworldGenPass(walls =>
		{
			walls.Message = "Infesting The World With Walls";
			Main.rockLayer = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					WorldGen.PlaceWall(i, j, WallID.CorruptGrassUnsafe);
				}
			}
		}),

		new SubworldGenPass(carving1 =>
		{
			carving1.Message = "Carving into the Ground";
			Main.rockLayer = Main.maxTilesY;
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.00013); ++index)
			{
				float num = (float) index / ((float) (Main.maxTilesX * Main.maxTilesY) * 0.00013f);
				carving1.Set(num);
				int type = -1;
				WorldGen.TileRunner(Main.maxTilesX / 2, Main.maxTilesY / 2, 80, WorldGen.genRand.Next(800, 1000), type, false, 0.0f, 0.0f, false, true);
			}
		}),

		new SubworldGenPass(smallislands =>
		{
			smallislands.Message = "Creating Safety Pads";
			Main.rockLayer = Main.maxTilesY;
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.0003); ++index)
			{
				float num = (float) index / ((float) (Main.maxTilesX * Main.maxTilesY) * 0.00013f);
				smallislands.Set(num);
				int type =  ModContent.TileType<DiseasedSoil>();
				WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, Main.maxTilesY), (double) WorldGen.genRand.Next(8, 25), WorldGen.genRand.Next(4, 18), type, true, 0.0f, 0.0f, false, true);
			}
		}),

		/*new SubworldGenPass(spawn =>
		{
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.0003); ++index)
			{
				int i = Main.maxTilesY / 2;
				int j = Main.maxTilesX / 2;

				StructureHelper.StructureHelper.GenerateStructure("Structures/Burial", new Point16(i, j), Retribution.Retribution.instance); 
				
				WorldGen.PlaceTile(i - 1, j + 3, TileID.Dirt);
				WorldGen.PlaceTile(i, j + 3, TileID.Dirt);
				WorldGen.PlaceTile(i + 1, j + 3, TileID.Dirt);
			}
		}),*/

		new SubworldGenPass(ore1 =>
		{
			ore1.Message = "Infesting with Cyaton";
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.0002); k++)
			{
				int i = WorldGen.genRand.Next(0, Main.maxTilesX);
				int j = WorldGen.genRand.Next(0, Main.maxTilesY);

				WorldGen.TileRunner(i, j, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(10, 30), ModContent.TileType<Cyaton>());
			}
		})
	};

	public override void Load()
	{
		Main.dayTime = false;
		Main.time = 27000;
	}
}