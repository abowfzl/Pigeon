namespace Pigeon.Algoritms;

public struct TrainingSample
{
    public int Observation { get; private set; }

    public List<String> Products { get; private set; }

    public TrainingSample(int observation, List<String> products)
    {
        Observation = observation;
        Products = products;
    }
}
