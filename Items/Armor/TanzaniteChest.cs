using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Items.Souls;

namespace Retribution.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class TanzaniteChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tanzanite Breastplate");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;

            item.width = 30;
            item.height = 20;
            item.defense = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahoganyBreastplate);
            recipe.AddIngredient(ItemID.JungleSpores, 4);
            recipe.AddIngredient(ModContent.ItemType<junglesoul>(), 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
    }
}