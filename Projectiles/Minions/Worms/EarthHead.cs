using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs.Summons;

namespace Retribution.Projectiles.Minions.Worms
{
	public class EarthHead : ModProjectile
	{
		public override void SetDefaults()
		{
			base.projectile.width = 64;
			base.projectile.height = 74;
			base.projectile.friendly = true;
			base.projectile.ignoreWater = true;
			base.projectile.netImportant = true;
			base.projectile.tileCollide = false;
			base.projectile.minion = true;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 18000;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
			base.projectile.timeLeft *= 5;
		}

		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Earth Worm");
		}

		public override void AI()
		{
			Player player10 = Main.player[base.projectile.owner];
			if ((int)Main.time % 120 == 0)
			{
				base.projectile.netUpdate = true;
			}
			if (!player10.active)
			{
				base.projectile.active = false;
				return;
			}
			Player player1 = Main.player[base.projectile.owner];
			RetributionPlayer rP = (RetributionPlayer)player1.GetModPlayer<RetributionPlayer>();
			player1.AddBuff(ModContent.BuffType<earthwormbuff>(), 3600, true);
			if (player1.dead)
			{
				rP.eWormMinion = false;
			}
			if (rP.eWormMinion)
			{
				base.projectile.timeLeft = 2;
			}
			int num1049 = 30;
			Vector2 center14 = player10.Center;
			float maxTargetDist = 700f;
			float num1050 = 1000f;
			int num1051 = -1;
			if (base.projectile.Distance(center14) > 2000f)
			{
				base.projectile.Center = center14;
				base.projectile.netUpdate = true;
			}
			if (true)
			{
				NPC ownerMinionAttackTargetNPC5 = base.projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC5 != null && ownerMinionAttackTargetNPC5.CanBeChasedBy(base.projectile, false) && base.projectile.Distance(ownerMinionAttackTargetNPC5.Center) < maxTargetDist * 2f)
				{
					num1051 = ownerMinionAttackTargetNPC5.whoAmI;
					if (ownerMinionAttackTargetNPC5.boss)
					{
						int whoAmI = ownerMinionAttackTargetNPC5.whoAmI;
					}
					else
					{
						int whoAmI2 = ownerMinionAttackTargetNPC5.whoAmI;
					}
				}
				if (num1051 < 0)
				{
					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC target = Main.npc[i];
						if (target.CanBeChasedBy(base.projectile, false) && player10.Distance(target.Center) < num1050 && base.projectile.Distance(target.Center) < maxTargetDist)
						{
							num1051 = i;
						}
					}
				}
			}
			if (num1051 != -1)
			{
				NPC nPC15 = Main.npc[num1051];
				Vector2 vector148 = nPC15.Center - base.projectile.Center;
				Utils.ToDirectionInt(vector148.X > 0f);
				Utils.ToDirectionInt(vector148.Y > 0f);
				float scaleFactor15 = 0.4f;
				if (vector148.Length() < 600f)
				{
					scaleFactor15 = 0.6f;
				}
				if (vector148.Length() < 300f)
				{
					scaleFactor15 = 0.8f;
				}
				if (vector148.Length() > nPC15.Size.Length() * 0.75f)
				{
					base.projectile.velocity += Vector2.Normalize(vector148) * scaleFactor15 * 1.5f;
					if (Vector2.Dot(base.projectile.velocity, vector148) < 0.25f)
					{
						base.projectile.velocity *= 0.8f;
					}
				}
				float num1052 = 30f;
				if (base.projectile.velocity.Length() > num1052)
				{
					base.projectile.velocity = Vector2.Normalize(base.projectile.velocity) * num1052;
				}
			}
			else
			{
				float num1053 = 0.2f;
				Vector2 vector149 = center14 - base.projectile.Center;
				if (vector149.Length() < 200f)
				{
					num1053 = 0.12f;
				}
				if (vector149.Length() < 140f)
				{
					num1053 = 0.06f;
				}
				if (vector149.Length() > 100f)
				{
					if (Math.Abs(center14.X - base.projectile.Center.X) > 20f)
					{
						base.projectile.velocity.X = base.projectile.velocity.X + num1053 * (float)Math.Sign(center14.X - base.projectile.Center.X);
					}
					if (Math.Abs(center14.Y - base.projectile.Center.Y) > 10f)
					{
						base.projectile.velocity.Y = base.projectile.velocity.Y + num1053 * (float)Math.Sign(center14.Y - base.projectile.Center.Y);
					}
				}
				else if (base.projectile.velocity.Length() > 2f)
				{
					base.projectile.velocity *= 0.96f;
				}
				if (Math.Abs(base.projectile.velocity.Y) < 1f)
				{
					base.projectile.velocity.Y = base.projectile.velocity.Y - 0.1f;
				}
				float num1054 = 15f;
				if (base.projectile.velocity.Length() > num1054)
				{
					base.projectile.velocity = Vector2.Normalize(base.projectile.velocity) * num1054;
				}
			}
			base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) + 1.57079637f;
			int direction = base.projectile.direction;
			base.projectile.direction = (base.projectile.spriteDirection = ((base.projectile.velocity.X > 0f) ? 1 : -1));
			if (direction != base.projectile.direction)
			{
				base.projectile.netUpdate = true;
			}
			float num1055 = MathHelper.Clamp(base.projectile.localAI[0], 0f, 50f);
			base.projectile.position = base.projectile.Center;
			base.projectile.scale = 1f + num1055 * 0.01f;
			base.projectile.width = (base.projectile.height = (int)((float)num1049 * base.projectile.scale));
			base.projectile.Center = base.projectile.position;
			if (base.projectile.alpha > 0)
			{
				base.projectile.alpha -= 42;
				if (base.projectile.alpha < 0)
				{
					base.projectile.alpha = 0;
					return;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[base.projectile.owner] = 7;
		}
	}
}
