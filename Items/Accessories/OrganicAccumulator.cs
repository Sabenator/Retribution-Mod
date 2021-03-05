using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Retribution.Items.Accessories.Special;
using Retribution.Buffs;

namespace Retribution.Items.Accessories
{
	public class OrganicAccumulator : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Organic Accumulator");
			Tooltip.SetDefault("Generates a soul every 8 seconds");
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
			RetributionPlayer.soulConstant = true;
			RetributionPlayer.soulConstantThresh = 480;
		}
	}
}