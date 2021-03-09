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
            npc.damage = RetributionWorld.nightmareMode ? 40 : 30;
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
        //to make sure you are first entering the phase
        private bool firstEnter2 = true;
        private bool firstEnter3 = true;
        private bool firstEnter4 = true;
        private bool firstEnter5 = true;
        private Vector2 randPos;
        private Vector2 phase2Velocity;
        private float dist;
        private Vector2 randPosTarget;
        private int prevAttackRand;
        private int attackRand2;





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
            //Phase I
            if (npc.life > npc.lifeMax * (Main.expertMode ? RetributionWorld.nightmareMode ? 0.9f : 0.8f : 0.75f)) {
                if ((counter % (Main.expertMode ? RetributionWorld.nightmareMode ? 270 : 360 : 420)) == 0) {
                    attackRand = Main.rand.Next(0, 2);
                    attackCounter = 0;
                }
                attackCounter++;
                //attack from left
                if (attackRand == 0)
                {
                    if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                    {
                        if (attackCounter % (Main.expertMode ? RetributionWorld.nightmareMode ? 40 : 60 : 90) == 0) {
                            ringBall(npc, 0);
                        }
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X - (Main.screenWidth / 3), player.Center.Y), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 200 : 270 : 360))
                    {
                        //dash code
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                        {
                            npcPos = npc.Center;
                            target = player.Center;
                            //pauses boss movement so the player can react before boss dashes
                            npc.velocity *= 0;
                        }
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 150 : 210 : 270))
                        {
                            Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
                        }
                        if (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 150 : 210 : 270))
                        {
                            AIMethods.DashToward(npcPos, target, (2 * (Main.screenWidth / 3)) / (Main.expertMode ? RetributionWorld.nightmareMode ? 50 : 60 : 90), npc);
                        }
                    }
                    //goes above the player.
                    else
                    {
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X, player.Center.Y - (Main.screenHeight / 3)), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                }
                //attack from right
                else if (attackRand == 1)
                {
                    if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                    {
                        if (attackCounter % (Main.expertMode ? RetributionWorld.nightmareMode ? 40 : 60 : 90) == 0)
                        {
                            ringBall(npc, 0);
                        }
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X + (Main.screenWidth / 3), player.Center.Y), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 200 : 270 : 360))
                    {
                        //dash code
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240))
                        {
                            npcPos = npc.Center;
                            target = player.Center;
                            //pauses boss movement so the player can react before boss dashes
                            npc.velocity *= 0;
                        }
                        if (attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 150 : 210 : 270)) {
                            Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
                        }
                        if (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 150 : 210 : 270))
                        {
                            AIMethods.DashToward(npcPos, target, (2 * (Main.screenWidth / 3)) / (Main.expertMode ? RetributionWorld.nightmareMode ? 50 : 60 : 90), npc);
                        }
                    }
                    //goes above the player.
                    else
                    {
                        AIMethods.MoveToward(npc, new Vector2(player.Center.X, player.Center.Y - (Main.screenHeight / 3)), (Main.expertMode ? RetributionWorld.nightmareMode ? 15f : 12f : 9f), 20f);
                    }
                    if (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 300 : 360 : 420)) {
                        attackCounter = 0;
                    }
                }
            }

            //Phase II

            else if (npc.life > npc.lifeMax * (Main.expertMode ? RetributionWorld.nightmareMode ? 0.75f : 0.6f : 0.5f)) {
                if (firstEnter2 == true) {
                    attackCounter = 0;
                    firstEnter2 = false;
                    //Add a visual effect to signify a change
                }
                dist = (Main.screenWidth / 4) / (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240);
                attackCounter++;
                if (attackCounter == 1)
                {
                    attackRand = Main.rand.Next(0, 4);
                    while (attackRand == prevAttackRand)
                    {
                        attackRand = Main.rand.Next(0, 4);
                    }
                    prevAttackRand = attackRand;
                }
                if (attackCounter < 6) {
                    switch (attackRand)
                    {
                        case 0:
                            randPos = new Vector2(0, Main.screenWidth / 4) + player.Center;
                            phase2Velocity = new Vector2(-dist, -dist);
                            randPosTarget = new Vector2(-Main.screenWidth / 4, 0) + player.Center;
                            break;
                        case 1:
                            randPos = new Vector2(0, -Main.screenWidth / 4) + player.Center;
                            phase2Velocity = new Vector2(dist, dist);
                            randPosTarget = new Vector2(Main.screenWidth / 4, 0) + player.Center;
                            break;
                        case 2:
                            randPos = new Vector2(Main.screenWidth / 4, 0) + player.Center;
                            phase2Velocity = new Vector2(-dist, dist);
                            randPosTarget = new Vector2(0, Main.screenWidth / 4) + player.Center;
                            break;
                        case 3:
                            randPos = new Vector2(-Main.screenWidth / 4, 0) + player.Center;
                            phase2Velocity = new Vector2(dist, -dist);
                            randPosTarget = new Vector2(0, -Main.screenWidth / 4) + player.Center;
                            break;
                    }
                } else if (attackCounter == 6) {
                    npc.Center = randPos;
                    npc.velocity = phase2Velocity;
                } else if (attackCounter % 60 == 0) {
                    fanBolt(npc, player.Center);
                } else if (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 120 : 180 : 240)) {
                    attackCounter = 0;
                }
                Lighting.AddLight(randPos, 0.164f, 0, 0.878f);
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(randPos, npc.width, npc.height, 27);
                    Dust.NewDust(randPos, npc.width, npc.height, 57);
                    Dust.NewDust(randPos, npc.width, npc.height, 62);
                    Dust.NewDust(randPosTarget, npc.width, npc.height, 27);
                    Dust.NewDust(randPosTarget, npc.width, npc.height, 57);
                    Dust.NewDust(randPosTarget, npc.width, npc.height, 62);
                }
                if (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 122 : 184 : 246)) {
                    attackCounter = 0;
                }
            } 
            
            //Phase III

            else if (npc.life > npc.lifeMax * (Main.expertMode ? RetributionWorld.nightmareMode ? 0.5f : 0.4f : 0.3f)) {
                if (firstEnter3 == true) {
                    attackCounter = 0;
                    firstEnter3 = false;
                    //add a visual effect to signal a change
                }
                if (attackCounter == 0) {
                    attackRand2 = Main.rand.Next(0, 2);
                }
                attackCounter++;
                if (attackRand2 == 0)
                {
                    if (attackCounter == 1 || attackCounter == (Main.expertMode ? RetributionWorld.nightmareMode ? 140 : 150 : 160))
                    {
                        attackRand = Main.rand.Next(0, 4);
                        while (attackRand == prevAttackRand)
                        {
                            attackRand = Main.rand.Next(0, 4);
                        }
                        prevAttackRand = attackRand;
                    }
                    dist = (Main.screenWidth / 2) / (Main.expertMode ? RetributionWorld.nightmareMode ? 40 : 50f : 60f);
                    if (attackCounter < 60 || (attackCounter > (Main.expertMode ? RetributionWorld.nightmareMode ? 140 : 150 : 160) && attackCounter < 240))
                    {
                        npcPos = npc.Center;
                        switch (attackRand)
                        {
                            case 0:
                                randPos = new Vector2(0, Main.screenWidth / 4) + player.Center;
                                phase2Velocity = new Vector2(0, -dist);
                                randPosTarget = new Vector2(0, -Main.screenWidth / 4) + player.Center;
                                break;
                            case 1:
                                randPos = new Vector2(0, -Main.screenWidth / 4) + player.Center;
                                phase2Velocity = new Vector2(0, dist);
                                randPosTarget = new Vector2(0, Main.screenWidth / 4) + player.Center;
                                break;
                            case 2:
                                randPos = new Vector2(Main.screenWidth / 4, 0) + player.Center;
                                phase2Velocity = new Vector2(-dist, 0);
                                randPosTarget = new Vector2(-Main.screenWidth / 4, 0) + player.Center;
                                break;
                            case 3:
                                randPos = new Vector2(-Main.screenWidth / 4, 0) + player.Center;
                                phase2Velocity = new Vector2(dist, 0);
                                randPosTarget = new Vector2(Main.screenWidth / 4, 0) + player.Center;
                                break;
                        }
                    }
                    //go to position
                    if (attackCounter < 60)
                    {
                        AIMethods.MoveToward(npc, randPos, 14f, 10f);
                    }
                    //pause
                    else if (attackCounter < 100)
                    {
                        npc.velocity *= 0;
                    }
                    //dash #1
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 140 : 150 : 160))
                    {
                        if (attackCounter == 100)
                        {
                            Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
                        }
                        AIMethods.DashToward(npcPos, randPosTarget, dist, npc);
                    }
                    //go to position
                    else if (attackCounter < 200)
                    {
                        AIMethods.MoveToward(npc, randPos, 14f, 10f);
                    }
                    //pause
                    else if (attackCounter < 240)
                    {
                        npc.velocity *= 0;
                    }
                    //dash #2
                    else if (attackCounter < (Main.expertMode ? RetributionWorld.nightmareMode ? 280 : 290 : 300))
                    {
                        if (attackCounter == 240)
                        {
                            Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
                        }
                        AIMethods.DashToward(npcPos, randPosTarget, dist, npc);
                    }
                    //go to position
                    else
                    {
                        AIMethods.MoveToward(npc, randPos, 14f, 10f);
                    }
                    //reset counter
                    if (attackCounter > 360)
                    {
                        attackCounter = 0;
                    }
                }
                else {
                    npc.velocity *= 0.5f;
                    if (attackCounter < 360) {
                        AIMethods.ShootRing(Main.expertMode ? RetributionWorld.nightmareMode ? 5 : 4 : 3, ModContent.ProjectileType<SandBall>(), 6f, 2f, 20, npc.Center, counter * 3.5f);
                    }
                    if (attackCounter > 420) {
                        attackCounter = 0;
                    }
                    
                
                
                
                }
            }


        }
        public void ringBolt(NPC npc, int rot) {
            Main.PlaySound(SoundID.Item45, npc.Center);
            AIMethods.ShootRing(8, ModContent.ProjectileType<SandBolt>(), Main.expertMode ? RetributionWorld.nightmareMode ? 18 : 15 : 12, 2f, Main.expertMode ? RetributionWorld.nightmareMode ? 30 : 20 : 10, npc.Center, rot);
            npc.netUpdate = true;
        }
        public void ringBall(NPC npc, int rot)
        {
            Main.PlaySound(SoundID.Item45, npc.Center);
            AIMethods.ShootRing(8, ModContent.ProjectileType<SandBolt>(), Main.expertMode ? RetributionWorld.nightmareMode ? 18 : 15 : 12, 2f, Main.expertMode ? RetributionWorld.nightmareMode ? 30 : 20 : 10, npc.Center, rot);
            npc.netUpdate = true;
        }
        public void fanBolt(NPC npc, Vector2 target) {
            Main.PlaySound(SoundID.Item45, npc.Center);
            AIMethods.ShootFan(Main.expertMode ? RetributionWorld.nightmareMode ? 5 : 4 : 3, ModContent.ProjectileType<SandBolt>(), 8f, 2f, Main.expertMode ? RetributionWorld.nightmareMode ? 30 : 20 : 10, target, 110, npc.Center);
            npc.netUpdate = true;
        }
        public void fanBall(NPC npc, Vector2 target)
        {
            Main.PlaySound(SoundID.Item45, npc.Center);
            AIMethods.ShootFan(Main.expertMode ? RetributionWorld.nightmareMode ? 5 : 4 : 3, ModContent.ProjectileType<SandBall>(), 8f, 2f, Main.expertMode ? RetributionWorld.nightmareMode ? 30 : 20 : 10, target, 110, npc.Center);
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
            Main.NewText("The Desert sands are shifting...", 111, 199, 214, true);

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