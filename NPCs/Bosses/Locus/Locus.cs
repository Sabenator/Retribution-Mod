using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Projectiles;

namespace Retribution.NPCs.Bosses.Locus
{
    public class Locus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Locus");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 0;
            if (RetributionWorld.nightmareMode == false)
            {
                npc.lifeMax = 7050;
            }
            if (RetributionWorld.nightmareMode == true)
            {
                npc.lifeMax = 9000;
            }

            if (RetributionWorld.nightmareMode == false)
            {
                npc.damage = 40;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                npc.damage = 50;
            }
            npc.defense = 16;
            npc.knockBackResist = 0f;
            npc.width = 92;
            npc.height = 89;
            npc.value = Item.buyPrice(0, 12, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.alpha = 0;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;

            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/locust");


            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }
        private double counting;
        private int frame;
        private int frameIdle = 0;
        private bool stateIdle = false;
        private bool stateEnraged = false;
        private bool stateEnraged2 = false;
        private int phaseCount = 0;
        private int bulletTimer;
        private int lightningTimer;
        private int summonTimer;
        private int bulleHellTimer;

		public int counter;
		public int flux;
		public bool shift;
		public static bool sideRight;
		public int shifted;
		public int angry;
        public int attackState;
        public bool moving = true;
		public bool strike;
		public bool charge;
		public bool charging;
        public bool npcTrail = false;

