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
    public class LocusCloudRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cumulus Cloud");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.width = 45;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.timeLeft = 400;
            projectile.tileCollide = false;
            projectile.knockBack = 15;
            projectile.penetrate = 1;

        }

        public bool canRain = true;

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
            projectile.ai[0]++;
            if (projectile.ai[0] > 5 && canRain == true)
            {
                Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-20, 20), projectile.Center.Y + 10, 0, Main.rand.Next(10, 15), ModContent.ProjectileType<LocusRain>(), 15, 0f, Main.myPlayer, projectile.whoAmI, 100);
                projectile.ai[0] = 0;
            }

            if (projectile.timeLeft < 120)
            {
                projectile.alpha+=5;
                canRain = false;
            }
        }
    }
}