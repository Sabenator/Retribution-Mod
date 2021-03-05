using Retribution.Projectiles;
using Retribution.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Retribution.Items.Weapons.Ranger
{
	public class Exterpater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Extripatir");
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.ranged = true;
			item.width = 50;
			item.height = 28;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 80000;
			item.rare = 0;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 1f;
			item.useAmmo = AmmoID.Bullet;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Main.PlaySound(SoundID.Item42, player.Center);
			Projectile.NewProjectile(player.Center.X, player.Center.Y, speedX, speedY, ModContent.ProjectileType<ExterpaterProj>(), damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
        }
    }

	public class ExterpaterProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 5;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 200;
			projectile.extraUpdates = 300;
		}

		public override void AI()
		{
			Dust dust;
			dust = Terraria.Dust.NewDustPerfect(projectile.Center, 229, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
			dust.noGravity = true;

			if (Main.rand.NextFloat() < .20f)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -10, ModContent.ProjectileType<ExterpaterProj2>(), 10, 0f, Main.myPlayer);
			}

			if (Main.rand.NextFloat() < .20f)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 10, ModContent.ProjectileType<ExterpaterProj2>(), 10, 0f, Main.myPlayer);
			}
		}
	}

	public class ExterpaterProj2 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 5;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 200;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			Dust dust;
			dust = Terraria.Dust.NewDustPerfect(projectile.Center, 229, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
			dust.noGravity = true;
		}
	}
}