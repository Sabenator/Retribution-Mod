using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.NPCs.Bosses.Silva;
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
	public class Silva_Bag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silva Treasure Bag");
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
			int choice = Main.rand.Next(0, 2);
			if (choice == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<WillowsWisp>());
			}
			if (choice == 1)
			{
				player.QuickSpawnItem(ModContent.ItemType<OakTwine>());
			}
			if (choice == 2)
			{
				player.QuickSpawnItem(ModContent.ItemType<Damaged_Lantern>());
			}
			if (Main.rand.NextFloat() < .35f)
			{
				player.QuickSpawnItem(ModContent.ItemType<forestsoul>(), Main.rand.Next(8, 12));
			}
			if (choice != 0 || choice != 1 || choice != 2 || choice != 3)
			{
				if (Main.rand.NextFloat() < .50f)
				{
					player.QuickSpawnItem(ModContent.ItemType<Crucifix>());
				}
				else
				{
					player.QuickSpawnItem(ModContent.ItemType<Crucifix>());
				}
			}
			if (RetributionWorld.nightmareMode == true)
			{
				if (Main.rand.NextFloat() < .50f)
				{
					player.QuickSpawnItem(ModContent.ItemType<Crucifix>());
				}
				else
				{
					player.QuickSpawnItem(ModContent.ItemType<Crucifix>());
				}
			}
		}

		public override int BossBagNPC => ModContent.NPCType<Silva>();
	}
}