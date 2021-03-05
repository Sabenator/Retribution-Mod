using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Buffs;
using Retribution.Items.Bags;

namespace Retribution.NPCs.Bosses.Silva
{
    [AutoloadBossHead]
    public class Silva : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elder Oak");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            if (RetributionWorld.nightmareMode == false)
            {
                npc.lifeMax = 1800;
            }
            if (RetributionWorld.nightmareMode == true)
            {
                npc.lifeMax = 2300;
            }

            if (RetributionWorld.nightmareMode == false)
            {
                npc.damage = 20;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                npc.damage = 35;
            }

            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.width = 202;
            npc.height = 242;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.noTileCollide = true;
            npc.DeathSound = SoundID.DD2_DrakinDeath;
            npc.buffImmune[BuffID.Confused] = true;
            npc.behindTiles = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silva");
            bossBag = ModContent.ItemType<Silva_Bag>();

        }

        private double counting;

        public int frameIdle = 0;
        public int frameIceBolt = 1;
        public int frameEnraged = 2;

        private int frame;

        private bool enraged = false;
        private bool moving = true;
        private int soundAmount = 0;
        private int spawnAmount = 1;

        private bool stateIdle = true;
        private bool stateIceBolt = false;

        private int summonTimer;
        private int leafTimer;
        private int logTimer;
        private int logTimer2;

        public bool doneSpawning = false;
        private bool canLog = true;
        private bool canFireTrigger = false;
        private bool canFire = true;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void AI()
        {

            if (doneSpawning == false)
            {
                if (spawnAmount == 1)
                {
                    Projectile.NewProjectile(npc.Center.X - 20, npc.Center.Y - 122, 0, 0, ModContent.ProjectileType<spawningDust>(), 0, 0f, Main.myPlayer, npc.whoAmI, 200);
                    spawnAmount = 2;
                    npc.damage = 0;
                }
                npc.position.Y -= 0.7f;
                npc.ai[0]++;
                Lighting.AddLight((int)npc.Center.X, (int)npc.Center.Y, 0.7f, 0.7f, 0.7f);

                if (npc.ai[0] >= 329)
                {
                    npc.ai[0] = 0;
                    doneSpawning = true;
                    npc.noGravity = false;
                    npc.noTileCollide = false;

                    if (RetributionWorld.nightmareMode == false)
                    {
                        npc.damage = 20;
                    }

                    if (RetributionWorld.nightmareMode == true)
                    {
                        npc.damage = 35;
                    }
                }
            }
            else
            {
                #region Animation Sets
                if (stateIdle == true)
                {
                    this.frame = frameIdle;
                }

                if (stateIceBolt == true)
                {
                    this.frame = frameIceBolt;
                }
                #endregion

                #region Despawn
                Player player = Main.player[npc.target];

                if (!player.active || player.dead)
                {
                    npc.TargetClosest(false);
                    player = Main.player[npc.target];
                    if (!player.active || player.dead)
                    {
                        npc.velocity = new Vector2(0f, 10f);
                        if (npc.timeLeft > 30)
                        {
                            npc.timeLeft = 30;
                        }
                        return;
                    }
                }
                #endregion

                #region Enrage Trigger
                if (npc.life <= npc.lifeMax / 2)
                {
                    enraged = true;
                }

                if (enraged == true && soundAmount == 0)
                {
                    Main.PlaySound(SoundID.DD2_DrakinDeath, (int)npc.position.X, (int)npc.position.Y);

                    soundAmount = 1;
                }
                #endregion

                #region Razor Leaf

                if (canFireTrigger == true)
                {
                    npc.ai[1]++;
                    if (npc.ai[1] > 180)
                    {
                        canFire = true;
                        canFireTrigger = false;
                        npc.ai[1] = 0;
                    }
                }

                if (canFire == true)
                {
                    leafTimer += 1;

                    if (leafTimer > 120 && enraged == false && Main.netMode != 1)
                    {
                        base.npc.TargetClosest(true);
                        Vector2 vector = Main.player[base.npc.target].Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
                        Vector2 vector2 = base.npc.Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
                        base.npc.netUpdate = true;

                        Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 45, 1f, 0f);
                        float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
                        int i = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)(Math.Cos((double)num) * 2.0 * -1.0), (float)(Math.Sin((double)num) * 2.0 * -1.0), ProjectileID.Leaf, 5, 0f, 0, 0f, 0f);
                        Main.projectile[i].hostile = true;
                        Main.projectile[i].friendly = false;

                        if (Main.rand.NextFloat() < .50f && Main.netMode != 1)
                        {
                            Main.projectile[i].tileCollide = false;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            Main.projectile[i].tileCollide = true;
                        }
                        npc.netUpdate = true;

                        if (leafTimer > 130)
                        {
                            leafTimer = 0;
                        }
                    }

                    if (leafTimer > 60 && enraged == true && Main.netMode != 1)
                    {
                        base.npc.TargetClosest(true);
                        Vector2 vector = Main.player[base.npc.target].Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
                        Vector2 vector2 = base.npc.Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
                        base.npc.netUpdate = true;

                        Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 45, 1f, 0f);
                        float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
                        int i = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)(Math.Cos((double)num) * 2.0 * -1.0), (float)(Math.Sin((double)num) * 2.0 * -1.0), ProjectileID.Leaf, 5, 0f, 0, 0f, 0f);
                        Main.projectile[i].hostile = true;
                        Main.projectile[i].friendly = false;

                        if (Main.rand.NextFloat() < .50f && Main.netMode != 1)
                        {
                            Main.projectile[i].tileCollide = false;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            Main.projectile[i].tileCollide = true;
                        }
                        npc.netUpdate = true;

                        if (leafTimer > 70)
                        {
                            leafTimer = 0;
                        }
                    }
                }
                
                #endregion

                #region Summon
                summonTimer += 1;

                if (summonTimer > 500 && Main.rand.NextFloat() < .40f && !NPC.AnyNPCs(ModContent.NPCType<EarthHeadMin>()) && Main.netMode != 1)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 100, ModContent.NPCType<EarthHeadMin>());
                    summonTimer = 0;
                    npc.netUpdate = true;
                }
                else if (summonTimer > 501)
                {
                    summonTimer = 0;
                }

                if (summonTimer > 500 && Main.rand.NextFloat() < .40f && !NPC.AnyNPCs(ModContent.NPCType<LeafHead>()) && enraged == false && RetributionWorld.nightmareMode == true && Main.netMode != 1)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 100, ModContent.NPCType<LeafHead>());
                    summonTimer = 0;
                    npc.netUpdate = true;
                }
                else if (summonTimer > 501)
                {
                    summonTimer = 0;
                }

                #endregion

                #region Spawn Log

                if (RetributionWorld.nightmareMode == true && enraged == true)
                {
                    logTimer++;

                    if (logTimer > 180 && Main.rand.NextFloat() < .30f && canLog == true && Main.netMode != 1)
                    {
                        canFire = false;
                        logTimer = 0;
                        canLog = false;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<SilvaAttach>(), 0, 0f, 0, 0f, 0f);
                        npc.netUpdate = true;
                    }

                    if (canLog == false && Main.netMode != 1)
                    {
                        logTimer2++;

                        if (logTimer2 > 60 && Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<SilvaAttach>(), 0, 0f, 0, 0f, 0f);
                            npc.netUpdate = true;
                            canLog = true;
                            logTimer2 = 0;
                            canFireTrigger = true;
                        }
                    }

                    else if (logTimer > 181)
                    {
                        logTimer = 0;
                    }
                }

                #endregion

                player.GetModPlayer<TreeLock>().screenLock = npc.whoAmI;
            }
        }

        /*public override void FindFrame(int frameHeight)
        {
            #region Idle Frames
            if (this.frame == frameIdle)
            {
                counting += 1.0;
                if (counting < 6.0)
                {
                    npc.frame.Y = 0;
                }
                else if (counting < 12.0)
                {
                    npc.frame.Y = frameHeight;
                }
                else if (counting < 18.0)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (counting < 24.0)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else
                {
                    counting = 0.0;
                }
            }
            #endregion

            #region IceBolt Frames
            if (this.frame == frameIceBolt)
            {
                counting += 1.0;
                if (counting < 6.0)
                {
                    npc.frame.Y = frameHeight * 4;
                    stateIdle = false;
                }
                else if (counting < 12.0)
                {
                    npc.frame.Y = frameHeight * 5;
                    stateIdle = false;
                }
                else if (counting < 18.0)
                {
                    npc.frame.Y = frameHeight * 6;
                    stateIdle = false;
                }
                else if (counting < 24.0)
                {
                    npc.frame.Y = frameHeight * 7;
                    stateIdle = false;
                }
                else if (counting < 30.0)
                {
                    npc.frame.Y = frameHeight * 8;
                    stateIdle = false;
                }
                else if (counting < 36.0)
                {
                    npc.frame.Y = frameHeight * 9;
                    stateIdle = false;
                }
                else if (counting < 42.0)
                {
                    npc.frame.Y = frameHeight * 10;
                    stateIdle = false;
                }
                else if (counting < 48.0)
                {
                    npc.frame.Y = frameHeight * 11;
                    stateIdle = false;
                }
                else if (counting > 54.0)
                {
                    stateIdle = true;
                    stateIceBolt = false;
                    counting = 0.0;
                }
            }
            #endregion
        }*/

        public override void NPCLoot()
        {
            Main.NewText("The spirits of the Forest have been unveiled...", 61, 153, 69, true);

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tesca1"), 1f);
            Gore.NewGore(npc.position, -npc.velocity, mod.GetGoreSlot("Gores/Tesca0"), 1f);


            if (Main.expertMode)
            {
                npc.DropBossBags();
            }

            RetributionWorld.downedSilva = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }

    public class TreeLock : ModPlayer
    {
        public int screenLock = -1;

        public override void ResetEffects()
        {
            screenLock = -1;
        }

        public override void ModifyScreenPosition()
        {
            if (screenLock != -1 && player.active && player.statLife > 0)
            {
                NPC Tree = Main.npc[screenLock];
                if (Tree.active)
                {
                    Main.screenPosition.X = Tree.position.X - Main.screenWidth / 1.2f;
                    Main.screenPosition.Y = Tree.position.Y - Main.screenHeight;
                }
            }
        }
    }
}