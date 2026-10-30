using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public static class GenericEquipmentCatalog
{
    public static List<EquipmentDefinition> CreateStarterDefinitions()
    {
        return
        [
            new EquipmentDefinition
            {
                Id = "def-solar-array-6x400w",
                Category = EquipmentCategory.SolarArray,
                DisplayName = "Generic Solar Array 6×400W",
                VerificationStatus = SpecificationVerificationStatus.UserProvided,
                Configuration = new Dictionary<string, string>
                {
                    ["nameplateWatts"] = "2400",
                    ["moduleCount"] = "6",
                    ["moduleWatts"] = "400"
                },
                Ports =
                [
                    new ElectricalPortDefinition
                    {
                        Id = "pv-dc-out",
                        Label = "PV DC Output",
                        ElectricalType = ElectricalType.Dc,
                        Direction = PortDirection.Output,
                        Purpose = PortPurpose.PvOutput,
                        VoltageMin = 120,
                        VoltageMax = 250,
                        CurrentMax = 20,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    }
                ]
            },
            new EquipmentDefinition
            {
                Id = "def-battery-48v-generic",
                Category = EquipmentCategory.BatteryBank,
                DisplayName = "Generic 48V-class Battery Bank",
                VerificationStatus = SpecificationVerificationStatus.UserProvided,
                Configuration = new Dictionary<string, string>
                {
                    ["nominalVoltage"] = "48",
                    ["nominalCapacityWh"] = "10240"
                },
                Ports =
                [
                    new ElectricalPortDefinition
                    {
                        Id = "battery-dc-port",
                        Label = "Battery DC",
                        ElectricalType = ElectricalType.Dc,
                        Direction = PortDirection.Bidirectional,
                        Purpose = PortPurpose.BatteryConnection,
                        VoltageMin = 44,
                        VoltageMax = 58,
                        CurrentMax = 200,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    }
                ]
            },
            new EquipmentDefinition
            {
                Id = "def-inverter-generic",
                Category = EquipmentCategory.Inverter,
                DisplayName = "Generic Inverter",
                VerificationStatus = SpecificationVerificationStatus.UserProvided,
                Configuration = new Dictionary<string, string>
                {
                    ["continuousPowerW"] = "5000"
                },
                Ports =
                [
                    new ElectricalPortDefinition
                    {
                        Id = "pv-dc-in",
                        Label = "PV DC Input",
                        ElectricalType = ElectricalType.Dc,
                        Direction = PortDirection.Input,
                        Purpose = PortPurpose.PvInput,
                        VoltageMin = 100,
                        VoltageMax = 500,
                        CurrentMax = 25,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    },
                    new ElectricalPortDefinition
                    {
                        Id = "battery-dc-inout",
                        Label = "Battery DC",
                        ElectricalType = ElectricalType.Dc,
                        Direction = PortDirection.Bidirectional,
                        Purpose = PortPurpose.BatteryConnection,
                        VoltageMin = 44,
                        VoltageMax = 58,
                        CurrentMax = 220,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    },
                    new ElectricalPortDefinition
                    {
                        Id = "ac-out",
                        Label = "AC Output",
                        ElectricalType = ElectricalType.Ac,
                        Direction = PortDirection.Output,
                        Purpose = PortPurpose.AcOutput,
                        VoltageMin = 110,
                        VoltageMax = 125,
                        CurrentMax = 40,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    }
                ]
            },
            new EquipmentDefinition
            {
                Id = "def-load-generic",
                Category = EquipmentCategory.ElectricalLoad,
                DisplayName = "Generic Electrical Load",
                VerificationStatus = SpecificationVerificationStatus.UserProvided,
                Configuration = new Dictionary<string, string>
                {
                    ["activeWatts"] = "1200"
                },
                Ports =
                [
                    new ElectricalPortDefinition
                    {
                        Id = "ac-load-in",
                        Label = "AC Input",
                        ElectricalType = ElectricalType.Ac,
                        Direction = PortDirection.Input,
                        Purpose = PortPurpose.LoadInput,
                        VoltageMin = 110,
                        VoltageMax = 125,
                        CurrentMax = 15,
                        VerificationStatus = SpecificationVerificationStatus.UserProvided
                    }
                ]
            }
        ];
    }
}
