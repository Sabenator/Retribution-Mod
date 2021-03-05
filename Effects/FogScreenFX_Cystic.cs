using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Retribution.Effects
{
	public class FogScreenFX_Cystic
	{
		public int fogOffsetX;
		public float fadeOpacity;
		public float dayTimeOpacity;
		public bool backgroundFog;

		public FogScreenFX_Cystic(bool bg)
		{
			backgroundFog = bg;
		}

		public void Update(Texture2D texture)
		{
			if (Main.netMode == 2 || Main.dedServ)
			{
				return;
			}
			Player player = Main.LocalPlayer;
			bool inCystic = player.GetModPlayer<RetributionPlayer>().ZoneCystic;
			fogOffsetX++;
			if (fogOffsetX >= texture.Width)
			{
				fogOffsetX = 0;
			}
			if (inCystic)
			{
				fadeOpacity += 0.02f;
				if (fadeOpacity > 0.2f)
				{
					fadeOpacity = 0.2f;
				}
			}
			else
			{
				fadeOpacity -= 0.5f;
				if (fadeOpacity < 0f)
				{
					fadeOpacity = 0f;
				}
			}
			if (backgroundFog)
			{
				dayTimeOpacity = (Main.dayTime ? BaseUtility.MultiLerp((float)Main.time / 52000f, new float[]
				{
					0.5f,
					1f,
					1f,
					1f,
					1f,
					1f,
					0.5f
				}) : 0.5f);
				dayTimeOpacity *= 0.7f;
				return;
			}
			dayTimeOpacity = (Main.dayTime ? BaseUtility.MultiLerp((float)Main.time / 52000f, new float[]
			{
				0.3f,
				1f,
				1f,
				1f,
				1f,
				1f,
				0.3f
			}) : 0.3f);
			dayTimeOpacity *= (Main.dayTime ? 2f : 1f);
		}

		public void Draw(Texture2D texture, bool dir, Color defaultColor, bool setSB = false)
		{
			if (fadeOpacity == 0f)
			{
				return;
			}
			if (setSB)
			{
				Main.spriteBatch.Begin();
			}
			Player localPlayer = Main.LocalPlayer;
			Color DefaultFog = new Color(83, 209, 61);
			Color fogColor = GetAlpha(DefaultFog, 0.4f * fadeOpacity * dayTimeOpacity);
			int num = -texture.Width;
			int minY = -texture.Height;
			int maxX = Main.screenWidth + texture.Width;
			int maxY = Main.screenHeight + texture.Height;
			for (int i = num; i < maxX; i += texture.Width)
			{
				for (int j = minY; j < maxY; j += texture.Height)
				{
					Main.spriteBatch.Draw(texture, new Rectangle(i + (dir ? (-fogOffsetX) : fogOffsetX), j, texture.Width, texture.Height), null, fogColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
				}
			}
			if (setSB)
			{
				Main.spriteBatch.End();
			}
		}

		public Color GetAlpha(Color newColor, float alph)
		{
			int alpha = 255 - (int)(255f * alph);
			float alphaDiff = (float)(255 - alpha) / 255f;
			int r = (int)((float)newColor.R * alphaDiff);
			int newG = (int)((float)newColor.G * alphaDiff);
			int newB = (int)((float)newColor.B * alphaDiff);
			int newA = (int)newColor.A - alpha;
			if (newA < 0)
			{
				newA = 0;
			}
			if (newA > 255)
			{
				newA = 255;
			}
			return new Color(r, newG, newB, newA);
		}
	}
}
