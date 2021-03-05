using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs;
using System;

namespace Retribution.Items.Weapons.Mage
{
	public class WorldsTundra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("World's Tundra");
            Tooltip.SetDefault("Shoots an ice ball.");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 24;
			item.width = 50;
			item.height = 50;
			item.magic = true;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 5f;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = 0;
			item.autoReuse = true;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<TundraBall>();
			item.shootSpeed = 10f;
			item.mana = 12;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class TundraBall : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Ice, projectile.oldVelocity.X * 0.1f, projectile.oldVelocity.Y * 0.1f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);

			if (target.life <= 0)
			{
				Projectile.NewProjectile(projectile.Center.X + 4, projectile.Center.Y - 4, 4, -4, ModContent.ProjectileType<TundraBallSpike>(), 10, 0f, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X + 4, projectile.Center.Y + 4, 4, 4, ModContent.ProjectileType<TundraBallSpike>(), 10, 0f, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X - 4, projectile.Center.Y + 4, -4, 4, ModContent.ProjectileType<TundraBallSpike>(), 10, 0f, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X - 4, projectile.Center.Y - 4, -4, -4, ModContent.ProjectileType<TundraBallSpike>(), 10, 0f, Main.myPlayer);
			}
		}

		public override void AI()
		{
			projectile.rotation -= 0.5f;

			Lighting.AddLight(projectile.position, 0f, 0.93f, 0.93f);
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

	public class TundraBallSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 18;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);
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

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Dust dust;
			Vector2 position = projectile.Center;
			dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
			dust.noGravity = true;

			float num132 = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
			float num133 = projectile.localAI[0];
			if (num133 == 0f)
			{
				projectile.localAI[0] = num132;
				num133 = num132;
			}
			float num134 = projectile.position.X;
			float num135 = projectile.position.Y;
			float num136 = 300f;
			bool flag3 = false;
			int num137 = 0;
			if (projectile.ai[1] == 0f)
			{
				for (int num138 = 0; num138 < 200; num138++)
				{
					if (Main.npc[num138].CanBeChasedBy(this, false) && (projectile.ai[1] == 0f || projectile.ai[1] == (float)(num138 + 1)))
					{
						float num139 = Main.npc[num138].position.X + (float)(Main.npc[num138].width / 2);
						float num140 = Main.npc[num138].position.Y + (float)(Main.npc[num138].height / 2);
						float num141 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num139) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num140);
						if (num141 < num136 && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
						{
							num136 = num141;
							num134 = num139;
							num135 = num140;
							flag3 = true;
							num137 = num138;
						}
					}
				}
				if (flag3)
				{
					projectile.ai[1] = (float)(num137 + 1);
				}
				flag3 = false;
			}
			if (projectile.ai[1] > 0f)
			{
				int num142 = (int)(projectile.ai[1] - 1f);
				if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
				{
					float num143 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
					float num144 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
					if (Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num143) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num144) < 1000f)
					{
						flag3 = true;
						num134 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
						num135 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
					}
				}
				else
				{
					projectile.ai[1] = 0f;
				}
			}
			if (!projectile.friendly)
			{
				flag3 = false;
			}
			if (flag3)
			{
				float num145 = num133;
				Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num146 = num134 - vector10.X;
				float num147 = num135 - vector10.Y;
				float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
				num148 = num145 / num148;
				num146 *= num148;
				num147 *= num148;
				int num149 = 8;
				projectile.velocity.X = (projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
				projectile.velocity.Y = (projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
			}
		}
	}
}