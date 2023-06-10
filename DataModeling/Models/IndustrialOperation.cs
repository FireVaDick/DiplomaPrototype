using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;

namespace DataModeling.Models
{
    public class IndustrialOperation : ObservableObject
    {
        public uint Chain { get; private set; }

        public uint Number { get; private set; }

        public string Name { get; set; } = string.Empty;

        public ISeries[] InputResources { get; set; }

        public ISeries[] Machines { get; set; }

        public ISeries[] OutputResource { get; set; }

        public IndustrialOperation(
            uint chain,
            uint number,
            string name,
            ISeries[] inputResources,
            ISeries[] machine,
            ISeries[] outputResource)
        {
            Chain = chain;
            Number = number;
            Name = name;
            InputResources = inputResources;
            Machines = machine;
            OutputResource = outputResource;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
