using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Items.Weapons.Ranger;
using Retribution.Items.Accessories;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Retribution.Projectiles;
using Retribution.NPCs.Bosses.Vanilla;
using Retribution.Tiles;

namespace Retribution
{
	public class RetributionNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public static int turkeyBoss = -1;

		public bool tFrost;

		public static bool hematicEnemy;
		public static bool cysticEnemy;

        public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			var retributionPlayer = Main.LocalPlayer.GetModPlayer<RetributionPlayer>();

			if (npc.CanBeChasedBy() && Main.LocalPlayer.altFunctionUse == 2)
			{
				return;
			}
			else if (Main.rand.NextFloat() < .40f)
			{
				retributionPlayer.addSoul = true;
			}

			if (npc.life <= 0 && npc.CanBeChasedBy())
			{
				retributionPlayer.addSoul = true;
			}
		}
        public override void SetDefaults(NPC npc)
        {
			#region Nightmare Mode Defaults
			if (RetributionWorld.nightmareMode == true)
			{
				if (npc.boss = false)
				{
					npc.damage = (int)(npc.damage * 1.5f);
					npc.lifeMax = (int)(npc.lifeMax * 1.35f);
				}
			}
            #endregion
        } 
		int soulTimer;

        public override void NPCLoot(NPC npc)
		{
			if (RetributionWorld.downedTesca == true && RetributionWorld.spawnedTanzanite == false)
			{
				Main.NewText("Your world has been blessed with Tanzanite", 50, 130, 200);


				for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 5E-05); k++)
				{
					int x = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y = (int)Main.rockLayer;

					Tile tile = Framing.GetTileSafely(x, y);

					if (tile.active() && tile.type == ModContent.TileType<IceCrystalTile>())
					{
						WorldGen.OreRunner(x, y, WorldGen.genRand.Next(1, 20), WorldGen.genRand.Next(20, 60), (ushort)ModContent.TileType<tanzanite>());
					}
				}
				RetributionWorld.spawnedTanzanite = true;
			}

			if ((Main.LocalPlayer.HeldItem.modItem is ReaperClass && !npc.boss == true))
			{
				if (Main.rand.NextFloat() < RetributionPlayer.soulPercent)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -2.5f, ModContent.ProjectileType<badsoul>(), npc.lifeMax / 5 * 2, 0f, Main.myPlayer);
				}
			}


			if (npc.type == NPCID.EyeofCthulhu && !Main.expertMode)
			{
				if (Main.rand.NextFloat() < .20f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("snapshot"), 1);
				}

				if (Main.rand.NextFloat() < .20f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("peeperstaff"), 1);
				}
			}

			if (npc.type == NPCID.EyeofCthulhu && !Main.expertMode)
			{
				if (Main.rand.NextFloat() < .20f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("honeyray"), 1);
				}
			}

			if (npc.type == NPCID.WallCreeper)
			{
				if (Main.rand.NextFloat() < .02f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("arachnoblaster"), 1);
				}
			}

			if (npc.type == NPCID.WallCreeperWall)
			{
				if (Main.rand.NextFloat() < .02f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("arachnoblaster"), 1);
				}
			}

			if (npc.type == NPCID.WallofFlesh && !Main.expertMode)
			{
				if (!Main.expertMode && Main.rand.NextFloat() < .15f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("reaperemblem"), 1);
				}
			}

				if (npc.type == NPCID.KingSlime && !Main.expertMode)
			{
				if (Main.rand.NextFloat() < .20f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("kingskatana"), 1);
				}
			}

			#region Unique Drops
			if (Main.LocalPlayer.ZoneCrimson)
			{
				if (Main.rand.NextFloat() < .002f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("soulcontract"));
				}
			}
			#endregion

			#region Death Fragment
			if (npc.type == NPCID.LunarTowerSolar && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(6, 15));
			}

			if (npc.type == NPCID.LunarTowerSolar && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(5, 10));
			}

			if (npc.type == NPCID.LunarTowerNebula && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(6, 15));
			}

			if (npc.type == NPCID.LunarTowerNebula && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(5, 10));
			}

			if (npc.type == NPCID.LunarTowerVortex && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(6, 15));
			}

			if (npc.type == NPCID.LunarTowerVortex && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(5, 10));
			}

			if (npc.type == NPCID.LunarTowerStardust && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(6, 15));
			}

			if (npc.type == NPCID.LunarTowerStardust && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("deathfragment"), Main.rand.Next(5, 10));
			}
			#endregion

			#region Souls
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDesert)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("desertsoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("frozensoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("evilsoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("evilsoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("junglesoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneRockLayerHeight)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("cavernsoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("hellfiresoul"));
				}
			}
			if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon)
			{
				if (Main.rand.NextFloat() < .05f)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("dungeonsoul"));
				}
			}
			#endregion
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.ArmsDealer)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<autorifle>());
				nextSlot++;
			}
		}

		public override void ResetEffects(NPC npc)
		{
			tFrost = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (tFrost)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 4;
				if (damage < 2)
				{
					damage = 2;
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (tFrost)
			{
				if (Main.rand.Next(4) < 3)
				{
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 185, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.05f;
					}
				}
				Lighting.AddLight(npc.position, 0.1f, 0.1f, 0.7f);
			}
		}
	}
}