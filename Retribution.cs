using Retribution;
using Retribution.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Retribution.UI;
using Terraria.ModLoader.IO;

namespace Retribution
{
	public class Retribution : Mod
	{

		private UserInterface _soulbarUserInterface;

		internal soulbar soulbar;

		public static Retribution instance { get; set; }

		public Retribution() { instance = this; }

		public override void Load()
		{
			instance = this;

			if (Main.netMode != 2)
			{
				soulbar = new soulbar();
				_soulbarUserInterface = new UserInterface();
				_soulbarUserInterface.SetState(soulbar);

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/morbus"), ItemType("CursedProtector"), TileType("CursedProtectorBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/tesca"), ItemType("TescaBox"), TileType("TescaMusicBox"));
			}

			if (Main.netMode != 2)
			{
				Ref<Effect> @ref = new Ref<Effect>(base.GetEffect("Effects/ShockwaveEffect"));
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(@ref, "Shockwave"), (EffectPriority)4);
				Filters.Scene["Shockwave"].Load();
			}

			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadBackgrounds = true,
				AutoloadSounds = true
			};

			RightClickOverrides = new List<Func<bool>>();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			_soulbarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"Retribution: Souls",
					delegate {
						_soulbarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		public override void Close()
		{
			var slots = new int[] {
				GetSoundSlot(SoundType.Music, "Sounds/Music/moss"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/morbus"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/tesca"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/locust"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/silva"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/inferis"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/crystalcave"),
				GetSoundSlot(SoundType.Music, "Sounds/Music/kane")
			};
			/*foreach (var slot in slots)
			{
				if (Main.music.IndexInRange(slot) && Main.music[slot]?.IsPlaying == true)
				{
					Main.music[slot].Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.Immediate);
				}
			}
			base.Close();*/
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			if (Main.LocalPlayer.GetModPlayer<RetributionPlayer>().ZoneCrystal)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/crystalcave");
				priority = MusicPriority.BiomeHigh;
			}

			if (Main.LocalPlayer.GetModPlayer<RetributionPlayer>().ZoneCystic)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/moss");
			}
		}

		public override void PostSetupContent()
		{
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				bossChecklist.Call(
					"AddBoss",
					7f,
					ModContent.NPCType<NPCs.Bosses.Morbus.Morbus>(),
					this,
					"Morbus",
					(Func<bool>)(() => RetributionWorld.downedMorbus),
					ModContent.ItemType<Items.Summons.rottenfang>(),
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					"Can be summoned at night time in the Corruption",
					"",
					"Retribution/NPCs/Bosses/Morbus/Morbus_Thumb");

				bossChecklist.Call(
					"AddBoss",
					4.5f,
					ModContent.NPCType<NPCs.Bosses.Tesca.Tesca>(),
					this,
					"Tesca",
					(Func<bool>)(() => RetributionWorld.downedTesca),
					ModContent.ItemType<Items.Summons.rottenfang>(),
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Mage.WorldsTundra>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Mage.GlacialSpire>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Summoner.IceBreaker>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Ranger.SnowingFrost>() },
					//new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Melee.GlacialCutter>() },
					//new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Mage.Avalanche>() },

					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					"Can be summoned at any time in the Tundra",
					"",
					"Retribution/NPCs/Bosses/Morbus/Tesca_Thumb");

				bossChecklist.Call(
					"AddBoss",
					0f,
					ModContent.NPCType<NPCs.Bosses.Silva.Silva>(),
					this,
					"Silva",
					(Func<bool>)(() => RetributionWorld.downedSilva),
					ModContent.ItemType<Items.Summons.rottenfang>(),
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Mage.WillowsWisp>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Ranger.OakTwine>() },
					//new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Summoner.IceBreaker>() },
					//new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Melee.GlacialCutter>() },
					//new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.Weapons.Mage.Avalanche>() },
					"Can be summoned at any time in the Forest",
					"",
					"Retribution/NPCs/Bosses/Morbus/Silva_Thumb");

				bossChecklist.Call(
					"AddMiniBoss",
					1.1f,
					ModContent.NPCType<NPCs.Minibosses.sanguine>(),
					this,
					"Sanguine",
					(Func<bool>)(() => RetributionWorld.downedSanguine),
					null,
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					"Has a 5% chance to spawn in the Crimson when it's raining",
					"",
					"",
					(Func<bool>)(() => WorldGen.crimson == true));

				bossChecklist.Call(
					"AddMiniBoss",
					1.2f,
					ModContent.NPCType<NPCs.Minibosses.vilacious>(),
					this,
					"Vilacious",
					(Func<bool>)(() => RetributionWorld.downedVilacious),
					null,
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					new List<int> { ModContent.ItemType<Items.scythemold>(), ModContent.ItemType<Items.scythemold>() },
					"Has a 5% chance to spawn in the Corruption when it's raining",
					"",
					"",
					(Func<bool>)(() => WorldGen.crimson == false));
			}
		}
		public const string SuperSlotBackTex = "SuperSlotBackground";
		public static bool AllowAccessorySlots = false;
		public static bool SlotsNextToAccessories = true;

		private static List<Func<bool>> RightClickOverrides;

		public override void Unload()
		{
			if (RightClickOverrides != null)
			{
				RightClickOverrides.Clear();
				RightClickOverrides = null;
			}
		}

		public override object Call(params object[] args)
		{
			try
			{
				string keyword = args[0] as string;

				if (string.IsNullOrEmpty(keyword))
				{
					return null;
				}

				switch (keyword)
				{
					case "add":
					case "remove":
						Func<bool> func = args[1] as Func<bool>;

						if (func == null) return null;

						if (keyword == "add")
						{
							RightClickOverrides.Add(func);
						}
						else if (keyword == "remove")
						{
							RightClickOverrides.Remove(func);
						}
						break;
					case "getEquip":
					case "getVisible":

						int whoAmI = Convert.ToInt32(args[1]);
						SuperSlotPlayer wsp = Main.player[whoAmI].GetModPlayer<SuperSlotPlayer>();

						if (keyword == "getEquip")
						{
							return wsp.EquipSlot.Item;
						}
						else if (keyword == "getVisible")
						{
							return wsp.EquipSlot.Item;
						}
						break;
				}
			}
			catch
			{
				return null;
			}

			return null;
		}

		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			SuperSlotPlayer wsp = Main.LocalPlayer.GetModPlayer<SuperSlotPlayer>();
			wsp.Draw(spriteBatch);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			PacketMessageType message = (PacketMessageType)reader.ReadByte();
			byte player = reader.ReadByte();
			SuperSlotPlayer modPlayer = Main.player[player].GetModPlayer<SuperSlotPlayer>();

			switch (message)
			{
				case PacketMessageType.All:
					modPlayer.EquipSlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)PacketMessageType.All);
						packet.Write(player);
						ItemIO.Send(modPlayer.EquipSlot.Item, packet);
						packet.Send(-1, whoAmI);
					}
					break;
				case PacketMessageType.EquipSlot:
					modPlayer.EquipSlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						modPlayer.SendSingleItemPacket(PacketMessageType.EquipSlot, modPlayer.EquipSlot.Item, -1, whoAmI);
					}
					break;
				default:
					Logger.InfoFormat("[Retribution] Unknown message type: {0}", message);
					break;
			}
		}

		public static bool OverrideRightClick()
		{
			foreach (var func in RightClickOverrides)
			{
				if (func())
				{
					return true;
				}
			}

			return false;
		}
	}
}