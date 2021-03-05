using System;
using Microsoft.Xna.Framework;
using Retribution.Buffs.Summons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Buffs;

namespace Retribution.Projectiles.Minions
{
    public class peeper : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Spazmamini);
            aiType = ProjectileID.Spazmamini;
            projectile.netImportant = true;
            projectile.width = 54;
            projectile.height = 42;
            Main.projFrames[projectile.type] = 3;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peeper");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            #region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<peeperbuff>());
			}
			if (player.HasBuff(ModContent.BuffType<peeperbuff>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

            Player owner = null;
            if (projectile.owner != -1)
            {
                owner = Main.player[projectile.owner];
            }
            else if (projectile.owner == 255)
            {
                owner = Main.LocalPlayer;
            }
            var flag = projectile.type == mod.ProjectileType("peeper");
            var modPlayer = player.GetModPlayer<RetributionPlayer>();
            if (flag)
            {
                if (player.dead)
                {
                    modPlayer.peeperMinion = false;
                }
                if (modPlayer.peeperMinion)
                {
                    projectile.timeLeft = 2;
                }
            }

            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f;

            float minionPositionOffsetX = (10 + projectile.minionPos * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX;

            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
                    else projectile.velocity.X += overlapVelocity;

                    if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
                    else projectile.velocity.Y += overlapVelocity;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = oldVelocity.Y;
            }
            return false;
        }
    }
}