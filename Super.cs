using Retribution.Dusts;
using Retribution.Items;
using Retribution.NPCs;
using Retribution.Projectiles.Minions;
using Retribution.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;

namespace Retribution
{
    public abstract class Super : ModItem
    {
		public override bool CloneNewInstances => true;
		public int points;
		public bool healing = false;

		public virtual void SafeSetDefaults()
		{
		}
		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();

				if (healing == true)
				{
					tt.text = damageValue + " ability healing";
				}
				else
				{
					tt.text = damageValue + " ability " + damageWord;
				}
			}
		}

        public override bool CanUseItem(Player player)
		{
			var rP = player.GetModPlayer<RetributionPlayer>();

			if (rP.superCurrent >= points)
			{
				rP.superCurrent -= points;
				return true;
			}
			return false;
		}
	}
}