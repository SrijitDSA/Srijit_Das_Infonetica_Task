---

````markdown
# 🧩 Configurable Workflow Engine (State-Machine API)

A minimal backend API built with **.NET 8** that allows clients to define, run, and manage workflow instances as configurable state machines.

## 🚀 Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run the project
```bash
dotnet run
````

The API will be available at: `http://localhost:5000` (or another port shown in the console)

---

## 📬 API Endpoints

### 1. Create Workflow Definition

**POST** `/workflows`

```json
{
  "id": "workflow1",
  "name": "My First Workflow",
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false },
    { "id": "end", "name": "End", "isInitial": false, "isFinal": true }
  ],
  "actions": [
    { "id": "go", "name": "Go", "fromStates": ["start"], "toState": "end" }
  ]
}
```

---

### 2. Get Workflow Definition

**GET** `/workflows/{id}`

---

### 3. Start Workflow Instance

**POST** `/instances`

```json
{
  "definitionId": "workflow1"
}
```

---

### 4. Get Instance Status

**GET** `/instances/{id}`

---

### 5. Execute Action

**POST** `/instances/{id}/execute`

```json
{
  "actionId": "go"
}
```

---

## ✅ Features

* Create workflows with valid transitions
* Start instances at the initial state
* Enforce state machine rules (e.g. disallow invalid transitions, disabled actions, transitions from final state)
* Track action history with timestamps

---

## 📦 Persistence

This implementation uses **in-memory storage**. Data is lost on restart. Suitable for testing and demo.

---

## 📌 Assumptions & Notes

* All workflow state IDs must be unique.
* Each workflow must have exactly one `IsInitial = true` state.
* Transitions (`Action`) are only allowed if:

  * The current state is in `fromStates`
  * The target state exists
  * The action is enabled
* Final states cannot have outgoing transitions.
* The API uses ASP.NET Core Minimal API style for simplicity.

---

## 🔧 Extensibility

Designed with simplicity in mind, this codebase can be easily extended to support:

* Persistent storage (e.g. JSON/YAML files or a database)
* Action/State metadata
* Role-based transitions
* Workflow versioning

---

