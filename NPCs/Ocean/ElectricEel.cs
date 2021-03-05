using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;

namespace Retribution.NPCs.Ocean
{
    public class ElectricEel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Eel");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 120;
            npc.damage = 50;
            npc.defense = 11;
            npc.width = 36;
            npc.height = 26;
            npc.value = Item.buyPrice(0, 0, 2, 0);
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.behindTiles = true;
        }

        public override void NPCLoot()
        {
            Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/preHM-eel-gore-01"));
            for (int i = 0; i < segmentCount; i++)
            {
                Gore.NewGore(segmentPos[i], npc.velocity, i == segmentCount - 1 ? mod.GetGoreSlot("Gores/preHM-eel-gore-03") : mod.GetGoreSlot("Gores/preHM-eel-gore-02"));
            }
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.active = false;
                    npc.life = -1;
                }
            }
            npc.noGravity = npc.wet;
            npc.velocity += Vector2.Normalize(Main.player[npc.target].Center - npc.Center) / 14;
            npc.velocity *= 0.98f;
            npc.rotation = npc.velocity.ToRotation() + (float)Math.PI;
        }

        static int segmentCount = 20;

        Vector2[] segmentPos = new Vector2[segmentCount];

        float[] segmentRot = new float[segmentCount];

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2 = mod.GetTexture("NPCs/Ocean/ElectricEel");
            spriteBatch.Draw(texture2, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), npc.scale, npc.velocity.X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            for (int i = 0; i < segmentCount; i++)
            {
                if (i == segmentCount - 1)
                    texture2 = mod.GetTexture("NPCs/Ocean/ElectricEel_Tail");
                else
                    texture2 = mod.GetTexture("NPCs/Ocean/ElectricEel_Body");
                Vector2 previousSegment;
                float previousRot;
                if (i != 0)
                {
                    previousSegment = segmentPos[i - 1];
                    previousRot = segmentRot[i - 1];
                }
                else
                {
                    previousSegment = npc.Center;
                    previousRot = npc.rotation - (float)Math.PI;
                }
                int gap = 20;
                gap = (int)(gap * npc.scale);
                if (!Main.gamePaused)
                {
                    segmentPos[i] += Vector2.Normalize(previousSegment - previousRot.ToRotationVector2() * gap * 2 - segmentPos[i]) * 2;
                    segmentPos[i] = -(Vector2.Normalize(previousSegment - segmentPos[i]) * gap) + previousSegment;
                    segmentRot[i] = (previousSegment - segmentPos[i]).ToRotation();
                }
                spriteBatch.Draw(texture2, segmentPos[i] - Main.screenPosition, null, Lighting.GetColor((int)segmentPos[i].X / 16, (int)segmentPos[i].Y / 16), segmentRot[i] + (float)Math.PI, new Vector2(texture2.Width / 2, texture2.Height / 2), npc.scale, npc.velocity.X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
