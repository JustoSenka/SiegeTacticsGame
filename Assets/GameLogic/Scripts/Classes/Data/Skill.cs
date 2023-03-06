using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Skill
    {
        public int ID;
        public string Name;

        public int MagicReq;

        public int Cooldown;
        public int CooldownLeft;

        public int Duration;
        public int DurationLeft;

        public int AOE;

        public int RangeMin;
        public int RangeMax;

        public List<SkillEffect> Effects;
    }

    [Flags]
    public enum SkillType
    {
        None = 0,
        Attack = 1,
        Guard = 1 << 1,
        Heal = 1 << 2,

    }

    public static class SkillTypeIDs
    {
        public static Dictionary<int, SkillType> IdToSkillType = new Dictionary<int, SkillType>
        {
            { 0, SkillType.None },
            { 1, SkillType.Attack },
            { 2, SkillType.Guard },
            { 3, SkillType.Heal },

        };

        public static Dictionary<SkillType, int> SkillTypeToId = new Dictionary<SkillType, int>
        {
            { SkillType.None, 0 },
            { SkillType.Attack, 1 },
            { SkillType.Guard, 2 },
            { SkillType.Heal, 3 },

        };
    }

    [Serializable]
    public struct SkillEffect
    {
        public int ID;

        public int Amount;
        public bool ShouldSet;
        public bool IsPermament;
        public PropertyName PropertyName;
    }

    public enum PropertyName
    {
        Attack,
        Defense,

        Health,
        Magic,

        Range,
        Movement,
    }

#if UNITY_EDITOR
    public static class SkillTypeExtension
    {
        public static SkillType SkillTypeFromIDs(params int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return default;

            return ids.Select(id => SkillTypeIDs.IdToSkillType[id]).Aggregate((a, b) => a | b);
        }

        public static int[] IDsFromSkillType(this SkillType skillType)
        {
            return Enum.GetValues(typeof(SkillType)).Cast<SkillType>()
                .Where(f => skillType.HasFlag(f))
                .Select(s => SkillTypeIDs.SkillTypeToId[s])
                .ToArray();
        }
    }
#endif
}
