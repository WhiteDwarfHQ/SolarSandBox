using System.Text.Json;
using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;
using SolarSandBox.UI.Services;

namespace SolarSandBox.UI.Tests;

public sealed class SolarProjectDocumentSerializationTests
{
    [Fact]
    public void SerializeDeserialize_PreservesEquipmentAndConnections()
    {
        var project = new StartupProject
        {
            Id = "project-serialization",
            Name = "Serialization Project",
            Kind = StartupProjectKind.DemoSystem,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        var document = new SolarProjectDocument
        {
            Id = project.Id,
            SchemaVersion = 2,
            AppVersion = "0.1.0",
            Project = project,
            Nodes =
            [
                new ProjectNodePlaceholder { Id = "node-1", Kind = "inverter", Label = "Inv", X = 10, Y = 20 }
            ],
            EquipmentDefinitions = GenericEquipmentCatalog.CreateStarterDefinitions(),
            EquipmentInstances =
            [
                new EquipmentInstance { Id = "inst-1", DefinitionId = "def-inverter-generic", DisplayName = "Inv-1" }
            ],
            ElectricalConnections =
            [
                new ElectricalConnection
                {
                    Id = "conn-1",
                    From = new EquipmentPortReference { EquipmentInstanceId = "inst-1", PortId = "ac-out" },
                    To = new EquipmentPortReference { EquipmentInstanceId = "inst-1", PortId = "ac-out" },
                    IsVerified = false,
                    VerificationNotes = "Test"
                }
            ]
        };

        var json = JsonSerializer.Serialize(document);
        var roundTrip = JsonSerializer.Deserialize<SolarProjectDocument>(json);

        Assert.NotNull(roundTrip);
        Assert.Equal(2, roundTrip!.SchemaVersion);
        Assert.Single(roundTrip.Nodes);
        Assert.NotEmpty(roundTrip.EquipmentDefinitions);
        Assert.Single(roundTrip.EquipmentInstances);
        Assert.Single(roundTrip.ElectricalConnections);
    }

    [Fact]
    public void Deserialize_Schema1Document_BackfillsNewCollections()
    {
        const string legacyJson = """
            {
              "id": "legacy-1",
              "schemaVersion": 1,
              "appVersion": "0.1.0",
              "project": {
                "id": "legacy-1",
                "name": "Legacy",
                "kind": 0,
                "createdAtUtc": "2026-10-30T00:00:00+00:00",
                "updatedAtUtc": "2026-10-30T00:00:00+00:00"
              },
              "nodes": [
                { "id": "n1", "kind": "inverter", "label": "Inv", "x": 1, "y": 2 }
              ]
            }
            """;

        var document = JsonSerializer.Deserialize<SolarProjectDocument>(legacyJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(document);
        Assert.Equal(1, document!.SchemaVersion);
        Assert.Single(document.Nodes);
        Assert.NotNull(document.EquipmentDefinitions);
        Assert.NotNull(document.EquipmentInstances);
        Assert.NotNull(document.ElectricalConnections);
        Assert.Empty(document.EquipmentDefinitions);
        Assert.Empty(document.EquipmentInstances);
        Assert.Empty(document.ElectricalConnections);
    }
}
