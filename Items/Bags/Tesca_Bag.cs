using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.NPCs.Bosses.Tesca;
using Retribution.Items.Weapons.Mage;
using Retribution.Items.Weapons.Melee;
using Retribution.Items.Weapons.Ranger;
using Retribution.Items.Weapons.Summoner;
using Retribution.Items.Weapons.Reaper;
using Retribution.Items.Hooks;
using Retribution.Items.Accessories;
using Retribution.Items.Souls;


namespace Retribution.Items.Bags
{
	public class Tesca_Bag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesca Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.rare = ItemRarityID.Expert;
			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			int choice = Main.rand.Next(0, 3);
			if (choice == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<WorldsTundra>());
			}
			if (choice == 1)
			{
				player.QuickSpawnItem(ModContent.ItemType<SnowingFrost>());
			}
			if (choice == 2)
			{
				player.QuickSpawnItem(ModContent.ItemType<IceBreaker>());
			}
			if (choice == 3)
			{
				player.QuickSpawnItem(ModContent.ItemType<IceHook>());
			}
			if (Main.rand.NextFloat() < .35f)
			{
				player.QuickSpawnItem(ModContent.ItemType<frozensoul>(), Main.rand.Next(8, 12));
			}
			if (RetributionWorld.nightmareMode == true)
			{
				if (Main.rand.NextFloat() < .50f)
				{
					player.QuickSpawnItem(ModContent.ItemType<GlacialSpire>());
				}
				else
				{
					player.QuickSpawnItem(ModContent.ItemType<FrostHeart>());
				}
			}
		}

		public override int BossBagNPC => ModContent.NPCType<Tesca>();
	}
}