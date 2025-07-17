// File: Models/WorkflowInstance.cs
namespace WorkflowEngine.Models
{
    public class WorkflowInstance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DefinitionId { get; set; } = string.Empty;
        public string CurrentStateId { get; set; } = string.Empty;
        public List<TransitionRecord> History { get; set; } = new();
    }

    public class TransitionRecord
    {
        public string ActionId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}