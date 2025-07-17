// File: Store/InMemoryStore.cs
using WorkflowEngine.Models;

namespace WorkflowEngine.Store
{
    public static class InMemoryStore
    {
        public static Dictionary<string, WorkflowDefinition> Definitions { get; set; } = new();
        public static Dictionary<string, WorkflowInstance> Instances { get; set; } = new();
    }
}