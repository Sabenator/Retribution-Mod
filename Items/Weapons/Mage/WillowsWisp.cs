using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs;
using System;

namespace Retribution.Items.Weapons.Mage
{
	public class WillowsWisp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Willow's Wisp");
            Tooltip.SetDefault("Hurls a ball of sap");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 36;
			item.height = 36;
			item.magic = true;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 5f;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = 0;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SapBall>();
			item.shootSpeed = 11f;
			item.mana = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Main.PlaySound(SoundID.Item112, player.Center);
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(player.position + player.velocity, 10, 10, 153, speedX / 2, speedY / 2);
			}
			return true;
		}
    }

	public class SapBall : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
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
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 153, projectile.oldVelocity.X / 4, projectile.oldVelocity.Y / 4);
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Honey, 180);
		}

        public override void AI()
		{
			projectile.rotation -= 0.8f;

			projectile.velocity.Y = projectile.velocity.Y + 0.1f; 
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
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