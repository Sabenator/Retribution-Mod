using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Items.Souls;

namespace Retribution.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TanzaniteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tanzanite Mask");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;
            item.width = 28;
            item.height = 26;
            item.defense = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahoganyHelmet);
            recipe.AddIngredient(ItemID.JungleSpores, 3);
            recipe.AddIngredient(ModContent.ItemType<junglesoul>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("livingmahagonybreastplate") && legs.type == mod.ItemType("livingmahagonyleggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = ("Increases minion damage when in the Jungle");

            if (Main.LocalPlayer.ZoneJungle)
            {
                player.minionDamage *= 1.15f;
            }
        }
    }
}