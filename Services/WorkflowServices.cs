// File: Services/WorkflowService.cs
using WorkflowEngine.Models;
using WorkflowEngine.Store;

namespace WorkflowEngine.Services
{
    public static class WorkflowService
    {
        public static string CreateWorkflow(WorkflowDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Id) || InMemoryStore.Definitions.ContainsKey(definition.Id))
                throw new Exception("Invalid or duplicate workflow ID.");

            if (definition.States.Count(s => s.IsInitial) != 1)
                throw new Exception("Exactly one initial state is required.");

            var stateIds = new HashSet<string>(definition.States.Select(s => s.Id));
            if (stateIds.Count != definition.States.Count)
                throw new Exception("Duplicate state IDs found.");

            foreach (var action in definition.Actions)
            {
                if (!stateIds.Contains(action.ToState) || action.FromStates.Any(s => !stateIds.Contains(s)))
                    throw new Exception("Action refers to invalid state(s).");
            }

            InMemoryStore.Definitions[definition.Id] = definition;
            return definition.Id;
        }

        public static WorkflowDefinition? GetWorkflow(string id)
        {
            InMemoryStore.Definitions.TryGetValue(id, out var def);
            return def;
        }

        public static WorkflowInstance StartInstance(string definitionId)
        {
            if (!InMemoryStore.Definitions.TryGetValue(definitionId, out var def))
                throw new Exception("Workflow definition not found.");

            var initialState = def.States.First(s => s.IsInitial);
            var instance = new WorkflowInstance
            {
                DefinitionId = def.Id,
                CurrentStateId = initialState.Id
            };

            InMemoryStore.Instances[instance.Id] = instance;
            return instance;
        }

        public static WorkflowInstance? GetInstance(string instanceId)
        {
            InMemoryStore.Instances.TryGetValue(instanceId, out var inst);
            return inst;
        }

        public static WorkflowInstance ExecuteAction(string instanceId, string actionId)
        {
            if (!InMemoryStore.Instances.TryGetValue(instanceId, out var instance))
                throw new Exception("Instance not found.");

            var definition = InMemoryStore.Definitions[instance.DefinitionId];
            var action = definition.Actions.FirstOrDefault(a => a.Id == actionId && a.Enabled);
            if (action == null)
                throw new Exception("Action not found or not enabled.");

            var currentState = definition.States.First(s => s.Id == instance.CurrentStateId);
            if (currentState.IsFinal)
                throw new Exception("Cannot perform actions on a final state.");

            if (!action.FromStates.Contains(currentState.Id))
                throw new Exception("Action not valid from the current state.");

            instance.CurrentStateId = action.ToState;
            instance.History.Add(new TransitionRecord
            {
                ActionId = action.Id,
                Timestamp = DateTime.UtcNow
            });

            return instance;
        }
    }
}
