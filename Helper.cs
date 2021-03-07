using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Retribution.Buffs;
using Terraria.ID;
using System.Linq;
using Terraria.Localization;

namespace Retribution
{
    #region RecipeGroupValidators
    //validates recipe groups. if they are incorrect, it replaces it with an iron bar, and the recipe group it titled "Recipe Group Error in <name of recipe group>"
    public class rg
    {
        public bool valid = false;
        public RecipeGroup _RecipeGroup = null;

        public rg(Mod m, String Name, String a1, String a2)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error in " + Name, new int[] { ItemID.IronBar });
            }

        }
        public rg(Mod m, String Name, String a1, String a2, String a3)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }
        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }
        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }
        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5, String a6)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5), m.ItemType(a6) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }
        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5, String a6, String a7)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5), m.ItemType(a6), m.ItemType(a7) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }
        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5, String a6, String a7, String a8)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5), m.ItemType(a6), m.ItemType(a7), m.ItemType(a8) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }

        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5, String a6, String a7, String a8, String a9)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5), m.ItemType(a6), m.ItemType(a7), m.ItemType(a8), m.ItemType(a9) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }

        }
        public rg(Mod m, String Name, String a1, String a2, String a3, String a4, String a5, String a6, String a7, String a8, String a9, String a10)
        {

            int[] arr = { m.ItemType(a1), m.ItemType(a2), m.ItemType(a3), m.ItemType(a4), m.ItemType(a5), m.ItemType(a6), m.ItemType(a7), m.ItemType(a8), m.ItemType(a9), m.ItemType(a10) };
            if (arr.Contains(0) == false)
            {
                _RecipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Name + " Weapon", arr);
            }
            else
            {
                _RecipeGroup = new RecipeGroup(() => "Recipe Group Error", new int[] { ItemID.IronBar });
            }

        }
    }
    #endregion
    //returns iron bar if item isn't valid to prevent excessive reloading of the mod when an item is incorrect
    public class ItemCheck
    {
        public int b;
        public ItemCheck(int a)
        {
            b = a;
        }
        public int Validate()
        {
            int i = b;
            if (i == 0)
            {
                return (ItemID.IronBar);
            }
            else
            {
                return b;
            }
        }
    }
    
    public class AIMethods
    {
        //NPC Follow
        public static void MoveToward(NPC npc, Vector2 playerTarget, float speed, float turnResistance)
        {
            Vector2 move = playerTarget - npc.Center;
            float length = move.Length();
            if (length > speed)
            {
                move *= speed / length;
            }
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            length = move.Length();
            if (length > speed)
            {
                move *= speed / length;
            }
            npc.velocity = move;
        }
        //Projectile Follow
        public static void MoveTowardProj(Projectile projectile, Vector2 playerTarget, float speed, float turnResistance)
        {
            Vector2 move = playerTarget - projectile.Center;
            float length = move.Length();
            if (length > speed)
            {
                move *= speed / length;
            }
            move = (projectile.velocity * turnResistance + move) / (turnResistance + 1f);
            length = move.Length();
            if (length > speed)
            {
                move *= speed / length;
            }
            projectile.velocity = move;
        }
        //NPC dash
        //By Target
        public static void DashToward(Vector2 npcStart, Vector2 playerTarget, float speed, NPC npc)
        {
            Vector2 move = playerTarget - npcStart;
            if (move.Length() > speed)
            {
                move *= speed / move.Length();
            }
            npc.velocity = move;
        }
        //By Angle
        public static void DashToward(Vector2 npcStart, float angle, float speed, NPC npc)
        {
            Vector2 playerTarget = new Vector2(speed, 0).RotatedBy(angle);
            Vector2 move = (playerTarget * 20) - npcStart;
            if (move.Length() > speed)
            {
                move *= speed / move.Length();
            }
            npc.velocity = move;
        }
        //Projectile "Dash"
        //By Target
        public static void DashTowardProj(Vector2 projStart, Vector2 playerTarget, float speed, Projectile projectile)
        {
            Vector2 move = playerTarget - projStart;
            if (move.Length() > speed)
            {
                move *= speed / move.Length();
            }
            projectile.velocity = move;
        }
        //By Angle
        public static void DashTowardProj(Vector2 projStart, float angle, float speed, Projectile projectile)
        {
            Vector2 target = new Vector2(speed, 0).RotatedBy(angle);
            Vector2 move = (target * 20) - projStart;
            if (move.Length() > speed)
            {
                move *= speed / move.Length();
            }
            projectile.velocity = move;
        }
        //Shoots in a ring
        public static void ShootRing(int count, int type, float velocity, float kb, int damage, Vector2 pos)
        {
            for (int i = 0; i < count; i++)
            {
                Projectile.NewProjectile(pos, new Vector2(velocity, 0).RotatedBy(MathHelper.ToRadians((360 / count) * i)), type, damage, kb);
            }
        }
        //Shoots in a ring, with rotation, in DEGREES
        public static void ShootRing(int count, int type, float velocity, float kb, int damage, Vector2 pos, int initRotationDeg)
        {
            for (int i = 0; i < count; i++)
            {
                Projectile.NewProjectile(pos, new Vector2(velocity, 0).RotatedBy(MathHelper.ToRadians(initRotationDeg)).RotatedBy(MathHelper.ToRadians((360 / count) * i)), type, damage, kb);
            }
        }
        //Shoots in a ring, with rotation, in DEGREES, And allows you to pass in ai[]
        public static void ShootRing(int count, int type, float velocity, float kb, int damage, Vector2 pos, int initRotationDeg, float ai0, float ai1)
        {
            for (int i = 0; i < count; i++)
            {
                Projectile.NewProjectile(pos, new Vector2(velocity, 0).RotatedBy(MathHelper.ToRadians(initRotationDeg)).RotatedBy(MathHelper.ToRadians((360 / count) * i)), type, damage, kb, 255, ai0, ai1);
            }
        }
        //Shoots at a player
        public static void ShootAt(Player player, Vector2 pos, int type, float velocity, float kb, int damage, int inaccuracy)
        {
            Vector2 angle = player.Center - pos;
            angle.Normalize();
            int rotation = Main.rand.Next(-inaccuracy, inaccuracy);
            angle.RotatedBy(MathHelper.ToRadians(rotation));
            Projectile.NewProjectile(pos, velocity * angle, type, damage, kb);
        }
        //Shoots at a player, with ai[]
        public static void ShootAt(Player player, Vector2 pos, int type, float velocity, float kb, int damage, int inaccuracy, float ai0, float ai1)
        {
            Vector2 angle = player.Center - pos;
            angle.Normalize();
            int rotation = Main.rand.Next(-inaccuracy, inaccuracy);
            angle.RotatedBy(MathHelper.ToRadians(rotation));
            Projectile.NewProjectile(pos, velocity * angle, type, damage, kb, 255, ai0, ai1);
        }
        //Shoots at a target
        public static void ShootAt(Vector2 target, Vector2 pos, int type, float velocity, float kb, int damage, int inaccuracy)
        {
            Vector2 angle = target - pos;
            angle.Normalize();
            int rotation = Main.rand.Next(-inaccuracy, inaccuracy);
            angle.RotatedBy(MathHelper.ToRadians(rotation));
            Projectile.NewProjectile(pos, velocity * angle, type, damage, kb);
        }
        //Shoots at a target, with ai[]
        public static void ShootAt(Vector2 target, Vector2 pos, int type, float velocity, float kb, int damage, int inaccuracy, float ai0, float ai1)
        {
            Vector2 angle = target - pos;
            angle.Normalize();
            int rotation = Main.rand.Next(-inaccuracy, inaccuracy);
            angle.RotatedBy(MathHelper.ToRadians(rotation));
            Projectile.NewProjectile(pos, velocity * angle, type, damage, kb, 255, ai0, ai1);
        }
        //projectile rain. direction is used to determine which way it is coming from. 0 = top, 1 = bottom, 2 = right, 3 = left
        //no ai[] passed in
        public static void Rain(int count, int type, float velocity, float kb, int damage, Vector2 playerPos, int direction)
        {
            if (direction == 0)
            {
                int spacing = Main.screenWidth / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X + (spacing * i), pos.Y), new Vector2(0, velocity), type, damage, kb);
                }
            }
            else if (direction == 1)
            {
                int spacing = Main.screenWidth / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y + (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X + (spacing * i), pos.Y), new Vector2(0, -velocity), type, damage, kb);
                }
            }
            else if (direction == 2)
            {
                int spacing = Main.screenHeight / count;
                Vector2 pos = new Vector2(playerPos.X + (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X, pos.Y + (spacing * i)), new Vector2(-velocity, 0), type, damage, kb);
                }
            }
            else if (direction == 3)
            {
                int spacing = Main.screenHeight / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X, pos.Y + (spacing * i)), new Vector2(velocity, 0), type, damage, kb);
                }
            }

        }
        //ai[] passed in
        public static void Rain(int count, int type, float velocity, float kb, int damage, Vector2 playerPos, int direction, float ai0, float ai1) {
            if (direction == 0)
            {
                int spacing = Main.screenWidth / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X + (spacing * i), pos.Y), new Vector2(0, velocity), type, damage, kb, 255, ai0, ai1);
                }
            }
            else if(direction == 1){
                int spacing = Main.screenWidth / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y + (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X + (spacing * i), pos.Y), new Vector2(0, -velocity), type, damage, kb, 255, ai0, ai1);
                }
            }
            else if (direction == 2)
            {
                int spacing = Main.screenHeight / count;
                Vector2 pos = new Vector2(playerPos.X + (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X, pos.Y + (spacing * i)), new Vector2(-velocity, 0), type, damage, kb, 255, ai0, ai1);
                }
            }
            else if (direction == 3)
            {
                int spacing = Main.screenHeight / count;
                Vector2 pos = new Vector2(playerPos.X - (Main.screenWidth / 1.75f), playerPos.Y - (Main.screenHeight / 1.75f));
                for (int i = 0; i < count + 1; i++)
                {
                    Projectile.NewProjectile(new Vector2(pos.X, pos.Y + (spacing * i)), new Vector2(velocity, 0), type, damage, kb, 255, ai0, ai1);
                }
            }

        }
        //shoots out a fan, may or may not work properly... use at your own risk

        public static void ShootFan(int count, int type, float velocity, float kb, int damage, Vector2 target, float anglescope, Vector2 pos, float ai0, float ai1) {
            Vector2 primaryProj = new Vector2(velocity, 0).RotatedBy((target - pos).ToRotation() - MathHelper.ToRadians(anglescope / 2));
            float rot = anglescope / count;
            for (int i = 0; i < count; i++) {
                Projectile.NewProjectile(pos, primaryProj.RotatedBy(MathHelper.ToRadians(rot * i)), type, damage, kb, 255, ai0, ai1);
            }
        
        }
        public static void ShootFan(int count, int type, float velocity, float kb, int damage, Vector2 target, float anglescope, Vector2 pos)
        {
            Vector2 primaryProj = new Vector2(velocity, 0).RotatedBy((target - pos).ToRotation() - MathHelper.ToRadians(anglescope / 2));
            float rot = anglescope / count;
            for (int i = 0; i < count; i++)
            {
                Projectile.NewProjectile(pos, primaryProj.RotatedBy(MathHelper.ToRadians(rot * i)), type, damage, kb, 255, 0, 0);
            }

        }
    }
}
