using Retribution.Dusts;
using Retribution.Buffs;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution.NPCs.Bosses.Locus
{
	public class LocusCloud : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cumulus Cloud");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
		{
            projectile.hostile = true;
            projectile.friendly = false;
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 0;
			projectile.friendly = false;
			projectile.timeLeft = Main.rand.Next(30, 180);
			projectile.tileCollide =false;
            projectile.knockBack = 15;
			projectile.penetrate = 1;

		}

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<LocusCloudRain>(), 25, 0f, Main.myPlayer, projectile.whoAmI, 100);
        }

        public override void AI()
        {

            int frameSpeed = 8;
            projectile.frameCounter++;
            if (projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}