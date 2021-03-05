using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Retribution
{
	public class BaseUtility
	{	
		public static float MultiLerp(float percent, params float[] floats)
		{
			float per = 1f / ((float)floats.Length - 1f);
			float total = per;
			int currentID = 0;
			while (percent / total > 1f && currentID < floats.Length - 2)
			{
				total += per;
				currentID++;
			}
			return MathHelper.Lerp(floats[currentID], floats[currentID + 1], (percent - per * (float)currentID) / per);
		}

		public static Vector2 MultiLerpVector(float percent, params Vector2[] vectors)
		{
			float per = 1f / ((float)vectors.Length - 1f);
			float total = per;
			int currentID = 0;
			while (percent / total > 1f && currentID < vectors.Length - 2)
			{
				total += per;
				currentID++;
			}
			return Vector2.Lerp(vectors[currentID], vectors[currentID + 1], (percent - per * (float)currentID) / per);
		}

		public static Color MultiLerpColor(float percent, params Color[] colors)
		{
			float per = 1f / ((float)colors.Length - 1f);
			float total = per;
			int currentID = 0;
			while (percent / total > 1f && currentID < colors.Length - 2)
			{
				total += per;
				currentID++;
			}
			return Color.Lerp(colors[currentID], colors[currentID + 1], (percent - per * (float)currentID) / per);
		}

		public static float RotationTo(Vector2 startPos, Vector2 endPos)
		{
			return (float)Math.Atan2((double)(endPos.Y - startPos.Y), (double)(endPos.X - startPos.X));
		}

		public static Vector2 RotateVector(Vector2 origin, Vector2 vecToRot, float rot)
		{
			float x = (float)(Math.Cos((double)rot) * (double)(vecToRot.X - origin.X) - Math.Sin((double)rot) * (double)(vecToRot.Y - origin.Y) + (double)origin.X);
			float newPosY = (float)(Math.Sin((double)rot) * (double)(vecToRot.X - origin.X) + Math.Cos((double)rot) * (double)(vecToRot.Y - origin.Y) + (double)origin.Y);
			return new Vector2(x, newPosY);
		}
		public static string[] GetLoadedMods()
		{
			return (from m in ModLoader.Mods.Reverse<Mod>()
					select m.Name).ToArray<string>();
		}

		public static void LogBasic(string logText)
		{
			LogManager.GetLogger("Terraria").Info(logText);
		}

		public static void LogFancy(string prefix, Exception e)
		{
			BaseUtility.LogFancy(prefix, null, e);
		}

		public static void LogFancy(string prefix, string logText, Exception e = null)
		{
			ILog logger = LogManager.GetLogger("Terraria");
			if (e != null)
			{
				logger.Info(">---------<");
				logger.Error(prefix + e.Message);
				logger.Error(e.StackTrace);
				logger.Info(">---------<");
				return;
			}
			logger.Info(">---------<");
			logger.Info(prefix + logText);
			logger.Info(">---------<");
		}

		public static void OpenChestUI(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			if (player.sign >= 0)
			{
				Main.PlaySound(11, -1, -1, 1, 1f, 0f);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}
			if (Main.editChest)
			{
				Main.PlaySound(12, -1, -1, 1, 1f, 0f);
				Main.editChest = false;
				Main.npcChatText = "";
			}
			if (player.editedChestName)
			{
				NetMessage.SendData(33, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
				player.editedChestName = false;
			}
			if (Main.netMode != 1)
			{
				int chest = Chest.FindChest(left, top);
				if (chest >= 0)
				{
					Main.stackSplit = 600;
					if (chest == player.chest)
					{
						player.chest = -1;
						Main.PlaySound(11, -1, -1, 1, 1f, 0f);
					}
					else
					{
						player.chest = chest;
						Main.playerInventory = true;
						Main.recBigList = false;
						player.chestX = left;
						player.chestY = top;
						Main.PlaySound((player.chest < 0) ? 10 : 12, -1, -1, 1, 1f, 0f);
					}
					Recipe.FindRecipes();
				}
				return;
			}
			if (left == player.chestX && top == player.chestY && player.chest >= 0)
			{
				player.chest = -1;
				Recipe.FindRecipes();
				Main.PlaySound(11, -1, -1, 1, 1f, 0f);
				return;
			}
			NetMessage.SendData(31, -1, -1, NetworkText.FromLiteral(""), left, (float)top, 0f, 0f, 0, 0, 0);
			Main.stackSplit = 600;
		}

		public static void DisplayTime(double time = -1.0, Color? overrideColor = null, bool sync = false)
		{
			string text = "AM";
			if (time <= -1.0)
			{
				time = Main.time;
			}
			if (!Main.dayTime)
			{
				time += 54000.0;
			}
			time = time / 86400.0 * 24.0;
			time = time - 7.5 - 12.0;
			if (time < 0.0)
			{
				time += 24.0;
			}
			if (time >= 12.0)
			{
				text = "PM";
			}
			int intTime = (int)time;
			double num = (double)((int)((time - (double)intTime) * 60.0));
			string text2 = string.Concat(num);
			if (num < 10.0)
			{
				text2 = "0" + text2;
			}
			if (intTime > 12)
			{
				intTime -= 12;
			}
			if (intTime == 0)
			{
				intTime = 12;
			}
			BaseUtility.Chat(string.Concat(new object[]
			{
				"Time: ",
				intTime,
				":",
				text2,
				" ",
				text
			}), (overrideColor != null) ? overrideColor.Value : new Color(255, 240, 20), sync);
		}

		public static int CalcValue(int plat, int gold, int silver, int copper, bool sellPrice = false)
		{
			int val = copper + silver * 100;
			val += gold * 10000;
			val += plat * 1000000;
			if (sellPrice)
			{
				val *= 5;
			}
			return val;
		}

		public static void AddTooltips(Item item, string[] tooltips)
		{
			BaseUtility.AddTooltips(item.modItem, tooltips);
		}

		public static void AddTooltips(ModItem item, string[] tooltips)
		{
			string supertip = "";
			for (int i = 0; i < tooltips.Length; i++)
			{
				supertip = supertip + tooltips[i] + ((i == tooltips.Length - 1) ? "" : "\n");
			}
			item.Tooltip.SetDefault(supertip);
		}

		public static NPC NPCByName(string n)
		{
			if (n.Contains(":"))
			{
				string mName = n.Split(new char[]
				{
					':'
				})[0];
				string n2 = n.Split(new char[]
				{
					':'
				})[1];
				return ModLoader.GetMod(mName).GetNPC(n2).npc;
			}
			string[] loadedMods = BaseUtility.GetLoadedMods();
			for (int j = 0; j < loadedMods.Length; j++)
			{
				ModNPC i = ModLoader.GetMod(loadedMods[j]).GetNPC(n);
				if (i != null)
				{
					return i.npc;
				}
			}
			return null;
		}

		public static Item ItemByName(string n)
		{
			if (n.Contains(":"))
			{
				string mName = n.Split(new char[]
				{
					':'
				})[0];
				string n2 = n.Split(new char[]
				{
					':'
				})[1];
				return ModLoader.GetMod(mName).GetItem(n2).item;
			}
			string[] loadedMods = BaseUtility.GetLoadedMods();
			for (int j = 0; j < loadedMods.Length; j++)
			{
				ModItem i = ModLoader.GetMod(loadedMods[j]).GetItem(n);
				if (i != null)
				{
					return i.item;
				}
			}
			return null;
		}

		public static Projectile ProjByName(string n)
		{
			if (n.Contains(":"))
			{
				string mName = n.Split(new char[]
				{
					':'
				})[0];
				string n2 = n.Split(new char[]
				{
					':'
				})[1];
				return ModLoader.GetMod(mName).GetProjectile(n2).projectile;
			}
			string[] loadedMods = BaseUtility.GetLoadedMods();
			for (int j = 0; j < loadedMods.Length; j++)
			{
				ModProjectile i = ModLoader.GetMod(loadedMods[j]).GetProjectile(n);
				if (i != null)
				{
					return i.projectile;
				}
			}
			return null;
		}

		public static ModTile TileByName(string n)
		{
			if (n.Contains(":"))
			{
				string mName = n.Split(new char[]
				{
					':'
				})[0];
				string n2 = n.Split(new char[]
				{
					':'
				})[1];
				return ModLoader.GetMod(mName).GetTile(n2);
			}
			string[] loadedMods = BaseUtility.GetLoadedMods();
			for (int j = 0; j < loadedMods.Length; j++)
			{
				ModTile i = ModLoader.GetMod(loadedMods[j]).GetTile(n);
				if (i != null)
				{
					return i;
				}
			}
			return null;
		}

		public static bool CanHit(Rectangle rect, Rectangle rect2)
		{
			return Collision.CanHit(new Vector2((float)rect.X, (float)rect.Y), rect.Width, rect.Height, new Vector2((float)rect2.X, (float)rect2.Y), rect2.Width, rect2.Height);
		}

		public static Vector2 TileToPos(Vector2 tile)
		{
			return tile * new Vector2(16f, 16f);
		}

		public static Vector2 PosToTile(Vector2 pos)
		{
			return pos / new Vector2(16f, 16f);
		}

		public static int TicksToSeconds(int ticks)
		{
			return ticks / 60;
		}

		public static int SecondsToTicks(int seconds)
		{
			return seconds * 60;
		}

		public static int TicksToMinutes(int ticks)
		{
			return BaseUtility.TicksToSeconds(ticks) / 60;
		}

		public static int MinutesToTicks(int minutes)
		{
			return BaseUtility.SecondsToTicks(minutes) * 60;
		}

		public static Color[] AddToArray(Color[] array, Color valueToAdd, int indexAt = -1)
		{
			Array.Resize<Color>(ref array, (indexAt + 1 > array.Length + 1) ? (indexAt + 1) : (array.Length + 1));
			if (indexAt == -1)
			{
				array[array.Length - 1] = valueToAdd;
			}
			else
			{
				List<Color> list = array.ToList<Color>();
				list.Insert(indexAt, valueToAdd);
				array = list.ToArray();
			}
			return array;
		}

		public static string[] AddToArray(string[] array, string valueToAdd, int indexAt = -1)
		{
			Array.Resize<string>(ref array, (indexAt + 1 > array.Length + 1) ? (indexAt + 1) : (array.Length + 1));
			if (indexAt == -1)
			{
				array[array.Length - 1] = valueToAdd;
			}
			else
			{
				List<string> list = array.ToList<string>();
				list.Insert(indexAt, valueToAdd);
				array = list.ToArray();
			}
			return array;
		}

		public static int[] AddToArray(int[] array, int valueToAdd, int indexAt = -1)
		{
			Array.Resize<int>(ref array, (indexAt + 1 > array.Length + 1) ? (indexAt + 1) : (array.Length + 1));
			if (indexAt == -1)
			{
				array[array.Length - 1] = valueToAdd;
			}
			else
			{
				List<int> list = array.ToList<int>();
				list.Insert(indexAt, valueToAdd);
				array = list.ToArray();
			}
			return array;
		}

		public static int[] CombineArrays(int[] array1, int[] array2)
		{
			int[] newArray = new int[array1.Length + array2.Length];
			for (int i = 0; i < array1.Length; i++)
			{
				newArray[i] = array1[i];
			}
			for (int j = 0; j < array2.Length; j++)
			{
				newArray[array1.Length + j] = array2[j];
			}
			return newArray;
		}

		public static int[] FillArray(int[] array, int value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
			return array;
		}

		public static bool InArray(int[] array, int value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value == array[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool InArray(int[] array, int value, ref int index)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value == array[i])
				{
					index = i;
					return true;
				}
			}
			return false;
		}

		public static bool InArray(float[] array, float value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value == array[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool InArray(float[] array, float value, ref int index)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value == array[i])
				{
					index = i;
					return true;
				}
			}
			return false;
		}

		public static Color ColorAlpha(Color color, int alpha)
		{
			return color * (1f - (float)alpha / 255f);
		}

		public static Color ColorBrightness(Color color, int factor)
		{
			int num = Math.Max(0, Math.Min(255, (int)color.R + factor));
			int g = Math.Max(0, Math.Min(255, (int)color.G + factor));
			int b = Math.Max(0, Math.Min(255, (int)color.B + factor));
			return new Color(num, g, b, (int)color.A);
		}

		public static Color ColorMult(Color color, float mult)
		{
			int num = Math.Max(0, Math.Min(255, (int)((float)color.R * mult)));
			int g = Math.Max(0, Math.Min(255, (int)((float)color.G * mult)));
			int b = Math.Max(0, Math.Min(255, (int)((float)color.B * mult)));
			return new Color(num, g, b, (int)color.A);
		}

		public static Color ColorClamp(Color color1, Color color2)
		{
			int r = (int)color1.R;
			int g = (int)color1.G;
			int b = (int)color1.B;
			int a = (int)color1.A;
			if (r < (int)color2.R)
			{
				r = (int)color2.R;
			}
			if (g < (int)color2.G)
			{
				g = (int)color2.G;
			}
			if (b < (int)color2.B)
			{
				b = (int)color2.B;
			}
			if (a < (int)color2.A)
			{
				a = (int)color2.A;
			}
			return new Color(r, g, b, a);
		}

		public static Color ColorBrightnessClamp(Color color1, Color color2)
		{
			float num = (float)color1.R / 255f;
			float g = (float)color1.G / 255f;
			float b = (float)color1.B / 255f;
			float r2 = (float)color2.R / 255f;
			float g2 = (float)color2.G / 255f;
			float b2 = (float)color2.B / 255f;
			float brightness = (r2 > g2) ? r2 : ((g2 > b2) ? g2 : b2);
			float num2 = num * brightness;
			g *= brightness;
			b *= brightness;
			return new Color(num2, g, b, (float)color1.A / 255f);
		}

		public static Color BuffColorize(Color buffColor, Color lightColor)
		{
			Color color2 = BaseUtility.ColorBrightnessClamp(buffColor, lightColor);
			return BaseUtility.ColorClamp(BaseUtility.Colorize(buffColor, lightColor), color2);
		}

		public static Color Colorize(Color tint, Color lightColor)
		{
			float r = (float)lightColor.R / 255f;
			float g = (float)lightColor.G / 255f;
			float b = (float)lightColor.B / 255f;
			float a = (float)lightColor.A / 255f;
			Color newColor = tint;
			float nr = (float)((byte)((float)newColor.R * r));
			float ng = (float)((byte)((float)newColor.G * g));
			float nb = (float)((byte)((float)newColor.B * b));
			float na = (float)((byte)((float)newColor.A * a));
			newColor.R = (byte)nr;
			newColor.G = (byte)ng;
			newColor.B = (byte)nb;
			newColor.A = (byte)na;
			return newColor;
		}

		public static Vector2 ClampToWorld(Vector2 position, bool tilePos = false)
		{
			if (tilePos)
			{
				position.X = (float)((int)MathHelper.Clamp(position.X, 0f, (float)Main.maxTilesX));
				position.Y = (float)((int)MathHelper.Clamp(position.Y, 0f, (float)Main.maxTilesY));
			}
			else
			{
				position.X = (float)((int)MathHelper.Clamp(position.X, 0f, (float)(Main.maxTilesX * 16)));
				position.Y = (float)((int)MathHelper.Clamp(position.Y, 0f, (float)(Main.maxTilesY * 16)));
			}
			return position;
		}

		public static float GetTotalDistance(Vector2[] points)
		{
			float totalDistance = 0f;
			for (int i = 1; i < points.Length; i++)
			{
				totalDistance += Vector2.Distance(points[i - 1], points[i]);
			}
			return totalDistance;
		}

		public static Rectangle ScaleRectangle(Rectangle rect, float scale)
		{
			float ratioWidth = ((float)rect.Width * scale - (float)rect.Width) / 2f;
			float ratioHeight = ((float)rect.Height * scale - (float)rect.Height) / 2f;
			int num = rect.X - (int)ratioWidth;
			int y = rect.Y - (int)ratioHeight;
			int width = rect.Width + (int)(ratioWidth * 2f);
			int height = rect.Height + (int)(ratioHeight * 2f);
			return new Rectangle(num, y, width, height);
		}

		public static Vector2 GetRandomPosNear(Vector2 pos, int minDistance, int maxDistance, bool circular = false)
		{
			return BaseUtility.GetRandomPosNear(pos, Main.rand, minDistance, maxDistance, circular);
		}

		public static Vector2 GetRandomPosNear(Vector2 pos, UnifiedRandom rand, int minDistance, int maxDistance, bool circular = false)
		{
			int distance = maxDistance - minDistance;
			if (!circular)
			{
				float num = pos.X + (float)((Main.rand.Next(2) == 0) ? (-(float)(minDistance + rand.Next(distance))) : (minDistance + rand.Next(distance)));
				float newPosY = pos.Y + (float)((Main.rand.Next(2) == 0) ? (-(float)(minDistance + rand.Next(distance))) : (minDistance + rand.Next(distance)));
				return new Vector2(num, newPosY);
			}
			return BaseUtility.RotateVector(pos, pos + new Vector2((float)(minDistance + rand.Next(distance))), MathHelper.Lerp(0f, 6.28318548f, (float)rand.NextDouble()));
		}

		public static void Chat(string s, Color color, bool sync = true)
		{
			BaseUtility.Chat(s, color.R, color.G, color.B, sync);
		}

		public static void Chat(string s, byte colorR = 255, byte colorG = 255, byte colorB = 255, bool sync = true)
		{
			if (Main.netMode == 0)
			{
				Main.NewText(s, colorR, colorG, colorB, false);
				return;
			}
			if (Main.netMode == 1)
			{
				Main.NewText(s, colorR, colorG, colorB, false);
				return;
			}
			if (sync && Main.netMode == 2)
			{
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color((int)colorR, (int)colorG, (int)colorB), -1);
			}
		}
		public static Vector2[] ChainVector2(Vector2 start, Vector2 end, float jump = 0f)
		{
			List<Vector2> points = new List<Vector2>();
			if (jump <= 0f)
			{
				jump = 16f;
			}
			Vector2 dir = end - start;
			dir.Normalize();
			float length = Vector2.Distance(start, end);
			for (float way = 0f; way < length; way += jump)
			{
				points.Add(start + dir * way);
			}
			return points.ToArray();
		}

		private static FieldInfo soundField;
		private static FieldInfo soundInstanceField;
	}
}
