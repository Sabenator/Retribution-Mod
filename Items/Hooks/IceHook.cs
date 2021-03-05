using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Retribution.Items.Hooks
{
	internal class IceHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Hook");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Cyan;
			item.noUseGraphic = true;
			item.useStyle = 5;
			item.shootSpeed = 11.25f;
			item.shoot = ModContent.ProjectileType<IceHookProj>();
			item.UseSound = SoundID.Item1;
			item.useAnimation = 20;
			item.useTime = 20;
			item.noMelee = true;
		}

		public override bool CanUseItem(Player player)
		{
			return false;
		}
	}

	public class IceHookProj : BaseHook
	{
		public override void SetHookDefaults()
		{
			projectile.width = 30;
			projectile.height = 20;
		}

		public override float GrappleRange()
		{
			return 380f;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 14.75f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 2;
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return new bool?(false);
		}
	}
}