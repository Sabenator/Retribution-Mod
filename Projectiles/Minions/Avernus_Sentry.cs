using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Projectiles.Minions

{
    public class Avernus_Sentry : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Avernus");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 46;
            projectile.aiStyle = -1;
            projectile.scale = 0.9f;
            projectile.tileCollide = true;
            projectile.sentry = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 7200;
        }

        public override void Kill(int timeLeft)
        {

            Main.PlaySound(SoundID.Dig, projectile.Center, 62); 

            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Dirt, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 120, default(Color));
                Main.dust[dust].velocity *= 2.5f;
            }
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y - 23), projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 120, default(Color));
            Main.dust[dust].velocity *= 2.5f;
            projectile.ai[0]++;

            if (projectile.ai[0] > 180)
            {
                int a = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, -4, -1, ProjectileID.BallofFire, 60, 0, Main.myPlayer);
                int b = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, 4, -1, ProjectileID.BallofFire, 60, 0, Main.myPlayer);
                int c = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, 2, -3, ProjectileID.BallofFire, 60, 0, Main.myPlayer);
                int d = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, -2, -3, ProjectileID.BallofFire, 60, 0, Main.myPlayer);
                int e = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, -0.5f, -4f, ProjectileID.BallofFire, 60, 0, Main.myPlayer);
                int f = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 23, 0.5f, -4f, ProjectileID.BallofFire, 60, 0, Main.myPlayer);

                Main.projectile[a].penetrate = Main.rand.Next(3, 5);
                Main.projectile[b].penetrate = Main.rand.Next(3, 5);
                Main.projectile[c].penetrate = Main.rand.Next(3, 5);
                Main.projectile[d].penetrate = Main.rand.Next(3, 5);
                Main.projectile[e].penetrate = Main.rand.Next(3, 5);
                Main.projectile[f].penetrate = Main.rand.Next(3, 5);



                projectile.ai[0] = 0;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.position += projectile.velocity;
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}