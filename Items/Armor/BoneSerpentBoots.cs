using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Items.Souls;

namespace Retribution.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BoneSerpentBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Serpent Boots");
            Tooltip.SetDefault("Speed increased by 15% when in the Jungle");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;
            item.defense = 1;
            item.width = 22;
            item.height = 18;
        }

        public override void UpdateEquip(Player player)
        {
            if (Main.LocalPlayer.ZoneJungle)
            {
                player.moveSpeed *= 1.15f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahoganyGreaves);
            recipe.AddIngredient(ItemID.JungleSpores, 5);
            recipe.AddIngredient(ModContent.ItemType<junglesoul>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}