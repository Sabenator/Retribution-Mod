using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution;
using Retribution.Tiles;
using System.Collections.Generic;
using Terraria.World.Generation;

public class HematicZone : Subworld
{
	public override int width => 1200;
	public override int height => 600;

	public override ModWorld modWorld => ModContent.GetInstance <RetributionWorld>();

	public override bool saveSubworld => true;
	public override bool disablePlayerSaving => true;
	public override bool saveModData => true;


	public override List<GenPass> tasks => new List<GenPass>()
	{
		new SubworldGenPass(crimstone =>
		{
			crimstone.Message = "Generating Fleshy Mass";
			Main.rockLayer = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					crimstone.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
					Main.tile[i, j].active(true);
					Main.tile[i, j].type = (ushort)ModContent.TileType<Hemasoil>();

					/*int choice = Main.rand.Next(2); //For a random tile choice
					if (choice == 0)
					{
						Main.tile[i, j].type = TileID.Crimstone;
					}
					else if (choice == 1)
					{
						Main.tile[i, j].type = TileID.CrimsonHardenedSand;
					}*/
				}
			}
		}),

		new SubworldGenPass(walls =>
		{
			walls.Message = "Filling the World with Walls";
			Main.rockLayer = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					WorldGen.PlaceWall(i, j, WallID.CrimsonUnsafe2);
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
				WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, Main.maxTilesY), (double) WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(300, 400), type, false, 0.0f, 0.0f, false, true);
			}
		}),

		new SubworldGenPass(carving2 =>
		{
			carving2.Message = "Generating Small Chasms";
			Main.rockLayer = Main.maxTilesY;
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.00016); ++index)
			{
				float num = (float) index / ((float) (Main.maxTilesX * Main.maxTilesY) * 0.00013f);
				carving2.Set(num);
				int type = -1;
				WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, Main.maxTilesY), (double) WorldGen.genRand.Next(10, 70), WorldGen.genRand.Next(10, 100), type, false, 0.0f, 0.0f, false, true);
			}
		}),

		new SubworldGenPass(carving3 =>
		{
			carving3.Message = "Generating Smaller Chasms";
			Main.rockLayer = Main.maxTilesY;
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.0003); ++index)
			{
				float num = (float) index / ((float) (Main.maxTilesX * Main.maxTilesY) * 0.00013f);
				carving3.Set(num);
				int type = -1;
				WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, Main.maxTilesY), (double) WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 20), type, false, 0.0f, 0.0f, false, true);
			}
		}),

		new SubworldGenPass(smallislands =>
		{
			smallislands.Message = "Creating Tumors";
			Main.rockLayer = Main.maxTilesY;
			for (int index = 0; index < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 0.0003); ++index)
			{
				float num = (float) index / ((float) (Main.maxTilesX * Main.maxTilesY) * 0.00013f);
				smallislands.Set(num);
				int type = (ushort)ModContent.TileType<Hemasoil>();
				WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, Main.maxTilesY), (double) WorldGen.genRand.Next(8, 25), WorldGen.genRand.Next(4, 18), type, true, 0.0f, 0.0f, false, true);
			}
		}),

		new SubworldGenPass(spawn =>
		{
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 1); k++)
			{
				int i = Main.maxTilesY / 2;
				int j = Main.maxTilesX / 2;

				WorldGen.PlaceTile(i - 3, j - 1, TileID.Dirt);
				WorldGen.PlaceTile(i - 3, j, TileID.Dirt);
				WorldGen.PlaceTile(i - 3, j + 1, TileID.Dirt);
			}
		}),

		new SubworldGenPass(ore1 =>
		{
			ore1.Message = "Infesting with Hematite";
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.0002); k++)
			{
				int i = WorldGen.genRand.Next(0, Main.maxTilesX);
				int j = WorldGen.genRand.Next(0, Main.maxTilesY);

				WorldGen.TileRunner(i, j, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(10, 30), ModContent.TileType<Hematite>());
			}
		})
	};

	public override void Load()
	{
		Main.dayTime = false;
		Main.time = 27000;
	}
}