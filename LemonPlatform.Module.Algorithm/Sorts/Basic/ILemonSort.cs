using LemonPlatform.Module.Algorithm.Models;
using LiveChartsCore;

namespace LemonPlatform.Module.Algorithm.Sorts.Basic
{
    public interface ILemonSort
    {
        ISeries[] GenerateSeries(int count, int bound, int delay, GenerationDataType generationDataType = GenerationDataType.Random);

        void UpdateDelay(int delay);

        Task RunAsync();
    }
}