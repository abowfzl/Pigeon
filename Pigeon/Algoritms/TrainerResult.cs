namespace Pigeon.Algoritms
{
    public class TrainerResult
    {
        public IList<string> Ins { get; set; } = new List<string>();

        public IList<string> Outs { get; set; } = new List<string>();

        public double Percentage { get; set; }
    }
}
