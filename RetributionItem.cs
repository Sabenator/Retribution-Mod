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
using Retribution.Items.Consumables;
using Retribution.Items;
using Retribution.Projectiles;
using Retribution.Items.Weapons.Ranger;
using IL.Terraria.Utilities;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace Retribution
{
	public class RetributionItem : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			var rC = ModContent.GetInstance<RetributionConfig>();

            #region Auto-Swing Fixes
            if (rC.autoSwing == true)
			{
				if (item.type == ItemID.Flamelash || item.type == ItemID.MagicMissile || item.type == ItemID.RodofDiscord || item.type == ItemID.RainbowRod || item.type == ItemID.VenusMagnum || item.thrown == true || item.useStyle == ItemUseStyleID.EatingUsing || item.fishingPole > 0 || item.type == ModContent.ItemType<DesecratedAmulet>())
				{
					item.autoReuse = item.autoReuse;
				}
				else {
					item.autoReuse = true;
				}
			}
			else if (rC.autoSwing == false)
			{
				item.autoReuse = false;
			}
            #endregion

            if (item.type == ItemID.Cobweb)
			{
				item.ammo = ItemID.Cobweb;
				item.shoot = ModContent.ProjectileType<webshot>();
			}

			if (item.type == ModContent.ItemType<frostfirebulletitem>())
			{
				item.ammo = ModContent.ItemType<frostfirebulletitem>();
				item.shoot = ModContent.ProjectileType<frostfirebullet>();
			}
		}
	}
}