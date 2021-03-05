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
    public class Morbus_Mask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morbus Mask");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;
            item.width = 34;
            item.height = 28;
            item.defense = 2;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = false;
        }
    }
}