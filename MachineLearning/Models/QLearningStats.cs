using System.Text;

namespace QLearning.MachineLearning
{
    public class QLearningStats
    {
        public int InitialState { get; set; }
        public int EndState { get; set; }
        public int Steps { get; set; }
        public int[] Actions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Agent needed {Steps} steps to find the solution");
            sb.AppendLine($"Agent Initial State: {InitialState}");
            foreach (var action in Actions)
                sb.AppendLine($"Action: {action}");
            sb.AppendLine($"Agent arrived at the goal state: {EndState}");
            return sb.ToString();
        }
    }
}
