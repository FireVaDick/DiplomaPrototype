using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace DataModeling.Models
{
    public class IndustrialChain : ObservableObject
    {
        public uint Number { get; }

        public string Name { get; }

        public ObservableCollection<IndustrialOperation> Operations { get; }

        public IndustrialChain(
            uint number,
            string name,
            ObservableCollection<IndustrialOperation> operations)
        {
            Number = number;
            Name = name;
            Operations = operations;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
