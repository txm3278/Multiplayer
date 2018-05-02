﻿using Harmony;
using Multiplayer.Common;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Multiplayer.Client
{
    public static class Extensions
    {
        public static IEnumerable<Type> AllSubtypesAndSelf(this Type t)
        {
            return t.AllSubclasses().Concat(t);
        }

        public static IEnumerable<Type> AllImplementing(this Type t)
        {
            return from x in GenTypes.AllTypes where t.IsAssignableFrom(x) select x;
        }

        // sets the current Faction.OfPlayer
        // applies faction's map components if map not null
        public static void PushFaction(this Map map, Faction faction)
        {
            FactionContext.Push(faction);
            if (map != null && faction != null)
                map.GetComponent<MultiplayerMapComp>().SetFaction(faction);
        }

        public static void PushFaction(this Map map, string factionId)
        {
            Faction faction = Find.FactionManager.AllFactions.FirstOrDefault(f => f.GetUniqueLoadID() == factionId);
            map.PushFaction(faction);
        }

        public static void PopFaction(this Container<Map> c)
        {
            PopFaction(c.Value);
        }

        public static void PopFaction(this Map map)
        {
            Faction faction = FactionContext.Pop();
            if (map != null && faction != null)
                map.GetComponent<MultiplayerMapComp>().SetFaction(faction);
        }

        public static Map GetMap(this ScheduledCommand cmd)
        {
            return Find.Maps.FirstOrDefault(map => map.uniqueID == cmd.mapId);
        }
    }
}