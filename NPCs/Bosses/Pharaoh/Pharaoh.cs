using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Buffs;
using Retribution.Items.Weapons.Ranger;
using Retribution.Items.Bags;

namespace Retribution.NPCs.Bosses.Pharaoh
{
    [AutoloadBossHead]
    public class Pharaoh : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Buried Pharaoh");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = RetributionWorld.nightmareMode ? 7000 : 5000;
            npc.damage = RetributionWorld.nightmareMode ? 60 : 40;
            npc.defense = 21;
            npc.knockBackResist = 0f;
            npc.width = 52;
            npc.height = 180;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.alpha = 0;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            NPCVuls.toWater = true;
            NPCVuls.fireImmune = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/inferis");
            bossBag = ModContent.ItemType<Tesca_Bag>();
        }
        //Animation Variables
        private double counting;
        public int frameIdle = 0;
        private int frame;


        //AI Variables
        private int counter = 0;
        private int attackCounter = 0;
        private int attackRand = 0;
        private Vector2 npcPos;
        private Vector2 target;





        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax += 1000;
            npc.damage += 20;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            DespawnHandler();
            counter++;
            if (npc.life > npc.lifeMax * (Main.expertMode ? RetributionWorld.nightmareMode ? 0.8f : 0.75f : 0.66f)) {
                if ((counter % (Main.expertMode ? RetributionWorld.nightmareMode ? 360 : 420 : 480)) == 0) {
                    attackRand = Main.rand.Next(0, 2);
                    attackCounter = 0;
                }
                attackCounter++;
                if (attackRand == 0)
                {
                    if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                    {
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X - (Main.screenWidth / 3), player.Center.Y), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 160 : 240 : 330))
                    {
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                        {
                            Main.PlaySound(SoundID.DD2_DarkMageCastHeal, npc.Center);
                            npcPos = npc.Center;
                            target = player.Center;
                        }
                        AIMethods.DashToward(npcPos, target, (2 * (Main.screenWidth / 3)) / (Main.expertMode ? RetributionWorld.nightmareMode ? 40 : 60 : 90), npc);
                    }
                    else
                    {
                        if (attackCounter == 359 || attackCounter == 419 || attackCounter == 479)
                        {
                            ring(npc, Main.rand.Next(0, 180));
                        }
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X, player.Center.Y - (Main.screenHeight / 3)), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                }
                else if (attackRand == 1)
                {
                    if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                    {
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X + (Main.screenWidth / 3), player.Center.Y), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 160 : 240 : 330))
                    {
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                        {
                            Main.PlaySound(SoundID.DD2_DarkMageCastHeal, npc.Center);
                            npcPos = npc.Center;
                            target = player.Center;
                        }
                        AIMethods.DashToward(npcPos, target, (2 * (Main.screenWidth / 3)) / (Main.expertMode ? RetributionWorld.nightmareMode ? 40 : 60 : 90), npc);
                    }
                    else
                    {
                        if (attackCounter == 359 || attackCounter == 419 || attackCounter == 479) {
                            ring(npc, Main.rand.Next(0, 180));
                        }
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X, player.Center.Y - (Main.screenHeight / 3)), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                }
            }
        }
        public void ring(NPC npc, int rot) {
            Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
            AIMethods.ShootRing(8, ModContent.ProjectileType<SandSpikeWarn>(), Main.expertMode ? RetributionWorld.nightmareMode ? 18 : 15 : 12, 2f, Main.expertMode ? RetributionWorld.nightmareMode ? 90 : 70 : 50, npc.Center, rot);
            npc.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            counting += 1.0;
            if (counting < 5.0)
            {
                npc.frame.Y = 0;

            }
            else if (counting < 10.0)
            {
                npc.frame.Y = frameHeight;
            }
            else if (counting < 15.0)
            {
                npc.frame.Y = frameHeight * 2;
            }
            else if (counting < 20.0)
            {
                npc.frame.Y = frameHeight * 3;
            }
            else if (counting < 25.0)
            {
                npc.frame.Y = frameHeight * 4;
            }
            else if (counting < 30.0)
            {
                npc.frame.Y = frameHeight * 5;
            }
            else if (counting < 35.0)
            {
                npc.frame.Y = frameHeight * 6;
            }
            else if (counting < 40.0)
            {
                npc.frame.Y = frameHeight * 7;
            }
            else
            {
                counting = 0.0;
            }
        }

        public override void NPCLoot()
        {
            Main.NewText("The essence of the Desert has been released...", 111, 199, 214, true);

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tesca1"), 1f);
            Gore.NewGore(npc.position, -npc.velocity, mod.GetGoreSlot("Gores/Tesca0"), 1f);

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }

            Main.PlaySound(SoundID.NPCKilled, npc.Center, 33);

            RetributionWorld.downedTesca = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        private void DespawnHandler()
        {
            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.velocity *= 0.96f;
                npc.velocity.Y -= 1;
                if (npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
                npc.netUpdate = true;
                return;
            }
        }
    }
}