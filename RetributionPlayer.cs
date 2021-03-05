using Retribution.Items.StarterBags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Retribution.Items.Consumables;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Retribution.Items.Weapons.Mage;
using Retribution.Items.Accessories;
using Retribution.Buffs;
using Retribution.Items.Weapons.Reaper.Minions;
using Retribution.Items.Weapons.Reaper;
using Retribution.Projectiles;
using Retribution.NPCs.Bosses.Locus;
using Retribution.Items.Accessories.Special;
using Retribution.Items.Weapons.Ranger;
using Retribution.NPCs.Bosses.Silva;

namespace Retribution
{
	public class RetributionPlayer : ModPlayer
	{
		private const int saveVersion = 0;
		public bool minionName = false;
		public bool Pet = false;
		public static bool hasProjectile;
		public bool fightingTree = false;

        #region Minions
        public bool blackholeMinion = false;
		public bool peeperMinion = false;
		public bool creeperMinion = false;
		public bool eWormMinion = false;
		public bool waterwarriorMinion = false;
		#endregion

		public bool ZoneCystic = false;
		public bool ZoneHematic = false;
		public bool ZoneCrystal = false;

		public static bool soulConstant = false;
		public bool addSoul = false;
		public static float soulPercent = .5f;


		public override void UpdateBiomes()
		{
			ZoneCystic = (RetributionWorld.cysticTiles > 0);
			ZoneHematic = (RetributionWorld.hematicTiles > 0);
			ZoneCrystal = (RetributionWorld.crystalTiles > 100);
		}

		public override Texture2D GetMapBackgroundImage()
		{
			if (ZoneCystic)
			{
				return mod.GetTexture("Backgrounds/MossnMapBackground");
			}

			if (ZoneHematic)
			{
				return mod.GetTexture("Backgrounds/MossnMapBackground");
			}

			if (ZoneCrystal)
			{
				return mod.GetTexture("Backgrounds/MossnMapBackground");
			}
			return null;
		}
		public override void ResetEffects()
		{
			minionName = false;
			Pet = false;
			tFrost = false;
			frostHeart = false;
			orbThunder = false;
			HorusEffect = false;
			soulConstant = false;

			#region Minions
			eWormMinion = false;
			blackholeMinion = false;
			peeperMinion = false;
			waterwarriorMinion = false;
			#endregion
			ResetVariables();
		}

		public static RetributionPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<RetributionPlayer>();
		}
		public float reaperDamage = 1f;
		public float reaperKnockback;
		public int reaperCrit;

		public bool tFrost;

		public int soulCurrent;
		public const int DefaultsoulMax = 50;
		public int soulMax;
		public int soulMax2;
		public int soulRecieve = 0;
		public static bool canAddSoul;

		public int superCurrent;
		public const int DefaultsuperMax = 0;
		public int superMax;
		public int superMax2;
		public bool addSuper;

		public static bool canUseFrostHeart = false;
		public static bool frostHeart = false;
		private static int frostHeartUsed = 0;

		public bool orbThunder;
		private static int orbThunderUsed = 0;

		public static bool HorusEffect = false;

		public static readonly Color Healsoul = new Color(187, 91, 201);

		public override void Initialize()
		{
			soulMax = DefaultsoulMax;
			superMax = DefaultsuperMax;
		}

        public override void OnHitAnything(float x, float y, Entity victim)
        {
			if (player.whoAmI == Main.myPlayer && canUseFrostHeart == true && frostHeartUsed == 0)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -8, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 6, -6, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 8, 0, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 6, 6, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 8, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, -6, 6, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, -8, 0, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, -6, -6, ModContent.ProjectileType<TundraBallSpike>(), 15, 0f, Main.myPlayer);

				frostHeartUsed = 1;

