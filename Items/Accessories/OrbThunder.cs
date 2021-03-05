using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Retribution.Items.Accessories.Special;
using Retribution.Buffs;

namespace Retribution.Items.Accessories
{
	public class OrbThunder : NightmareRarity
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orb of Eternal Thunder");
			Tooltip.SetDefault("Storm clouds will protect you from evil.");
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
			item.width = 16;
			item.height = 16;
			item.value = 1000;
			item.rare = ItemRarityID.White;
			item.accessory = true;
			item.maxStack = 1;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.AddBuff(ModContent.BuffType<StormCloudBuff>(), 30, true);

			player.lifeRegen += 3;
		}
	}
}