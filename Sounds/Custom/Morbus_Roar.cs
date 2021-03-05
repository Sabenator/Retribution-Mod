using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace Retribution.Sounds.Custom
{
	public class Morbus_Roar : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			if (soundInstance.State == SoundState.Playing)
			{
				return null;
			}

			soundInstance.Volume = volume * .5f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(-5, 6) * .05f;
			return soundInstance;
		}
	}
}