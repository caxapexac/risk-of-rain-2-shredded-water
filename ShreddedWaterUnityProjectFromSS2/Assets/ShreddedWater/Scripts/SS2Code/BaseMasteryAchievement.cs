using RoR2;
using RoR2.Achievements;

namespace Moonstorm.Starstorm2
{
    public abstract class BaseMasteryAchievement : BaseAchievement
    {
        public BaseMasteryAchievement(float requiredDifficultyCoefficient, CharacterBody requiredCharacterBody)
        {
            RequiredDifficultyCoefficient = requiredDifficultyCoefficient;
            RequiredCharacterBody = requiredCharacterBody;
        }

        public BaseMasteryAchievement()
        {

        }

        public virtual float RequiredDifficultyCoefficient { get; set; }
        public virtual CharacterBody RequiredCharacterBody { get; set; }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += OnClientGameOverGlobal;
        }
        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }
        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if ((bool)runReport.gameEnding && runReport.gameEnding.isWin)
            {
                DifficultyIndex difficultyIndex = runReport.ruleBook.FindDifficulty();
                DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(difficultyIndex);
                
                if ((runDifficulty.countsAsHardMode && runDifficulty.scalingValue >= RequiredDifficultyCoefficient) || //check our required difficulty coefficient
                    (difficultyIndex >= DifficultyIndex.Eclipse1 && difficultyIndex <= DifficultyIndex.Eclipse8) || //check for eclipse (to be consistent with other grand masteries)
                    (runDifficulty.nameToken == "INFERNO_NAME")) //check for inferno mod, as it starts at monsoon coefficient but scales higher
                {
                    Grant();
                }
            }
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return RequiredCharacterBody.bodyIndex;
        }
    }
}
