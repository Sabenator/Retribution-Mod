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
	public class tescanminispike : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tescan Minispike");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
		{
			projectile.width = 3;
			projectile.height = 3;
			projectile.aiStyle = 0;
			projectile.friendly = false;
            projectile.hostile = true;
			projectile.tileCollide = false;
            projectile.alpha = 255;
			projectile.penetrate = 1;
            projectile.timeLeft = 500;
        }

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
            Vector2 position = projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}