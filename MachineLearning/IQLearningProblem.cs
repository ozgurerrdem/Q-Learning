namespace QLearning.MachineLearning
{
    public interface IQLearningProblem
    {
        int NumberOfStates { get; }
        int NumberOfActions { get; }
        int[] GetValidActions(int currentState);
        double GetReward(int currentState, int action);
        bool GoalStateIsReached(int currentState);
    }
}
