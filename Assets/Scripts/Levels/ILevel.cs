using Cysharp.Threading.Tasks;

namespace NoMorePals
{
    public interface ILevel
    {
        int LevelIndex { get; }
        UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB);
        int GetLevelTurns();
        void SetQuestComplete(string questID);
        bool AreAllQuestsComplete();
    }
}