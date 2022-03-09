using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BenchMark
{
    internal class Program
    {
        public static void Main()
        {
            Summary summary = BenchmarkRunner.Run<DictionaryTest>();
        }
    }

    [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net60)]
    public class DictionaryTest
    {
        private static readonly Dictionary<Type, List<PropertyInfo>> StringPropertiesDictionary = new();

        [Benchmark]
        public List<PropertyInfo> NoDictionary()
        {
            return typeof(Entity).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();
        }

        [Benchmark]
        public List<PropertyInfo> WithDictionary()
        {
            return GetStringProperties(typeof(Entity));
        }

        private static List<PropertyInfo> GetStringProperties(Type entityType)
        {
            if (StringPropertiesDictionary.ContainsKey(entityType))
            {
                return StringPropertiesDictionary[entityType];
            }

            var stringProperties = entityType.GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();

            StringPropertiesDictionary.Add(entityType, stringProperties);

            return stringProperties;
        }
    }

    public class Entity
    {
        public string EntityName { get; set; } = string.Empty;

        public int Id { get; set; }

        public string LEI { get; set; } = string.Empty;

        public string MDM { get; set; } = string.Empty;

        public bool ParticipationTier1 { get; set; }

        public bool ParticipationTier2 { get; set; }

        public double? Tier1Fee { get; set; }

        public double? Tier2Fee { get; set; }

        public decimal? Tier2Limit { get; set; }
    }
}