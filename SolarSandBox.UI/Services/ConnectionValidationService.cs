using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public sealed class ConnectionValidationService : IConnectionValidationService
{
    public ConnectionValidationResult Validate(
        ElectricalConnection candidate,
        IReadOnlyCollection<EquipmentDefinition> equipmentDefinitions,
        IReadOnlyCollection<EquipmentInstance> equipmentInstances,
        IReadOnlyCollection<ElectricalConnection> existingConnections)
    {
        if (existingConnections.Any(existing =>
                existing.From.EquipmentInstanceId == candidate.From.EquipmentInstanceId &&
                existing.From.PortId == candidate.From.PortId &&
                existing.To.EquipmentInstanceId == candidate.To.EquipmentInstanceId &&
                existing.To.PortId == candidate.To.PortId))
        {
            return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "duplicate-connection",
                Severity = DiagnosticSeverity.Warning,
                Description = "A connection between these exact ports already exists."
            });
        }

        var fromResolution = ResolvePort(candidate.From, equipmentDefinitions, equipmentInstances);
        if (!fromResolution.IsResolved)
        {
            return ConnectionValidationResult.Rejected(fromResolution.ErrorMessage!);
        }

        var toResolution = ResolvePort(candidate.To, equipmentDefinitions, equipmentInstances);
        if (!toResolution.IsResolved)
        {
            return ConnectionValidationResult.Rejected(toResolution.ErrorMessage!);
        }

        var fromPort = fromResolution.Port!;
        var toPort = toResolution.Port!;

        if (!CanSource(fromPort.Direction) || !CanSink(toPort.Direction))
        {
            return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "invalid-direction",
                Severity = DiagnosticSeverity.Critical,
                Description = "Connection direction is invalid for the selected ports."
            });
        }

        if (fromPort.ElectricalType != ElectricalType.Unknown && toPort.ElectricalType != ElectricalType.Unknown && fromPort.ElectricalType != toPort.ElectricalType)
        {
            return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "electrical-type-mismatch",
                Severity = DiagnosticSeverity.Critical,
                Description = $"Incompatible electrical types: {fromPort.ElectricalType} cannot connect to {toPort.ElectricalType}."
            });
        }

        var messages = new List<ConnectionValidationMessage>();

        if (!ArePurposesCompatible(fromPort.Purpose, toPort.Purpose))
        {
            if (fromPort.Purpose == PortPurpose.Unknown || toPort.Purpose == PortPurpose.Unknown)
            {
                messages.Add(new ConnectionValidationMessage
                {
                    Code = "purpose-unknown",
                    Severity = DiagnosticSeverity.Info,
                    Description = "Port purpose is unknown. Connection is provisional and unverified."
                });
            }
            else
            {
                return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
                {
                    Code = "purpose-mismatch",
                    Severity = DiagnosticSeverity.Critical,
                    Description = $"Port purpose mismatch: {fromPort.Purpose} is not compatible with {toPort.Purpose}."
                });
            }
        }

        if (!AreVoltageRangesCompatible(fromPort, toPort))
        {
            return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "voltage-range-mismatch",
                Severity = DiagnosticSeverity.Critical,
                Description = "Known voltage operating ranges do not overlap."
            });
        }

        var provisional = messages.Any();

        if (HasUnknownElectricalSpecification(fromPort) || HasUnknownElectricalSpecification(toPort))
        {
            provisional = true;
            messages.Add(new ConnectionValidationMessage
            {
                Code = "specification-unknown",
                Severity = DiagnosticSeverity.Info,
                Description = "One or more specifications are unknown. Connection is provisional/unverified."
            });
        }

        if (provisional)
        {
            return ConnectionValidationResult.Allowed(true, [.. messages]);
        }

        messages.Add(new ConnectionValidationMessage
        {
            Code = "connection-verified",
            Severity = DiagnosticSeverity.Info,
            Description = "Connection is compatible based on known typed-port constraints."
        });

        return ConnectionValidationResult.Allowed(false, [.. messages]);
    }

    private static bool CanSource(PortDirection direction) => direction is PortDirection.Output or PortDirection.Bidirectional;

    private static bool CanSink(PortDirection direction) => direction is PortDirection.Input or PortDirection.Bidirectional;

    private static bool HasUnknownElectricalSpecification(ElectricalPortDefinition port)
    {
        return port.ElectricalType == ElectricalType.Unknown
               || port.Purpose == PortPurpose.Unknown
               || port.VoltageMin is null
               || port.VoltageMax is null;
    }

    private static bool AreVoltageRangesCompatible(ElectricalPortDefinition from, ElectricalPortDefinition to)
    {
        if (from.VoltageMin is null || from.VoltageMax is null || to.VoltageMin is null || to.VoltageMax is null)
        {
            return true;
        }

        var overlapMin = Math.Max(from.VoltageMin.Value, to.VoltageMin.Value);
        var overlapMax = Math.Min(from.VoltageMax.Value, to.VoltageMax.Value);

        return overlapMin <= overlapMax;
    }

    private static bool ArePurposesCompatible(PortPurpose from, PortPurpose to)
    {
        return (from, to) switch
        {
            (PortPurpose.PvOutput, PortPurpose.PvInput) => true,
            (PortPurpose.BatteryConnection, PortPurpose.BatteryConnection) => true,
            (PortPurpose.AcOutput, PortPurpose.AcInput) => true,
            (PortPurpose.AcOutput, PortPurpose.LoadInput) => true,
            (PortPurpose.Unknown, _) => false,
            (_, PortPurpose.Unknown) => false,
            _ => false
        };
    }

    private static PortResolution ResolvePort(
        EquipmentPortReference reference,
        IReadOnlyCollection<EquipmentDefinition> equipmentDefinitions,
        IReadOnlyCollection<EquipmentInstance> equipmentInstances)
    {
        var instance = equipmentInstances.FirstOrDefault(x => x.Id == reference.EquipmentInstanceId);
        if (instance is null)
        {
            return PortResolution.Failed(new ConnectionValidationMessage
            {
                Code = "instance-not-found",
                Severity = DiagnosticSeverity.Critical,
                Description = $"Equipment instance '{reference.EquipmentInstanceId}' was not found."
            });
        }

        var definition = equipmentDefinitions.FirstOrDefault(x => x.Id == instance.DefinitionId);
        if (definition is null)
        {
            return PortResolution.Failed(new ConnectionValidationMessage
            {
                Code = "definition-not-found",
                Severity = DiagnosticSeverity.Critical,
                Description = $"Equipment definition '{instance.DefinitionId}' was not found."
            });
        }

        var port = definition.Ports.FirstOrDefault(x => x.Id == reference.PortId);
        if (port is null)
        {
            return PortResolution.Failed(new ConnectionValidationMessage
            {
                Code = "port-not-found",
                Severity = DiagnosticSeverity.Critical,
                Description = $"Port '{reference.PortId}' was not found on '{instance.DisplayName}'."
            });
        }

        return PortResolution.Success(port);
    }

    private sealed class PortResolution
    {
        public bool IsResolved { get; init; }

        public ElectricalPortDefinition? Port { get; init; }

        public ConnectionValidationMessage? ErrorMessage { get; init; }

        public static PortResolution Success(ElectricalPortDefinition port)
        {
            return new PortResolution
            {
                IsResolved = true,
                Port = port
            };
        }

        public static PortResolution Failed(ConnectionValidationMessage message)
        {
            return new PortResolution
            {
                IsResolved = false,
                ErrorMessage = message
            };
        }
    }
}
