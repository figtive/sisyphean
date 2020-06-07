using System.Collections;
using System.Collections.Generic;

public enum TrashType { Small, Medium, Large };

public class TrashImpact {

    public static TrashImpact identity = new TrashImpact(null);

    public readonly string name;
    public readonly float speedMul;
    public readonly float maxRadiusMul;

    public TrashImpact(TrashType? type) {
        switch (type) {
            case TrashType.Small:
            name = "S";
            speedMul = 0.8f;
            maxRadiusMul = 0.9f;
            break;
            case TrashType.Medium:
            name = "M";
            speedMul = 0.6f;
            maxRadiusMul = 0.75f;
            break;
            case TrashType.Large:
            name = "L";
            speedMul = 0.4f;
            maxRadiusMul = 0.5f;
            break;
            default:
            name = "";
            speedMul = 1f;
            maxRadiusMul = 1f;
            break;

        }
    }
    public override string ToString() {
        return name;
    }

}

public enum GameDifficulty { Easy, Medium, Hard };

public enum EndState { Win, Lose };

public static class GamePoint {

    public static class Score {
        public static readonly int initial          = 0;
        public static readonly int npcBonus         = 10;
        public static readonly int passBonus        = 5;
        public static readonly int juggleBonus      = 5;
        public static readonly int npcNeutralize    = 50;
        public static int GetScore(TrashType type) {
            switch (type) {
                case TrashType.Small:   return 10;
                case TrashType.Medium:  return 20;
                case TrashType.Large:   return 30;
                default: return 0;
            }
        }
        public static int GetPenalty(TrashType type) {
            switch (type) {
                case TrashType.Small:   return -5;
                case TrashType.Medium:  return -10;
                case TrashType.Large:   return -15;
                default: return 0;
            }
        }
    }

    public static class Time {
        // public static readonly float initial        = 60f;
        public static readonly float npcBonus       = 0f;
        public static readonly float npcNeutralize  = 20f;
        public static readonly float dropPenalty    = 0f;
        // public static readonly float maximum        = 60f;
        public static float GetTime(TrashType type) {
            switch (type) {
                case TrashType.Small:   return 5f;
                case TrashType.Medium:  return 10f;
                case TrashType.Large:   return 15f;
                default: return 0f;
            }
        }
    }

    public static class Charisma {
        public static readonly float initial        = 0.2f;
        public static readonly float marginInitial  = 0.5f;
        public static readonly float fixedChance    = 0.25f;
        public static readonly float marginStep     = 1.15f;
        public static readonly float npcBonus       = 0f;
        public static readonly float npcNeutralize  = 0f;
        public static readonly float dropPenalty    = 0f;
        public static float GetCharisma(TrashType type) {
            switch (type) {
                case TrashType.Small:   return 0.05f;
                case TrashType.Medium:  return 0.10f;
                case TrashType.Large:   return 0.15f;
                default: return 0f;
            }
        }
        public static float GetPenalty(TrashType type) {
            switch (type) {
                case TrashType.Small:   return -0.05f;
                case TrashType.Medium:  return -0.10f;
                case TrashType.Large:   return -0.15f;
                default: return 0;
            }
        }
    }

}
