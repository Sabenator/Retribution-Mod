using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Retribution.Items.Weapons.Summoner
{
    public class peeperstaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peeper Staff");
            Tooltip.SetDefault("Summons a peeper to fight for you");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.summon = true;
            item.mana = 8;
            item.width = 50;
            item.height = 50;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("peeper");
            item.shootSpeed = 2f;
            item.buffType = mod.BuffType("peeperbuff");
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.altFunctionUse != 2;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }
}