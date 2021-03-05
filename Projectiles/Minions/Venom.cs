using Retribution.Dusts;
using Retribution.Buffs;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace Retribution.Projectiles.Minions
{
	public class Venom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;               
			projectile.height = 16;             
			projectile.aiStyle = 0;          
			projectile.friendly = true;       
			projectile.hostile = false;    
			projectile.penetrate = 2;
			projectile.alpha = 255;             
			projectile.timeLeft = 1500;          
			projectile.light = 0.5f;            
			projectile.ignoreWater = true;         
			projectile.tileCollide = true;         
			projectile.extraUpdates = 2;
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.CursedInferno, 180);
        }

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			Vector2 usePos = projectile.position;

			Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
			usePos += rotVector * 16f;

			const int NUM_DUSTS = 20;

            for (int i = 0; i < NUM_DUSTS; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, 0f, 50, new Color(0, 142, 255), 1f)];
                dust.noGravity = true;
            }
		}

		public override void AI()
		{
			projectile.netUpdate = true;
			projectile.rotation++;

			projectile.ai[0] += 1f;
			if (projectile.ai[0] >= 60f)
			{
				projectile.ai[0] = 15f;
				projectile.velocity.Y = projectile.velocity.Y + 0.8f;
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}

			Dust dust;
			Vector2 position = projectile.position;
			dust = Main.dust[Terraria.Dust.NewDust(position, 1, 1, 75, 0f, 0f, 0, new Color(0, 217, 255), 2.776316f)];
			dust.noGravity = true;

			Lighting.AddLight(projectile.position, 0.1f, 0.1f, 0.9f);
		}
	}
}