using UnityEngine;
using UnityEditor;

namespace COSMOS.Skills
{
    public enum Relation
    {
        Friend,
        Neutral,
        Enemy
    }
    public interface ISkillCountdown
    {
        public float TimeLeft { get; }
        public float Countdown { get; }
    }
    public interface ISkillOnOff
    {
        public bool State { get; }
    }
    public interface ISkillTarget
    {
        public int TargetCount { get; }
        public Relation TargetRelation { get; }
        public bool Self { get; }
    }
}