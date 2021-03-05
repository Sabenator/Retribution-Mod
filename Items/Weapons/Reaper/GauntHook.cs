using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Retribution;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Retribution.Projectiles;
using Retribution.Items.Souls;

namespace Retribution.Items.Weapons.Melee
{
	public class GauntHook : ReaperClass
	{
		public override void SetStaticDefaults() {

			DisplayName.SetDefault("Gaunt Hook");
			ItemID.Sets.Yoyo[item.type] = true;
			ItemID.Sets.GamepadExtraRange[item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 53;
			item.height = 74;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 16f;
			item.knockBack = 2.5f;
			item.damage = 19;
			item.rare = ItemRarityID.White;

			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 1);
			item.shoot = ModContent.ProjectileType<waxwhizproj>();
		}
	}

	public class GauntHookProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 8.5f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 18f;
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;

			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}

        public override void AI()
        {
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
    }
}