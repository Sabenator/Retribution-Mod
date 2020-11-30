﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using System;

namespace Retribution.Projectiles
{
    public class ropelashproj : ModProjectile
    {

        public const float whipLength = 18f;
        public const bool whipSoftSound = true;
        public const int handleHeight = 24;
        public const int chainHeight = 12;
        public const int partHeight = 12;
        public const int tipHeight = 12;
        public const bool doubleCritWindow = true;
        public const bool ignoreLighting = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rope Lash");
            			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.scale = 1f;
            projectile.aiStyle = 75;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.extraUpdates = 3;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            LashProj.LashAI(projectile, whipLength);
        }

        #region BaseWhip Stuff
        
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            LashProj.ModifyHitAny(projectile, ref damage, ref knockback, ref crit, doubleCritWindow);
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            LashProj.ModifyHitAny(projectile, ref damage, ref crit);
        }
        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            LashProj.ModifyHitAny(projectile, ref damage, ref crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            LashProj.OnHitAny(projectile, target, crit, whipSoftSound);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            LashProj.OnHitAny(projectile, crit, whipSoftSound);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            LashProj.OnHitAny(projectile, crit, whipSoftSound);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return LashProj.Colliding(projectile, targetHitbox);
        }

        public override bool? CanCutTiles()
        {
            return LashProj.CanCutTiles(projectile);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return LashProj.PreDraw(projectile, handleHeight, chainHeight, partHeight, tipHeight, 3, ignoreLighting, doubleCritWindow);
        }

        #endregion

    }
}
