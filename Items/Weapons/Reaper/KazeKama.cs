using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Retribution;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Retribution.Items.Weapons.Reaper
{
	public class KazeKama : ReaperClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("KazeKama");
		}

		public override void SafeSetDefaults()
		{
			item.channel = true;
			item.maxStack = 1;
			item.damage = 11;
			item.width = 32;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 0f;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 0;
			item.melee = true;
			item.UseSound = SoundID.Item1;
			item.useTurn = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			var retributionPlayer = player.GetModPlayer<RetributionPlayer>();

			if (player.altFunctionUse == 2 && retributionPlayer.soulCurrent >= 6)
			{
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.useTime = 20;
				item.useAnimation = 20;
				item.damage = 15;
				item.crit = 10;
				item.noMelee = true;
				item.melee = false;
				item.autoReuse = true;
				item.UseSound = SoundID.Item1;
				item.shoot = ModContent.ProjectileType<KazeKamaProj>();
				item.shootSpeed = 25f;

				soulCost = 6;

			}
			else
			{
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = 20;
				item.useAnimation = 20;
				item.damage = 6;
				item.noMelee = false;
				item.melee = true;
				item.autoReuse = true;
				item.UseSound = SoundID.Item1;
				item.shoot = 0;
				item.shootSpeed = 3f;

				soulCost = 0;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(15);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 15f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ebonwood, 50);
			recipe.AddIngredient(mod.ItemType("scythemold"), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class KazeKamaProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = false;
			projectile.alpha = 255;
			projectile.timeLeft = 200;
			projectile.extraUpdates = 3;
		}

		public float vectorOffset;
		public bool offsetLeft;
		public Vector2 originalVelocity = Vector2.Zero;

		public override void AI()
		{
			int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1f, projectile.Center.Y - 1f), 2, 2, 76, 0f, 0f, 100, Color.White, 1.6f);
			Main.dust[dustID].velocity *= 0f;
			Main.dust[dustID].noGravity = true;
			if (originalVelocity == Vector2.Zero)
			{
				originalVelocity = projectile.velocity;
			}
			if (offsetLeft)
			{
				vectorOffset -= 0.2f;
				if (vectorOffset <= -1f)
				{
					vectorOffset = -1f;
					offsetLeft = false;
				}
			}
			else
			{
				vectorOffset += 0.2f;
				if (vectorOffset >= 1f)
				{
					vectorOffset = 1f;
					offsetLeft = true;
				}
			}
			float velRot = BaseUtility.RotationTo(projectile.Center, projectile.Center + originalVelocity);
			projectile.velocity = BaseUtility.RotateVector(default(Vector2), new Vector2(projectile.velocity.Length(), 0f), velRot + vectorOffset * 0.5f);
			projectile.rotation = BaseUtility.RotationTo(projectile.Center, projectile.Center + projectile.velocity) + 1.57f - 0.7853982f;
			projectile.spriteDirection = 1;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 76, 0f, 0f, 100, default(Color), 1.2f);
				Main.dust[dustIndex].velocity *= 4.4f;
			}
		}
	}
}
