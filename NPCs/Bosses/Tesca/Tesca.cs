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

namespace Retribution.NPCs.Bosses.Tesca
{
    [AutoloadBossHead]
    public class Tesca : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tesca");
            Main.npcFrameCount[npc.type] = 12;
            NPCID.Sets.TrailCacheLength[npc.type] = 5;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            if (RetributionWorld.nightmareMode == false)
            {
                npc.lifeMax = 6500;
            }
            if (RetributionWorld.nightmareMode == true)
            {
                npc.lifeMax = 7200;
            }

            if (RetributionWorld.nightmareMode == false)
            {
                npc.damage = 20;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                npc.damage = 35;
            }

            npc.defense = 6;
            npc.knockBackResist = 0f;
            npc.width = 202;
            npc.height = 242;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.alpha = 0;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Frozen] = true;
            npc.buffImmune[BuffID.Confused] = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/tesca");
            bossBag = ModContent.ItemType<Tesca_Bag>();
        }

        private double counting;

        public int frameIdle = 0;
        public int frameIceBolt = 1;
        public int frameEnraged = 2;

        private int frame;

        private bool enraged = false;
        private bool moving = true;
        private int soundAmount = 0;
        private int attachAmount = 0;

        private bool stateIdle = true;
        private bool stateIceBolt = false;

        private int boltTimer;
        private int attachTimer;
        private int dashTimer;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (enraged == true)
            {
                Vector2 vector = new Vector2(0f, 0f);
                SpriteEffects effects = (base.npc.spriteDirection < 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                for (int i = 0; i < base.npc.oldPos.Length; i++)
                {
                    Vector2 position = base.npc.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, npc.gfxOffY);
                    Color value = base.npc.GetAlpha(new Color(255, 255, 255, 10)) * ((float)(base.npc.oldPos.Length - i) / (float)base.npc.oldPos.Length);
                    spriteBatch.Draw(Main.npcTexture[base.npc.type], position, new Rectangle?(base.npc.frame), value * 0.25f, base.npc.rotation, vector, base.npc.scale, effects, 0f);
                }
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
                npc.alpha = 80;
            }

            if (enraged == true && soundAmount == 0)
            {
                Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Tesca_Screech"));

                soundAmount = 1;
            }
            #endregion

            #region Idle
            if (moving == true)
            {
                Vector2 vector = Main.player[base.npc.target].Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);
                Vector2 vector2 = base.npc.Center + new Vector2(base.npc.Center.X, base.npc.Center.Y);

                if (Main.player[base.npc.target].Center.X < base.npc.Center.X && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    NPC npc2 = base.npc;
                    npc2.velocity.X = npc2.velocity.X - ((base.npc.velocity.X > 0f) ? 0.85f : 0.07f);
                }
                if (Main.player[base.npc.target].Center.X > base.npc.Center.X && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    NPC npc3 = base.npc;
                    npc3.velocity.X = npc3.velocity.X + ((base.npc.velocity.X < 0f) ? 0.85f : 0.07f);
                }

                if (enraged == false && RetributionWorld.nightmareMode == false)
                {
                    if (Main.player[base.npc.target].position.Y < base.npc.position.Y + 350f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc2 = base.npc;
                        npc2.velocity.Y = npc2.velocity.Y - ((base.npc.velocity.Y > 0f) ? 0.85f : 0.07f);
                    }
                    if (Main.player[base.npc.target].position.Y > base.npc.position.Y + 350f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc3 = base.npc;
                        npc3.velocity.Y = npc3.velocity.Y + ((base.npc.velocity.Y < 0f) ? 0.85f : 0.07f);
                    }
                }

                if (enraged == false && RetributionWorld.nightmareMode == true)
                {
                    if (Main.player[base.npc.target].position.Y < base.npc.position.Y + 320f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc2 = base.npc;
                        npc2.velocity.Y = npc2.velocity.Y - ((base.npc.velocity.Y > 0f) ? 0.85f : 0.07f);
                    }
                    if (Main.player[base.npc.target].position.Y > base.npc.position.Y + 320f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc3 = base.npc;
                        npc3.velocity.Y = npc3.velocity.Y + ((base.npc.velocity.Y < 0f) ? 0.85f : 0.07f);
                    }
                }

                if (enraged == true && RetributionWorld.nightmareMode == false)
                {
                    if (Main.player[base.npc.target].position.Y < base.npc.position.Y + 350f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc2 = base.npc;
                        npc2.velocity.Y = npc2.velocity.Y - ((base.npc.velocity.Y > 0f) ? 0.85f : 0.07f);
                    }
                    if (Main.player[base.npc.target].position.Y > base.npc.position.Y + 350f && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        NPC npc3 = base.npc;
                        npc3.velocity.Y = npc3.velocity.Y + ((base.npc.velocity.Y < 0f) ? 0.85f : 0.07f);
                    }
                }

                if (enraged == true && RetributionWorld.nightmareMode == true)
                {
                    dashTimer++;

                    if (dashTimer >= 120 && Main.rand.NextFloat() < .50f)
                    {
                        npc.netUpdate = true;
                        if (Main.player[base.npc.target].position.Y < base.npc.position.Y - 100f && Main.netMode != 1)
                        {
                            NPC npc2 = base.npc;
                            npc2.velocity.Y = npc2.velocity.Y - ((base.npc.velocity.Y > 0f) ? 7f : 0.4f);
                            dashTimer = 0;
                            npc.netUpdate = true;
                        }
                    }
                    else if (dashTimer >= 121)
                    {
                        npc.netUpdate = true;
                        if (Main.player[base.npc.target].position.Y > base.npc.position.Y + 100f && Main.netMode != 1)
                        {
                            NPC npc3 = base.npc;
                            npc3.velocity.Y = npc3.velocity.Y + ((base.npc.velocity.Y < 0f) ? 7f : 0.4f);
                            dashTimer = 0;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            #endregion

            #region Ice Bolt
            npc.ai[2]++;

            if (enraged == false)
            {
                if (npc.ai[2] >= 500 && Main.rand.NextFloat() < .20f && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    npc.ai[3]++;

                    stateIceBolt = true;
                    stateIdle = false;

                    if (npc.ai[3] >= 2)
                    {

                        Main.PlaySound(SoundID.Item28, npc.position);

                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 55, 0, 6, ModContent.ProjectileType<tescanlaserspike>(), 20, 0f, Main.myPlayer, npc.whoAmI, 40);
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                }
            }

            if (enraged == true)
            {
                if (npc.ai[2] >= 300 && Main.rand.NextFloat() < .20f && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    npc.ai[3]++;

                    stateIceBolt = true;
                    stateIdle = false;

                    if (npc.ai[3] >= 2)
                    {

                        Main.PlaySound(SoundID.Item28, npc.position);

                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 55, 0, 8, ModContent.ProjectileType<tescanlaserspike>(), 20, 0f, Main.myPlayer, npc.whoAmI, 40);
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                }
            }
            #endregion

            #region Ice Bolt Homing
            boltTimer++;

            if (enraged == false)
            {
                if (boltTimer >= 180 && Main.rand.NextFloat() < .25f && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    Main.PlaySound(SoundID.Item28, npc.position);

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, -8, 0, ModContent.ProjectileType<TescanHome>(), 20, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, 8, 0, ModContent.ProjectileType<TescanHome>(), 20, 0f, Main.myPlayer, npc.whoAmI, 40);
                }
                if (boltTimer >= 181)
                {
                    boltTimer = 0;
                }
            }

            if (enraged == true)
            {
                if (boltTimer >= 150 && Main.rand.NextFloat() < .25f && Main.netMode != 1)
                {
                    npc.netUpdate = true;
                    Main.PlaySound(SoundID.Item28, npc.position);

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, -8, 8, ModContent.ProjectileType<TescanHome>(), 25, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, 8, 8, ModContent.ProjectileType<TescanHome>(), 25, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, -8, -8, ModContent.ProjectileType<TescanHome>(), 25, 0f, Main.myPlayer, npc.whoAmI, 40);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 55, 8, -8, ModContent.ProjectileType<TescanHome>(), 25, 0f, Main.myPlayer, npc.whoAmI, 40);
                }
                if (boltTimer >= 151)
                {
                    boltTimer = 0;
                }
            }
            #endregion

            #region Attach Shoot
            if (enraged == true)
            {
                attachTimer++;

                if (attachTimer >= 480 && attachAmount == 0)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 1;
                }

                if (attachTimer >= 490 && attachAmount == 1)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 2;
                }

                if (attachTimer >= 500 && attachAmount == 2)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 3;
                }

                if (attachTimer >= 510 && attachAmount == 3)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 4;
                }

                if (attachTimer >= 520 && attachAmount == 4)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 5;
                }

                if (attachTimer >= 530 && attachAmount == 5)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 6;
                }

                if (attachTimer >= 540 && attachAmount == 6)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 7;
                }

                if (attachTimer >= 550 && attachAmount == 7)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<tescanattachspawn>(), 0, 0f, Main.myPlayer, npc.whoAmI, 40);
                    attachAmount = 0;
                }

                if (attachTimer >= 550)
                {
                    attachTimer = 0;
                }
            }
            #endregion
        }

        public override void FindFrame(int frameHeight)
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
        }

        public override void NPCLoot()
        {
            Main.NewText("The essence of the Tundra has been released...", 111, 199, 214, true);
            Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Tesca_Dead"));

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tesca1"), 1f);
            Gore.NewGore(npc.position, -npc.velocity, mod.GetGoreSlot("Gores/Tesca0"), 1f);

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }

            RetributionWorld.downedTesca = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }
}