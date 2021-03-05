using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Hooks
{
	internal class Omnipotence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omnipotence");
		}

		public override void SetDefaults()
		{
			item.noUseGraphic = true;
			item.damage = 0;
			item.knockBack = 7f;
			item.useStyle = 5;
			item.width = 18;
			item.height = 28;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = ItemRarityID.Blue;
			item.noMelee = true;
			item.value = 20000;
			item.shootSpeed = 50f;
			item.shoot = ModContent.ProjectileType<OmnipotenceProj>();
		}
	}

	internal class OmnipotenceProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omnipotence");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
				{
					hooksOut++;
				}
			}
			if (hooksOut > 9)
			{
				return false;
			}
			return true;
		}

		public override void UseGrapple(Player player, ref int type)
		{
			int hooksOut = 0;
			int oldestHookIndex = -1;
			int oldestHookTimeLeft = 100000;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
				{
					hooksOut++;
					if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
					{
						oldestHookIndex = i;
						oldestHookTimeLeft = Main.projectile[i].timeLeft;
					}
				}
			}
			if (hooksOut > 9)
			{
				Main.projectile[oldestHookIndex].Kill();
			}
		}

		public override float GrappleRange()
		{
			return 5000f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 9;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 45f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 25;
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = projectile.DirectionTo(player.Center);
			float hangDist = 50f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 center = projectile.Center;
			Vector2 distToProj = playerCenter - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();               
				distToProj *= 24f;                    
				center += distToProj;              
				distToProj = playerCenter - center;  
				distance = distToProj.Length();
				Color drawColor = lightColor;

				spriteBatch.Draw(mod.GetTexture("Items/Hooks/OmnipotenceChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height), drawColor, projRotation,
					new Vector2(Main.chain30Texture.Width * 0.5f, Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}