using System;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;

namespace Retribution
{
	public class GauntletWep : ModPlayer
	{

		public float dashSpeed;
		public float dashMaxSpeedThreshold;
		public float dashMaxFriction;
		public float dashMinFriction;
		public int dashEffect;

		public bool SetDash(float dashSpeed = 14.5f, float dashMaxSpeedThreshold = 12f, float dashMaxFriction = 0.992f, float dashMinFriction = 0.96f, bool forceDash = false, int dashEffect = 0)
		{
			if (forceDash)
			{
				player.dashDelay = 0;
			}
			else if (player.itemAnimation > 1)
			{
				return false;
			}
			if (player.dashDelay == 0)
			{
				if (dashMaxSpeedThreshold < 4f)
				{
					dashMaxSpeedThreshold = 4f;
				}
				float num = Math.Abs(player.velocity.X);
				if (num > dashSpeed)
				{
					dashSpeed = num;
					dashMaxSpeedThreshold = Math.Max(player.accRunSpeed, num - Math.Max(0f, dashSpeed - dashMaxSpeedThreshold));
				}
				this.dashSpeed = dashSpeed;
				this.dashMaxSpeedThreshold = dashMaxSpeedThreshold;
				this.dashMaxFriction = dashMaxFriction;
				this.dashMinFriction = dashMinFriction;
				this.dashEffect = dashEffect;
			}
			return player.dashDelay == 0;
		}
	}
}
