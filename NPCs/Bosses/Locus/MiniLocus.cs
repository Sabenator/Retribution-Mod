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
    public class MiniLocus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hopper");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 14;
            npc.lifeMax = 50;
            npc.damage = 25;
            npc.defense = 16;
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 22;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 300;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);

            const int NUM_DUSTS = 50;

            for (int i = 0; i < NUM_DUSTS; i++)
            {
                Dust dust;
                Vector2 position = npc.position;
                dust = Terraria.Dust.NewDustDirect(position, 45, 45, 75, 0f, 0f, 0, new Color(255, 226, 0), 1f);
            }

            npc.active = false;
        }
    }
}