        public override void AI()
        {
            #region Animations
            if (stateIdle == true)
            {
                frame = frameIdle;
            }
            #endregion

            #region Idle Behavior

            if (moving == true)
            {
                npc.ai[0] += 1f;
                npc.ai[2] += 1f;
                npc.ai[3] += 1f;
                if (npc.aiStyle != -1)
                {
                    npc.ai[1] -= 1f;
                }
                npc.TargetClosest(true);
                Player player = Main.player[npc.target];
                Vector2 vector = Main.player[npc.target].Center + new Vector2(npc.Center.X, npc.Center.Y);
                Vector2 vector2 = npc.Center + new Vector2(npc.Center.X, npc.Center.Y);
                npc.netUpdate = true;

                if (npc.ai[0] == 80f)
                {
                    counter = 12;
                    strike = true;
                }
                if (npc.ai[2] >= 360f)
                {
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                    npc.ai[0] = 0f;
                    charge = true;
                    charging = true;
                    npcTrail = true;
                    for (int i = 0; i < 5; i++)
                    {
                        int num = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 16, -((float)npc.spriteDirection * 8f), 0f, 0, default(Color), 0.75f);
                        Dust dust = Main.dust[num];
                        Main.dust[num].velocity *= 0.2f;
                        Main.dust[num].noGravity = true;
                    }
                }
                if (npc.ai[2] >= 420f)
                {
                    npc.aiStyle = -1;
                    if (sideRight)
                    {
                        sideRight = false;
                    }
                    else
                    {
                        sideRight = true;
                    }
                    npcTrail = true;
                    charge = true;
                    charging = true;
                    Vector2 vector3 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num2 = (float)Math.Atan2((double)(vector3.Y - (player.Center.Y - 350f)), (double)(vector3.X - player.Center.X));
                    npc.velocity.X = (float)(Math.Cos((double)num2) * 14.0) * -1f;
                    npc.velocity.Y = (float)(Math.Sin((double)num2) * 14.0) * -1f;
                    npc.netUpdate = true;
                    npc.ai[2] = 0f;
                }

                if (player.Center.X > npc.Center.X)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                if (!shift)
                {
                    flux += 2;
                }
                else
                {
                    flux -= 2;
                }
                if (flux > 120 && !shift)
                {
                    shift = true;
                }
                if (flux <= -60)
                {
                    shift = false;
                }
                if (Main.player[npc.target].position.Y < npc.position.Y + 30f + (float)flux)
                {
                    NPC npc2 = npc;
                    npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.6f : 0.07f);
                }
                if (Main.player[npc.target].position.Y > npc.position.Y + 30f + (float)flux)
                {
                    NPC npc3 = npc;
                    npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.6f : 0.07f);
                }
                if (!charging)
                {
                    npc.aiStyle = 14;
                    if (sideRight)
                    {
                        npcTrail = false;
                        if (Main.player[npc.target].position.X < npc.position.X + 575f + (float)(flux / 2))
                        {
                            NPC npc4 = npc;
                            npc4.velocity.X = npc4.velocity.X - ((npc.velocity.X > 0f) ? 2f : 1f);
                        }
                        if (Main.player[npc.target].position.X > npc.position.X + 475f + (float)(flux / 2))
                        {
                            NPC npc5 = npc;
                            npc5.velocity.X = npc5.velocity.X + ((npc.velocity.X < 0f) ? 2f : 1f);
                            return;
                        }
                    }
                    else
                    {
                        npcTrail = false;
                        if (Main.player[npc.target].position.X > npc.position.X - 575f + (float)(flux / 2))
                        {
                            NPC npc6 = npc;
                            npc6.velocity.X = npc6.velocity.X + ((npc.velocity.X < 0f) ? 2f : 1f);
                        }
                        if (Main.player[npc.target].position.X < npc.position.X - 475f + (float)(flux / 2))
                        {
                            NPC npc7 = npc;
                            npc7.velocity.X = npc7.velocity.X - ((npc.velocity.X > 0f) ? 2f : 1f);
                            return;
                        }
                    }
                }
                else if (Main.player[npc.target].position.X < npc.position.X - 250f || Main.player[npc.target].position.X > npc.position.X + 250f)
                {
                    charging = false;
                }
            }
            else if (moving == false)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
            }
            #endregion

            #region Enraged
            if (npc.life < npc.lifeMax / 2 && phaseCount == 0)
            {
                stateEnraged = true;
                phaseCount = 1;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                if (npc.life < npc.lifeMax / 4 && phaseCount == 1)
                {
                    stateEnraged2 = true;
                    phaseCount = 2;
                }
            }
            #endregion

            #region Lightning Bolt
            lightningTimer++;
            if (lightningTimer >= 200 && Main.rand.NextFloat() < .15f && stateEnraged == true)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
                Vector2 vector = Main.player[npc.target].Center + new Vector2(npc.Center.X, npc.Center.Y);
                Vector2 vector2 = npc.Center + new Vector2(npc.Center.X, npc.Center.Y);
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
                float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Cos((double)num) * 2.0 * -1.0), (float)(Math.Sin((double)num) * 2.0 * -1.0), ProjectileID.CultistBossLightningOrb, 10, 0f, 0, 0f, 0f);

                lightningTimer = 0;
            }
            #endregion

            #region Summon
            summonTimer++;

            if (summonTimer >= 300 && stateEnraged == false)
            {
                NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<MiniLocus>());
                summonTimer = 0;
            }

            if (summonTimer >= 120 && stateEnraged == true)
            {
                NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<MiniLocus>());
                summonTimer = 0;
            }
            #endregion

            #region Cloud Bolts
            bulletTimer++;
            if (bulletTimer >= 180 && Main.rand.NextFloat() < .25f)
            {
                if (sideRight == true)
                {
                    stateIdle = false;

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), -2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), 0, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), 2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);

                    bulletTimer = 0;
                    stateIdle = true;
                }
                else
                {
                    stateIdle = false;

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), 2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), 0, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), -2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);

                    bulletTimer = 0;
                    stateIdle = true;
                }
            }

            if (bulletTimer >= 120 && Main.rand.NextFloat() < .25f && stateEnraged == true)
            {
                if (sideRight == true)
                {
                    stateIdle = false;

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), -2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), 0, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(4, 10), 2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);

                    bulletTimer = 0;
                    stateIdle = true;
                }
                else
                {
                    stateIdle = false;

                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), 2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), 0, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, -4), -2, ModContent.ProjectileType<LocusCloud>(), 20, 0f, Main.myPlayer, npc.whoAmI, 100);

                    bulletTimer = 0;
                    stateIdle = true;
                }
            }
            #endregion

            #region Bullet Hell
            if (stateEnraged2 == true)
            {
                bulleHellTimer++;

                if (bulleHellTimer > 300 && Main.rand.NextFloat() < .1f)
                {
                    moving = false;
                    npc.velocity.X = 0;
                    npc.velocity.Y = 0;

                    for (int i = 0; i < 20; i++)
                    {
                        int num = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 16, -((float)npc.spriteDirection * 8f), 0f, 0, default(Color), 0.75f);
                        Dust dust = Main.dust[num];
                        Main.dust[num].velocity *= 0.2f;
                        Main.dust[num].noGravity = true;
                    }

                    if (bulleHellTimer > 360)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10,10), Main.rand.Next(-10, 10), ModContent.ProjectileType<RainBolt>(), 10, 0f, Main.myPlayer, npc.whoAmI, 100);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10), ModContent.ProjectileType<RainBolt>(), 10, 0f, Main.myPlayer, npc.whoAmI, 100);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10), ModContent.ProjectileType<RainBolt>(), 10, 0f, Main.myPlayer, npc.whoAmI, 100);

                        if (bulleHellTimer > 540)
                        {
                            bulleHellTimer = 0;
                            moving = true;
                        }
                    }
                }
            }
            #endregion
        }
        public override void FindFrame(int frameHeight)
        {
            #region Idle
            if (frame == frameIdle)
            {
                counting += 1.5;
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
                else if (counting < 48.0)
                {
                    npc.frame.Y = frameHeight * 5;
                }
                else if (counting < 56.0)
                {
                    npc.frame.Y = frameHeight * 6;
                }
                else if (counting < 64.0)
                {
                    npc.frame.Y = frameHeight * 7;
                }
                else
                {
                    counting = 0.0;
                }
            }
            #endregion
        }

        public override void NPCLoot()
        {
            Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 31);

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus1"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus2"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus3"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus4"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus5"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus6"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Locus7"), 1f);
        }
    }
}
