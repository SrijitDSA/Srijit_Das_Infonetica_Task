// File: Program.cs
using WorkflowEngine.Models;
using WorkflowEngine.Services;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/workflows", (WorkflowDefinition def) =>
{
    try
    {
        var id = WorkflowService.CreateWorkflow(def);
        return Results.Created($"/workflows/{id}", def);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// public record StartInstanceRequest(string DefinitionId);
// app.MapPost("/instances", (StartInstanceRequest body) =>
// {
//     try
//     {
//         var instance = WorkflowService.StartInstance(body.DefinitionId);
//         return Results.Created($"/instances/{instance.Id}", instance);
//     }
//     catch (Exception ex)
//     {
//         return Results.BadRequest(new { error = ex.Message });
//     }
// });


app.MapGet("/workflows/{id}", (string id) =>
{
    var def = WorkflowService.GetWorkflow(id);
    return def is not null ? Results.Ok(def) : Results.NotFound();
});

app.MapPost("/instances", (dynamic body) =>
{
    try
    {
        // Console.WriteLine(body);        
        // string defId = body.definitionId;
        string defId = body.GetProperty("definitionId").GetString();
        var instance = WorkflowService.StartInstance(defId);
        return Results.Created($"/instances/{instance.Id}", instance);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/instances/{id}", (string id) =>
{
    var inst = WorkflowService.GetInstance(id);

    return inst is not null ? Results.Ok(inst) : Results.NotFound();
});

app.MapPost("/instances/{id}/execute", (string id, dynamic body) =>
{
    try
    {
        string actionId = body.GetProperty("actionId").GetString();;
        var updated = WorkflowService.ExecuteAction(id, actionId);
        return Results.Ok(updated);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
