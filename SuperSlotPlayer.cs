using Retribution.Items.StarterBags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerraUI.Objects;
using Terraria.UI;
using Terraria.GameInput;

namespace Retribution
{
    internal class SuperSlotPlayer : ModPlayer
    {
        private const string SuperTag = "supers";

        public UIItemSlot EquipSlot;

        public override void clientClone(ModPlayer clientClone)
        {
            SuperSlotPlayer clone = clientClone as SuperSlotPlayer;

            if (clone == null)
            {
                return;
            }

            clone.EquipSlot.Item = EquipSlot.Item.Clone();
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            SuperSlotPlayer oldClone = clientPlayer as SuperSlotPlayer;

            if (oldClone == null)
            {
                return;
            }

            if (oldClone.EquipSlot.Item.IsNotTheSameAs(EquipSlot.Item))
            {
                SendSingleItemPacket(PacketMessageType.EquipSlot, EquipSlot.Item, -1, player.whoAmI);
            }
        }

        internal void SendSingleItemPacket(PacketMessageType message, Item item, int toWho, int fromWho)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)message);
            packet.Write((byte)player.whoAmI);
            ItemIO.Send(item, packet);
            packet.Send(toWho, fromWho);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)PacketMessageType.All);
            packet.Write((byte)player.whoAmI);
            ItemIO.Send(EquipSlot.Item, packet);
            packet.Send(toWho, fromWho);
        }

        public override void Initialize()
        {
            EquipSlot = new UIItemSlot(Vector2.Zero, context: ItemSlot.Context.EquipAccessory, hoverText: "Super",
                conditions: Slot_Conditions, drawBackground: Slot_DrawBackground, scaleToInventory: true);

            InitializeSuper();
        }

        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound.Add("super", ItemIO.Save(EquipSlot.Item));
            return tagCompound;
        }

        public override void Load(TagCompound tag)
        {
            ItemIO.Load(tag.GetCompound("super"));
        }

        private void Slot_DrawBackground(UIObject sender, SpriteBatch spriteBatch)
        {
            UIItemSlot slot = (UIItemSlot)sender;

            if (ShouldDrawSlots())
            {
                slot.OnDrawBackground(spriteBatch);

                if (slot.Item.stack == 0)
                {
                    Texture2D tex = mod.GetTexture(Retribution.SuperSlotBackTex);
                    Vector2 origin = tex.Size() / 2f * Main.inventoryScale;
                    Vector2 position = slot.Rectangle.TopLeft();

                    spriteBatch.Draw(
                        tex,
                        position + (slot.Rectangle.Size() / 2f) - (origin / 2f),
                        null,
                        Color.White * 0.35f,
                        0f,
                        origin,
                        Main.inventoryScale,
                        SpriteEffects.None,
                        0f); // layer depth 0 = front
                }
            }
        }

        private static bool Slot_Conditions(Item item)
        {
            if (item.modItem is Super)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int wingSlotAdd;

        public override void PreUpdate()
        {
            Mod wingSlot = ModLoader.GetMod("WingSlot");

            if (wingSlot != null)
            {
                wingSlotAdd = -1;
            }
            else
            {
                wingSlotAdd = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!ShouldDrawSlots())
            {
                return;
            }

            int mapH = 0;
            int rX;
            int rY;
            float origScale = Main.inventoryScale;

            Main.inventoryScale = 0.85f;

            if (Main.mapEnabled)
            {
                if (!Main.mapFullscreen && Main.mapStyle == 1)
                {
                    mapH = 256;
                }
            }

            if (!Retribution.SlotsNextToAccessories)
            {
                if (Main.mapEnabled)
                {
                    if ((mapH + 600) > Main.screenHeight)
                    {
                        mapH = Main.screenHeight - 600;
                    }
                }

                rX = Main.screenWidth - 92 - (47 * 2);
                rY = mapH + 174;

                if (Main.netMode == 1)
                {
                    rX -= 47;
                }
            }
            else
            {
                if (Main.mapEnabled)
                {
                    int adjustY = 600;

                    if (Main.player[Main.myPlayer].ExtraAccessorySlotsShouldShow)
                    {
                        adjustY = 610 + PlayerInput.UsingGamepad.ToInt() * 30;
                    }

                    if ((mapH + adjustY) > Main.screenHeight)
                    {
                        mapH = Main.screenHeight - adjustY;
                    }
                }

                int slotCount = 7 + Main.player[Main.myPlayer].extraAccessorySlots + wingSlotAdd;

                if ((Main.screenHeight < 900) && (slotCount >= 8))
                {
                    slotCount = 7 + wingSlotAdd;
                }

                rX = Main.screenWidth - 92 - 14 - (47 * 3) - (int)(Main.extraTexture[58].Width * Main.inventoryScale);
                rY = (int)(mapH + 174 + 4 + slotCount * 56 * Main.inventoryScale);
            }

            EquipSlot.Position = new Vector2(rX, rY);

            EquipSlot.Draw(spriteBatch);

            Main.inventoryScale = origScale;

            EquipSlot.Update();
        }

        private static bool ShouldDrawSlots()
        {
            return Main.playerInventory && ((Retribution.SlotsNextToAccessories && Main.EquipPage == 0) ||
                    (!Retribution.SlotsNextToAccessories && Main.EquipPage == 2));
        }

        private void InitializeSuper()
        {
            EquipSlot.Item = new Item();
            EquipSlot.Item.SetDefaults(0, true);
        }
    }
}