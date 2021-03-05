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
    public class LocusRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain");
        }

        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.width = 2;
            projectile.height = 40;
            projectile.aiStyle = 0;
            projectile.friendly = false;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            projectile.knockBack = 15;
            projectile.penetrate = 1;

        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 180);
        }

        public override void AI()
        {
            if (projectile.timeLeft < 60)
            {
                projectile.alpha--;
            }
        }
    }
}