				player.AddBuff(ModContent.BuffType<FrozenHeart>(), 180);
			}
        }

        public override void UpdateDead()
		{
			tFrost = false;
			canAddSoul = false;
			ResetVariables();
		}

		#region Reaper Class/Super
		private void ResetVariables()
		{
			reaperDamage = 1f;
			reaperKnockback = 0f;
			reaperCrit = 0;
			soulMax2 = soulMax;
			superMax2 = superMax;

			soulPercent = .5f;
		}

		public override void PostUpdateMiscEffects()
		{
			UpdateResource();
		}

		private static int soulConstantTime;
		public static int soulConstantThresh;

		private void UpdateResource()
		{
			if (addSoul == true && canAddSoul == true)
			{
				soulCurrent += 1 + soulRecieve;
				addSoul = false;
				soulRecieve = 0;
			}

			if (soulConstant == true && soulCurrent <= soulMax)
			{
				soulConstantTime += 1;

				if (soulConstantTime > soulConstantThresh)
				{
					soulCurrent += 1;
					soulConstantTime = 0;
				}
			}

			soulCurrent = Utils.Clamp(soulCurrent, 0, soulMax2);

			if (addSuper == true)
			{
				superCurrent += 1;
				addSuper = false;
			}

			superCurrent = Utils.Clamp(superCurrent, 0, superMax2);
		}

		public bool deathBuff = false;
        #endregion

        public override void PreUpdate()
		{
			if (soulCurrent == soulMax)
			{
				deathBuff = true;
			}

			if (deathBuff == true)
			{
				Main.LocalPlayer.AddBuff(ModContent.BuffType<godofdeath>(), 180, true);
				deathBuff = false;
			}

			if (player.whoAmI == Main.myPlayer && frostHeart == true && !player.HasBuff(ModContent.BuffType<FrozenHeart>()))
			{
				frostHeartUsed = 0;
				canUseFrostHeart = true;
			}

			if (orbThunder == true && orbThunderUsed == 0)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<StormCloud>(), 0, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<StormCloudLeft>(), 0, 0f, Main.myPlayer);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<StormCloudRight>(), 0, 0f, Main.myPlayer);
			}

			if (player.ownedProjectileCounts[ModContent.ProjectileType<StormCloud>()] > 0)
			{
				orbThunderUsed = 1;
			}

			if (orbThunder == true && orbThunderUsed == 1)
			{
				Main.LocalPlayer.AddBuff(ModContent.BuffType<StormCloudBuff>(), 1, true);
			}

			if (!player.HasBuff(ModContent.BuffType<StormCloudBuff>()))
			{
				orbThunder = false;
			}

			if (orbThunder == false || player.ownedProjectileCounts[ModContent.ProjectileType<StormCloud>()] <= 0)
			{
				orbThunderUsed = 0;
			}

			if (player.ownedProjectileCounts[ModContent.ProjectileType<miniblade>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<FireWisp>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<bloodbullet>()] > 0)
			{
				canAddSoul = false;
				addSoul = false;
			}
			else
			{
				canAddSoul = true;
			}
		}

		#region Save/Load
		public override TagCompound Save()
		{
			return new TagCompound {
				{"soulStorage", soulMax},
				{"soulCurrent", soulCurrent},
			};
		}

		public override void Load(TagCompound tag)
		{
			soulMax = tag.GetInt("soulStorage");
			soulCurrent = tag.GetInt("soulCurrent");
		}
		#endregion

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			Item item = new Item();
			item.SetDefaults(ModContent.ItemType<emptybag>());
			item.stack = 1;
			items.Add(item);

			if (Main.expertMode)
			{
				Item item2 = new Item();
				item2.SetDefaults(ModContent.ItemType<DesecratedAmulet>());
				item2.stack = 1;
				items.Add(item2);
			}
		}

		public override void UpdateBadLifeRegen()
		{
			if (tFrost)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 4;
			}
		}

        public override void PostUpdateRunSpeeds()
        {
			if (player.whoAmI == Main.myPlayer && player.HasBuff(ModContent.BuffType<TerrariasFrost>()))
			{
				player.moveSpeed -= .5f;
			}

			if (player.whoAmI == Main.myPlayer && player.HasBuff(BuffID.Slimed))
			{
				player.moveSpeed -= .1f;
			}
		}

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
			if (tFrost)
			{
				if (Main.rand.Next(4) < 3)
				{
					int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 185, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.05f;
					}
				}
				Lighting.AddLight(player.position, 0.1f, 0.1f, 0.7f);
			}
		}
	}
}