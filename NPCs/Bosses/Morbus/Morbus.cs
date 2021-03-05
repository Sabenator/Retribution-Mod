using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Projectiles;

namespace Retribution.NPCs.Bosses.Morbus
{
    [AutoloadBossHead]
    public class Morbus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morbus");
            Main.npcFrameCount[npc.type] = 15;
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            if (RetributionWorld.nightmareMode == false)
            {
                npc.lifeMax = 20000;
            }
            if (RetributionWorld.nightmareMode == true)
            {
                npc.lifeMax = 30000;
            }

            if (RetributionWorld.nightmareMode == false)
            {
                npc.damage = 40;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                npc.damage = 60;
            }

            npc.defense = 16;
            npc.knockBackResist = 0f;
            npc.width = 120;
            npc.height = 126;
            npc.value = Item.buyPrice(0, 12, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.alpha = 0;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Ichor] = true;

            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/morbus");
        }

        private double counting;

        public int frameIdle = 0;
        public int frameBlink = 1;
        public int frameFire = 2;
        public int frameSummon = 3;
        public int frameEnraged = 4;

        private int frame;

        private bool enraged = false;
        private bool moving = true;
        private int soundAmount = 0;

        private bool stateIdle = false;
        private bool stateBlink = false;
        private bool stateFire = false;
        private bool stateFire2 = false;
        private bool stateSummon = false;
        private bool stateEnraged = false;

        private int fire2;
        private int summonTimer;
        private int fangTimer;
        private int fangTimer2;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1.6f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (enraged == true)
            {
                Vector2 vector = new Vector2(0f, 0f);
                SpriteEffects effects = (npc.spriteDirection < 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                for (int i = 0; i < npc.oldPos.Length; i++)
                {
                    Vector2 position = npc.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, npc.gfxOffY);
                    Color value = npc.GetAlpha(new Color(255, 255, 255, 10)) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                    spriteBatch.Draw(Main.npcTexture[npc.type], position, new Rectangle?(npc.frame), value * 0.25f, npc.rotation, vector, npc.scale, effects, 0f);
                }

                SpriteEffects spriteEffects = SpriteEffects.None;

                Texture2D glow = ModContent.GetTexture("Retribution/NPCs/Bosses/Glowmasks/Morbus");

                Vector2 vector11 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
                Vector2 vector12 = npc.Center - Main.screenPosition;
                vector12 -= new Vector2((float)Main.npcTexture[npc.type].Width, (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f;
                vector12 += vector11 * npc.scale + new Vector2(0f, 4f + npc.gfxOffY);
                Color color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

                spriteBatch.Draw(Main.npcTexture[npc.type], vector12, new Rectangle?(npc.frame), npc.GetAlpha(lightColor), npc.rotation, vector11, npc.scale, spriteEffects, 0f);
                spriteBatch.Draw(glow, vector12, new Rectangle?(npc.frame), color37, npc.rotation, vector11, npc.scale, spriteEffects, 0f);
            }
            return true;
        }

        public override void AI()
        {
                #region Animation Sets
                if (stateIdle == true)
                {
                    this.frame = frameIdle;
                }

                if (stateBlink == true)
                {
                    this.frame = frameBlink;
                }

                if (stateFire == true)
                {
                    this.frame = frameFire;
                }

                if (stateSummon == true)
                {
                    this.frame = frameSummon;
                }
                #endregion

                #region Despawn
                Player player = Main.player[npc.target];

                if (!player.active || player.dead && Main.netMode != 1)
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
                if (npc.life <= npc.lifeMax / 2 && Main.netMode != 1)
                {
                    enraged = true;
                    stateEnraged = true;
                    stateIdle = false;
                    stateBlink = false;
                    frame = frameEnraged;
                    npc.alpha = 100;
                }

                if (enraged == true && soundAmount == 0 && Main.netMode != 1)
                {
                    Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Morbus_Roar"));
                    Main.PlaySound(SoundID.DD2_BetsyScream, npc.Center);

                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Morbus_Left"), 1f);

                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Morbus_Right"), 1f);

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveEffect>(), 0, 0f, 255, 0f, 0f);

                    soundAmount = 1;
                }
                #endregion

                #region Idle
                if (moving == true)
                {
                    if (stateIdle == true && stateBlink == false)
                    {
                        this.frame = frameIdle;
                    }

                    npc.ai[1]++;

                    if (npc.ai[1] >= 60 && Main.rand.NextFloat() < .30f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        stateIdle = false;
                        stateBlink = true;
                        npc.ai[1] = 0;
                    }

                    if (npc.ai[1] >= 61)
                    {
                        npc.ai[1] = 0;
                    }

                    npc.TargetClosest(true);
                    Vector2 targetPosition = Main.player[npc.target].position;

                    if (enraged == false)
                    {
                        if (targetPosition.X < npc.position.X && npc.velocity.X > -4)
                        {
                            npc.netUpdate = true;
                            npc.velocity.X -= 0.05f;
                        }
                        if (targetPosition.X > npc.position.X && npc.velocity.X < 4)
                        {
                            npc.netUpdate = true;
                            npc.velocity.X += 0.05f;
                        }
                        if (Main.player[npc.target].position.Y < npc.position.Y + 350f)
                        {
                            npc.netUpdate = true;
                            NPC npc2 = npc;
                            npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.45f : 0.05f);
                        }
                        if (Main.player[npc.target].position.Y > npc.position.Y + 250f)
                        {
                            npc.netUpdate = true;
                            NPC npc3 = npc;
                            npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.45f : 0.05f);
                        }

                        npc.position += npc.velocity;
                    }
                    else if (enraged == true)
                    {
                        if (targetPosition.X < npc.position.X && npc.velocity.X > -8 && Main.netMode != 1)
                        {
                            npc.netUpdate = true;
                            npc.velocity.X -= 0.2f;
                        }
                        if (targetPosition.X > npc.position.X && npc.velocity.X < 8 && Main.netMode != 1)
                        {
                            npc.netUpdate = true;
                            npc.velocity.X += 0.2f;
                        }
                        if (Main.player[npc.target].position.Y < npc.position.Y + 250f && Main.netMode != 1)
                        {
                            npc.netUpdate = true;
                            NPC npc2 = npc;
                            npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.75f : 0.06f);
                        }
                        if (Main.player[npc.target].position.Y > npc.position.Y + 250f && Main.netMode != 1)
                        {
                            npc.netUpdate = true;
                            NPC npc3 = npc;
                            npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.75f : 0.06f);
                        }

                        npc.position += npc.velocity;
                    }

                }
                #endregion

                #region Fire
                npc.ai[2]++;

                if (enraged == false)
                {
                    if (npc.ai[2] >= 500 && Main.rand.NextFloat() < .40f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        stateIdle = false;
                        stateFire = true;
                        moving = false;

                        npc.velocity.X = 0;

                        npc.velocity.Y = 0;

                        npc.ai[3]++;
                        if (npc.ai[3] >= 60)
                        {
                            Main.PlaySound(SoundID.NPCHit52, npc.position);

                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 0, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 2, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 4, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 6, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 8, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 40, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -2, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -4, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -6, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -8, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -40, -2, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);

                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 0, -15, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 1, -15, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 2, -15, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -1, -15, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -2, -15, ModContent.ProjectileType<cursedspit>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);


                            npc.ai[3] = 0;
                            npc.ai[2] = 0;
                            stateIdle = true;
                            moving = true;
                            stateFire = false;
                        }
                    }
                }

                if (enraged == true)
                {
                    if (npc.ai[2] >= 400 && Main.rand.NextFloat() < .50f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        stateIdle = false;
                        stateFire2 = true;
                        moving = false;

                        npc.velocity.X = 0;

                        npc.velocity.Y = 0;

                        npc.ai[3]++;
                        if (npc.ai[3] >= 30)
                        {
                            Main.PlaySound(SoundID.DD2_BetsyFireballShot, (int)npc.position.X, (int)npc.position.Y);

                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 0, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 1, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 2, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 3, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 4, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 5, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 6, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 7, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 8, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 9, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -1, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -2, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -3, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -4, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -5, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -6, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -7, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -8, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -9, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -30, -2, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);

                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 0, -15, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 1, -15, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, 2, -15, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -1, -15, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 60, -2, -15, ProjectileID.GoldenShowerHostile, 30, 0f, Main.myPlayer, npc.whoAmI, 30);

                            npc.ai[3] = 0;
                            npc.ai[2] = 0;
                            stateIdle = true;
                            moving = true;
                            stateFire2 = false;
                        }
                    }
                }

                #endregion

                #region Fire v2
                fire2++;

                if (enraged == false)
                {
                    if (fire2 >= 245 && Main.rand.NextFloat() < .35f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, 3, 2, ModContent.ProjectileType<cursedflameball>(), 10, 0f, Main.myPlayer, npc.whoAmI, 10);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, 1, 2, ModContent.ProjectileType<cursedflameball>(), 10, 0f, Main.myPlayer, npc.whoAmI, 10);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, -1, 2, ModContent.ProjectileType<cursedflameball>(), 10, 0f, Main.myPlayer, npc.whoAmI, 10);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, -3, 2, ModContent.ProjectileType<cursedflameball>(), 10, 0f, Main.myPlayer, npc.whoAmI, 10);

                        Main.PlaySound(SoundID.NPCHit52, npc.position);

                        fire2 = 0;
                    }
                }

                if (enraged == true)
                {
                    if (fire2 >= 150 && Main.rand.NextFloat() < .50f && Main.netMode != 1)
                    if (fire2 >= 150 && Main.rand.NextFloat() < .50f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, 3, 2, ModContent.ProjectileType<ichorball>(), 30, 0f, Main.myPlayer, npc.whoAmI, 40);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, 1, 2, ModContent.ProjectileType<ichorball>(), 30, 0f, Main.myPlayer, npc.whoAmI, 40);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, -1, 2, ModContent.ProjectileType<ichorball>(), 30, 0f, Main.myPlayer, npc.whoAmI, 40);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, -3, 2, ModContent.ProjectileType<ichorball>(), 30, 0f, Main.myPlayer, npc.whoAmI, 40);

                        Main.PlaySound(SoundID.NPCHit52, npc.position);

                        fire2 = 0;
                    }
                }
                #endregion

                #region Fang

                fangTimer++;
                fangTimer2++;

                if (fangTimer >= 500 && enraged == false && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 39);

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -10, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 8, -8, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -8, -8, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 10, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -10, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 8, 8, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -8, 8, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 10, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);

                    fangTimer = 0;
                }

                if (fangTimer >= 200 && enraged == true && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    Main.PlaySound(SoundID.DD2_BetsysWrathShot, (int)npc.position.X, (int)npc.position.Y);


                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -20, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 18, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -18, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 20, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -20, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 18, 18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -18, 18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 20, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);

                    fangTimer = 0;
                }

                if (fangTimer2 >= 300 && enraged == true && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    Main.PlaySound(SoundID.DD2_BetsysWrathImpact, (int)npc.position.X, (int)npc.position.Y);

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -20, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 18, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -18, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 20, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -20, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 18, 20, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -18, 18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 20, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 38, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -38, -18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 40, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -40, 0, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 38, 18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -38, 18, ModContent.ProjectileType<morbinspikehostile>(), 40, 0f, Main.myPlayer, npc.whoAmI, 40);

                    fangTimer2 = 0;
                }

                #endregion

                #region Summon
                summonTimer++;

                if (summonTimer >= 300 && enraged == false)
                {
                    npc.netUpdate = true;
                    NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<morbi>());
                    summonTimer = 0;
                }

                if (summonTimer >= 120 && enraged == true && RetributionWorld.nightmareMode == false)
                {
                    npc.netUpdate = true;
                    NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<morbi>());
                    summonTimer = 0;
                }

                if (summonTimer >= 250 && enraged == true && RetributionWorld.nightmareMode == true)
                {
                    npc.netUpdate = true;
                    NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<morbi2>());
                    summonTimer = 0;
                }
                #endregion
        }

        public override void FindFrame(int frameHeight)
        {
            #region Idle Frames
            if (this.frame == frameIdle)
            {
                counting += 2.0;
                if (counting < 8.0)
                {
                    npc.frame.Y = 0;
                }
                else if (counting < 16.0)
                {
                    npc.frame.Y = frameHeight;
                }
                else if (counting < 24.0)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (counting < 32.0)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else if (counting < 40.0)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else
                {
                    counting = 0.0;
                }
            }

            if (this.frame == frameIdle && stateFire == true)
            {
                counting += 2.0;
                if (counting < 4.0)
                {
                    npc.frame.Y = 0;
                }
                else if (counting < 6.0)
                {
                    npc.frame.Y = frameHeight;
                }
                else if (counting < 8.0)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (counting < 10.0)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else if (counting < 12.0)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else
                {
                    counting = 0.0;
                }
            }
            #endregion

            #region Blink Frames
            if (this.frame == frameBlink)
            {
                counting += 2.0;
                if (counting < 10.0)
                {
                    npc.frame.Y = frameHeight * 5;
                }
                else if (counting < 20.0)
                {
                    npc.frame.Y = frameHeight * 6;
                }
                else if (counting < 30.0)
                {
                    npc.frame.Y = frameHeight * 7;
                }
                else if (counting < 40.0)
                {
                    npc.frame.Y = frameHeight * 8;
                }
                else if (counting < 50.0)
                {
                    npc.frame.Y = frameHeight * 9;
                }
                else
                {
                    counting = 0.0;
                    stateIdle = true;
                    stateBlink = false;
                }
            }
            #endregion

            #region Enraged Frames
            if (this.frame == frameEnraged)
            {
                counting += 2.0;
                if (counting < 10.0)
                {
                    npc.frame.Y = frameHeight * 10;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 20.0)
                {
                    npc.frame.Y = frameHeight * 11;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 30.0)
                {
                    npc.frame.Y = frameHeight * 12;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 40.0)
                {
                    npc.frame.Y = frameHeight * 13;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 50.0)
                {
                    npc.frame.Y = frameHeight * 14;
                    stateIdle = false;
                    stateBlink = false;
                }
                else
                {
                    counting = 0.0;
                    stateIdle = false;
                    stateBlink = false;
                }
            }

            if (this.frame == frameEnraged && stateFire2 == true)
            {
                counting += 2.0;
                if (counting < 4.0)
                {
                    npc.frame.Y = frameHeight * 10;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 6.0)
                {
                    npc.frame.Y = frameHeight * 11;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 8.0)
                {
                    npc.frame.Y = frameHeight * 12;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 10.0)
                {
                    npc.frame.Y = frameHeight * 13;
                    stateIdle = false;
                    stateBlink = false;
                }
                else if (counting < 12.0)
                {
                    npc.frame.Y = frameHeight * 14;
                    stateIdle = false;
                    stateBlink = false;
                }
                else
                {
                    counting = 0.0;
                    stateIdle = false;
                    stateBlink = false;
                }
            }
            #endregion
        }

        public override void NPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Morbus_Roar"));
            Main.PlaySound(SoundID.DD2_BetsyScream, npc.Center);

            if (RetributionWorld.downedMorbus == false)
            {
                Main.NewText("The Corruption's pneuma has been released from it's shackles...", 108, 92, 145, true);
                RetributionWorld.downedMorbus = true;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Morbus2_1"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Morbus2_2"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Morbus2_3"), 1f);
        }
    }
}