namespace NoMorePals
{
    public class TableQuest : QuestTrigger
    {
        public override void Active()
        {
            base.Active();
            GameManager.Instance.SetQuestComplete(Constants.TableQuestID);
        }

        public override void Enter(MagnetBlock block)
        {
            base.Enter(block);
            GameManager.Instance.SetQuestComplete(Constants.GoToPcScene);
            block.EnterLevel1();
        }
    }
}