using QLearning.MachineLearning;
using System.Collections.Generic;

namespace QLearning
{
    class RoomsProblem : IQLearningProblem
    {
        public int[,] odul;
        public int hedef;
        public int NumberOfStates => odul.GetLength(0);

        public int NumberOfActions => odul.GetLength(0);

        public double GetReward(int currentState, int action)
        {
            return odul[currentState,action];
        }

        public int[] GetValidActions(int currentState)
        {
            List<int> validActions = new List<int>();
            for (int i = 0; i < odul.GetLength(0); i++)
            {
                if (odul[currentState,i] != -1)
                    validActions.Add(i);
            }
            return validActions.ToArray();
        }

        public bool GoalStateIsReached(int currentState)
        {
            return currentState == hedef;
        }
    }
}
