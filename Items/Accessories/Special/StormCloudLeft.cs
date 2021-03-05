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

namespace Retribution.Items.Accessories.Special
{
    public class StormCloudLeft : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Cloud");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.width = 45;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.timeLeft = 121;
            projectile.tileCollide = false;
            projectile.knockBack = 15;
            projectile.penetrate = -1;
        }

        public bool canRain = true;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.Center = player.Center;
            projectile.position.Y = player.position.Y - 60;
            projectile.position.X = player.position.X - 60;

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
                Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-20, 20), projectile.Center.Y + 10, 0, Main.rand.Next(10, 15), ModContent.ProjectileType<StormCloudRain>(), 15, 0f, Main.myPlayer, projectile.whoAmI, 100);
                projectile.ai[0] = 0;
            }

            if (projectile.timeLeft < 120)
            {
                projectile.alpha+=5;
                canRain = false;
            }

            var rP = Main.LocalPlayer.GetModPlayer<RetributionPlayer>();

            if (player.HasBuff(ModContent.BuffType<StormCloudBuff>()))
            {
                projectile.timeLeft = 121;
            }
        }
    }
}