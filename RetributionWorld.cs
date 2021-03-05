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
using Retribution.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Retribution.Items;

namespace Retribution
{
    public class RetributionWorld : ModWorld
    {
        public static int cysticTiles = 0;
		public static int hematicTiles = 0;
        public static int crystalTiles = 0;

        public static bool nightmareMode = false;

        public static float rottime;

        public static bool spawnedTanzanite = false;

        int Timer;

        #region Boss Checks
        public static bool downedVilacious = false;
        public static bool downedSanguine = false;
        public static bool downedKane = false;
        public static bool downedMorbus = false;
        public static bool downedTesca = false;
        public static bool downedSilva = false;
        #endregion

        #region Save/Load
        public override TagCompound Save()
        {
            var nightmare = new List<string>();
            if (nightmareMode)
            {
                nightmare.Add("NightmareMode");
            }

            var ore = new List<string>();
            if (spawnedTanzanite)
            {
                ore.Add("Tanzanite");
            }

            var downed = new List<string>();
            if (downedKane)
            {
                downed.Add("Kane");
            }
            if (downedSanguine)
            {
                downed.Add("Sanguine");
            }
            if (downedVilacious)
            {
                downed.Add("Vilacious");
            }
            if (downedMorbus)
            {
                downed.Add("Morbus");
            }
            if (downedTesca)
            {
                downed.Add("Tesca");
            }
            if (downedSilva)
            {
                downed.Add("Silva");
            }

            return new TagCompound
            {
                ["downed"] = downed,
                ["ore"] = ore,
                ["nightmare"] = nightmare,
            };
        }

        public override void Load(TagCompound tag)
        {
            var nightmare = tag.GetList<string>("nightmare");
            nightmareMode = nightmare.Contains("NightmareMode");

            var ore = tag.GetList<string>("ore");
            spawnedTanzanite = ore.Contains("Tanzanite");

            var downed = tag.GetList<string>("downed");
            downedKane = downed.Contains("Kane");
            downedSanguine = downed.Contains("Sanguine");
            downedVilacious = downed.Contains("Vilacious");
            downedMorbus = downed.Contains("Morbus");
            downedTesca = downed.Contains("Tesca");
            downedSilva = downed.Contains("Silva");
        }
        #endregion

        #region World Gen
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1) {

                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Rubidium", Rubidium));
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Kyanite", Kyanite));
            }

            int StructIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (StructIndex != -1)
            {
                tasks.Insert(StructIndex + 1, new PassLegacy("Ice Crystal Cave", GenerateCrystal));
                tasks.Insert(StructIndex + 1, new PassLegacy("Valley of Kings", GenerateBurial));
            }

            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
			if (genIndex == -1)
			{
				return;
			}
        }

        public override void PostWorldGen()
        {
            int[] itemsToPlaceInChests = { ModContent.ItemType<scratchedmirror>()};
            int itemsToPlaceInChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        {
                            if (Main.rand.NextFloat() < .25f)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(itemsToPlaceInChests[itemsToPlaceInChestsChoice]);
                                    itemsToPlaceInChestsChoice = (itemsToPlaceInChestsChoice + 1) % itemsToPlaceInChests.Length;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region World Gen Methods
        private void GenerateCrystal(GenerationProgress progress)
        {
            progress.Message = "Generating the Ice Crystal Cave";

            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.00001f); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = (int)Main.rockLayer;

                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.active() && tile.type == TileID.IceBlock)
                {
                    int genRandX = WorldGen.genRand.Next(180, 200);
                    int genRandY = WorldGen.genRand.Next(180, 200);

                    WorldGen.TileRunner(x, y, genRandX, genRandY, ModContent.TileType<IceCrystalTile>(), false, 0.0f, 0.0f, false, true);
                }
            }
        }

        private void GenerateBurial(GenerationProgress progress)
        {
            progress.Message = "Creating the Burial";

            int x = Main.maxTilesX / 2 + Main.rand.Next(-50, 50);
            int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)WorldGen.rockLayerHigh + 10);

            StructureHelper.StructureHelper.GenerateStructure("Structures/Burial", new Point16(x, y), Retribution.instance);
        }

        private void Rubidium(GenerationProgress progress)
        {
            progress.Message = "Generating Rubidium";

            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<rubidium>());
            }
        }

        private void Kyanite(GenerationProgress progress)
        {
            progress.Message = "Generating Kyanite";

            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 3E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(4, 8), ModContent.TileType<kyanite>());
            }
        }

        /*private void IcicleTraps(GenerationProgress progress)
        {
            progress.Message = "Making Frozen Stalactites";

            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.1f); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(0, Main.maxTilesY);

                Tile ice = Framing.GetTileSafely(x, y);
                if (ice.active() && ice.type == ModContent.TileType<IceCrystalTile>())
                {
                    bool placeSuccessful = false;
                    Tile tile;
                    int tileToPlace = ModContent.TileType<IcicleTrap>();
                    while (!placeSuccessful)
                    {
                        WorldGen.PlaceTile(x, y, tileToPlace);
                        tile = Main.tile[x, y];
                        placeSuccessful = tile.active() && tile.type == tileToPlace;
                    }
                }
            }
        }*/
        #endregion

        public override void TileCountsAvailable(int[] tileCounts)
        {
            cysticTiles = tileCounts[ModContent.TileType<DiseasedSoil>()];
			hematicTiles = tileCounts[ModContent.TileType<Hemasoil>()];
            crystalTiles = tileCounts[ModContent.TileType<IceCrystalTile>()];
        }

		public override void ResetNearbyTileEffects()
        {
            cysticTiles = 0;
			hematicTiles = 0;
            crystalTiles = 0;
        }
    }
}