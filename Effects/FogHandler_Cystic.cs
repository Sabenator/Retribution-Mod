using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Retribution.Effects
{
	public class FogHandler_Cystic : ModWorld
	{
		private FogScreenFX_Cystic cysticFog = new FogScreenFX_Cystic(false);

		public override void PostDrawTiles()
		{
			cysticFog.Update(mod.GetTexture("Effects/Fog_Cystic"));
			cysticFog.Draw(mod.GetTexture("Effects/Fog_Cystic"), false, Color.White, true);
		}
	}
}