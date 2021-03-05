using Retribution.Dusts;
using Retribution.Buffs;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution.NPCs.Bosses.Tesca
{
	public class tescanminiproj : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tescan Laser Spike");
        }

        public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 86;
			projectile.aiStyle = 0;
			projectile.friendly = false;
            projectile.hostile = true;
			projectile.tileCollide = false;
            projectile.alpha = 255;
			projectile.penetrate = 100;
            projectile.extraUpdates = 2;
        }

        private int spikeTimer;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (RetributionWorld.nightmareMode == true)
            {
                target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 60);
            }
            else
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0f, 0.93f, 0.93f);

            Dust dust;
            Vector2 position = projectile.position;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            spikeTimer++;

            if (spikeTimer >= 35)
            {
                Projectile.NewProjectile(projectile.position.X, projectile.Center.Y - 60, 0, -5, ModContent.ProjectileType<tescanminispike>(), 10, 0f, Main.myPlayer, projectile.whoAmI, 40);
                spikeTimer = 0;
            }
        }
    }
}