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
using Retribution.Items.Weapons.Ranger;

namespace Retribution
{
    public class RetributionProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (RetributionPlayer.HorusEffect == true && projectile.type != ModContent.ProjectileType<HorusBolt>())
            {
				if (Main.rand.NextFloat() < .3f)
				{
					Projectile.NewProjectile(target.Center.X + 10, target.Center.Y - 10, 3, -3, ModContent.ProjectileType<HorusBolt>(), 0, 0f, Main.LocalPlayer.whoAmI, 0f, 0f);
				}

				if (Main.rand.NextFloat() < .3f)
				{
					Projectile.NewProjectile(target.Center.X - 10, target.Center.Y - 10, -3, -3, ModContent.ProjectileType<HorusBolt>(), 0, 0f, Main.LocalPlayer.whoAmI, 0f, 0f);
				}

				if (Main.rand.NextFloat() < .3f)
				{
					Projectile.NewProjectile(target.Center.X - 10, target.Center.Y + 10, -3, 3, ModContent.ProjectileType<HorusBolt>(), 0, 0f, Main.LocalPlayer.whoAmI, 0f, 0f);
				}

				if (Main.rand.NextFloat() < .3f)
				{
					Projectile.NewProjectile(target.Center.X + 10, target.Center.Y + 10, 3, 3, ModContent.ProjectileType<HorusBolt>(), 0, 0f, Main.LocalPlayer.whoAmI, 0f, 0f);
				}
			}
        }
    }
}