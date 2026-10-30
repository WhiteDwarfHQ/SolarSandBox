using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Services;

namespace SolarSandBox.UI.Tests;

public sealed class ConnectionValidationServiceTests
{
    private readonly ConnectionValidationService _service = new();

    [Fact]
    public void Validate_KnownCompatibleBatteryPorts_AllowsConnection()
    {
        var definitions = GenericEquipmentCatalog.CreateStarterDefinitions();
        var instances = CreateDemoInstances();

        var candidate = new ElectricalConnection
        {
            Id = "conn-ok",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-inverter",
                PortId = "battery-dc-inout"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-battery",
                PortId = "battery-dc-port"
            },
            IsVerified = false
        };

        var result = _service.Validate(candidate, definitions, instances, []);

        Assert.True(result.IsAllowed);
        Assert.False(result.IsProvisional);
    }

    [Fact]
    public void Validate_AcToDcTypeMismatch_RejectsConnection()
    {
        var definitions = GenericEquipmentCatalog.CreateStarterDefinitions();
        var instances = CreateDemoInstances();

        var candidate = new ElectricalConnection
        {
            Id = "conn-invalid-type",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-pv",
                PortId = "pv-dc-out"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-load",
                PortId = "ac-load-in"
            },
            IsVerified = false
        };

        var result = _service.Validate(candidate, definitions, instances, []);

        Assert.False(result.IsAllowed);
        Assert.Contains(result.Messages, m => m.Code == "electrical-type-mismatch");
    }

    [Fact]
    public void Validate_DuplicateConnection_RejectsConnection()
    {
        var definitions = GenericEquipmentCatalog.CreateStarterDefinitions();
        var instances = CreateDemoInstances();

        var candidate = new ElectricalConnection
        {
            Id = "conn-dup",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-inverter",
                PortId = "ac-out"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-load",
                PortId = "ac-load-in"
            },
            IsVerified = false
        };

        var existing = new ElectricalConnection
        {
            Id = "conn-existing",
            From = candidate.From,
            To = candidate.To,
            IsVerified = true
        };

        var result = _service.Validate(candidate, definitions, instances, [existing]);

        Assert.False(result.IsAllowed);
        Assert.Contains(result.Messages, m => m.Code == "duplicate-connection");
    }

    [Fact]
    public void Validate_UnknownSpecification_AllowProvisional()
    {
        var definitions = GenericEquipmentCatalog.CreateStarterDefinitions();
        definitions.Add(new EquipmentDefinition
        {
            Id = "def-unknown-dc-input",
            Category = EquipmentCategory.Inverter,
            DisplayName = "Unknown DC Input Device",
            VerificationStatus = SpecificationVerificationStatus.Unknown,
            Ports =
            [
                new ElectricalPortDefinition
                {
                    Id = "unknown-dc-in",
                    Label = "Unknown DC In",
                    ElectricalType = ElectricalType.Dc,
                    Direction = PortDirection.Input,
                    Purpose = PortPurpose.PvInput,
                    VoltageMin = null,
                    VoltageMax = null,
                    VerificationStatus = SpecificationVerificationStatus.Unknown
                }
            ]
        });

        var instances = CreateDemoInstances();
        instances.Add(new EquipmentInstance
        {
            Id = "inst-unknown",
            DefinitionId = "def-unknown-dc-input",
            DisplayName = "Unknown Device",
            VerificationStatus = SpecificationVerificationStatus.Unknown
        });

        var candidate = new ElectricalConnection
        {
            Id = "conn-provisional",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-pv",
                PortId = "pv-dc-out"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-unknown",
                PortId = "unknown-dc-in"
            },
            IsVerified = false
        };

        var result = _service.Validate(candidate, definitions, instances, []);

        Assert.True(result.IsAllowed);
        Assert.True(result.IsProvisional);
        Assert.Contains(result.Messages, m => m.Code == "specification-unknown");
    }

    private static List<EquipmentInstance> CreateDemoInstances()
    {
        return
        [
            new EquipmentInstance
            {
                Id = "inst-pv",
                DefinitionId = "def-solar-array-6x400w",
                DisplayName = "PV"
            },
            new EquipmentInstance
            {
                Id = "inst-battery",
                DefinitionId = "def-battery-48v-generic",
                DisplayName = "Battery"
            },
            new EquipmentInstance
            {
                Id = "inst-inverter",
                DefinitionId = "def-inverter-generic",
                DisplayName = "Inverter"
            },
            new EquipmentInstance
            {
                Id = "inst-load",
                DefinitionId = "def-load-generic",
                DisplayName = "Load"
            }
        ];
    }
}
