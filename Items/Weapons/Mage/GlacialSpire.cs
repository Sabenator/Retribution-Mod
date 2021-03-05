using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Retribution.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs;

namespace Retribution.Items.Weapons.Mage
{
	public class GlacialSpire : NightmareRarity
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glacial Spire");
			Tooltip.SetDefault("Swirls up a whirlwind to blow away your enemies");
			Item.staff[item.type] = true;
		}

		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
		{
			foreach (TooltipLine line2 in list)
			{
				if (line2.mod == "Terraria" && line2.Name == "ItemName")
				{
					line2.overrideColor = NightmareColor.Magenta;
				}
			}
		}

        public override void SetDefaults()
		{
			item.damage = 24;
			item.magic = true;
			item.mana = 11;
			item.rare = ItemRarityID.White;
			item.width = 28;
			item.height = 30;
			item.useTime = 30;
			item.UseSound = SoundID.Item34;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 15f;
			item.useAnimation = 30;
			item.shoot = ModContent.ProjectileType <GlacialSpireProj>();
			item.value = Item.sellPrice(gold: 1);
		}
	}

	public class GlacialSpireProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.height = 110;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 50;
		}

		public override bool? CanCutTiles()
		{
			return new bool?(true);
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);
		}

		public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
		{
			float num = 50f;
			float num2 = projectile.ai[0];
			float num3 = MathHelper.Clamp(num2 / 30f, 0f, 1f);
			if (num2 > num - 60f)
			{
				num3 = MathHelper.Lerp(1f, 0f, (num2 - (num - 60f)) / 60f);
			}
			Vector2 top = projectile.Top;
			Vector2 bottom = projectile.Bottom;
			Vector2.Lerp(top, bottom, 0.5f);
			Vector2 vector = new Vector2(0f, bottom.Y - top.Y);
			vector.X = vector.Y;
			new Vector2(top.X - vector.X / 2f, top.Y);
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			Rectangle rectangle = Utils.Frame(texture2D, 1, 1, 0, 0);
			Vector2 origin = Utils.Size(rectangle) / 2f;
			float num4 = -0.157079637f * num2 * (float)((projectile.velocity.X > 0f) ? -1 : 1);
			SpriteEffects effects = (projectile.velocity.X > 0f) ? SpriteEffects.FlipVertically : SpriteEffects.None;
			bool flag = projectile.velocity.X > 0f;
			Vector2 unitY = Vector2.UnitY;
			double num5 = (double)(num2 * 0.14f);
			Vector2 vector2 = Utils.RotatedBy(unitY, num5, default(Vector2));
			float num6 = 0f;
			float num7 = 5.01f + num2 / 0f * -0.9f;
			if (num7 < 4.11f)
			{
				num7 = 4.11f;
			}
			Color value = new Color(184, 248, 255, 150);
			Color color = new Color(42, 213, 213, 150);
			float num8 = num2 % 60f;
			if (num8 < 30f)
			{
				color *= Utils.InverseLerp(22f, 30f, num8, true);
			}
			else
			{
				color *= Utils.InverseLerp(38f, 30f, num8, true);
			}
            _ = color != Color.Transparent;
			for (float num9 = (float)((int)bottom.Y); num9 > (float)((int)top.Y); num9 -= num7)
			{
				num6 += num7;
				float num10 = num6 / vector.Y;
				float num11 = num6 * 6.28318548f / -20f;
				if (flag)
				{
					num11 *= -1f;
				}
				float num12 = num10 - 0.35f;
				Vector2 vector3 = vector2;
				double num13 = (double)num11;
				Vector2 vector4 = Utils.RotatedBy(vector3, num13, default(Vector2));
				Vector2 vector5 = new Vector2(0f, num10 + 1f);
				vector5.X = vector5.Y;
				Color color2 = Color.Lerp(Color.Transparent, value, num10 * 2f);
				if (num10 > 0.5f)
				{
					color2 = Color.Lerp(Color.Transparent, value, 2f - num10 * 2f);
				}
				color2.A = (byte)((float)color2.A * 0.5f);
				color2 *= num3;
				vector4 *= vector5 * 100f;
				vector4.Y = 0f;
				vector4.X = 0f;
				vector4 += new Vector2(bottom.X, num9) - Main.screenPosition;
				Main.spriteBatch.Draw(texture2D, vector4, new Rectangle?(rectangle), color2, num4 + num11, origin, (1f + num12) * this.scale, effects, 0f);
			}
			return false;
		}

		public override void AI()
		{
			for (int i = 0; i < 3; i++)
			{
				Dust dust;
				dust = Main.dust[Terraria.Dust.NewDust(projectile.position, projectile.width, projectile.height, 156, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				dust.noGravity = true;
			}

			projectile.rotation += 0.3f;

			float num = 180f;
			if (projectile.localAI[0] >= 16f && projectile.ai[0] < num - 15f)
			{
				projectile.ai[0] = num - 15f;
			}
			projectile.ai[0] += 1f;
			if (projectile.ai[0] >= num)
			{
				projectile.Kill();
			}
			Vector2 top = projectile.Top;
			Vector2 bottom = projectile.Bottom;
			Vector2 value = Vector2.Lerp(top, bottom, 0.5f);
			Vector2 value2 = new Vector2(0f, bottom.Y - top.Y);
			if (projectile.ai[0] < num - 30f)
			{
				for (int i = 0; i < 1; i++)
				{
					float value3 = -1f;
					float value4 = 0.9f;
					float amount = Utils.NextFloat(Main.rand);
					Vector2 value5 = new Vector2(MathHelper.Lerp(0.1f, 1f, Utils.NextFloat(Main.rand)), MathHelper.Lerp(value3, value4, amount));
					value5.X *= MathHelper.Lerp(2.2f, 0.6f, amount);
					value5.X *= -1f;
					Vector2 value6 = new Vector2(6f, 10f);
                    _ = value + value2 * value5 * 0.5f + value6;
				}
			}
		}
		public float scale = 1f;
	}
}