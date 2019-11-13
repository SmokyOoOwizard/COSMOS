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
        float TimeLeft { get; }
        float Countdown { get; }
    }
    public interface ISkillOnOff
    {
        bool State { get; }
    }
    public interface ISkillTarget
    {
        int TargetCount { get; }
        Relation TargetRelation { get; }
        bool Self { get; }
    }
    public interface ISkillDistanceTarget
    {
        float Distance { get; }
    }
    public interface ISkillZoneTarget
    {
        float Radius { get; }
    }
}