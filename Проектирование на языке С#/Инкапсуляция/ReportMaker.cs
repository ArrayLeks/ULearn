using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incapsulation.Failures
{
    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class Common
    {
        public static bool IsEarlier(DateTime device, DateTime current)
        {
            if (device.Year != current.Year) return device.Year < current.Year;
            if (device.Month != current.Month) return device.Month < current.Month;
            return device.Day < current.Day;
        }
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            DateTime dateTime = new DateTime(year, month, day);

            var devicesList = new List<Device>();
            foreach (var device in devices)
                devicesList.Add(new Device(device,
                    failureTypes,
                    times,
                    deviceId));

            return ReportMaker.FindDevicesFailedBeforeDate(devicesList, dateTime);
        }

        public static List<string> FindDevicesFailedBeforeDate(
            List<Device> devices,
            DateTime dateTime)
        {
            var problematicDevices = new HashSet<int>();
            foreach (var device in devices)
                if (Failure.IsFailureSerious(device.failure) && Common.IsEarlier(device.dateTime, dateTime))
                    problematicDevices.Add(device.id);

            return devices
                .Where(s => problematicDevices.Contains(s.id))
                .Select(s => s.name)
                .ToList();
        }
    }

    public class Device
    {
        public readonly int id;
        public readonly string name;
        public readonly FailureType failure;
        public readonly DateTime dateTime;

        public Device(Dictionary<string, object> dict,
            int[] failureType,
            object[][] times,
            int[] devices)
        {
            id = (int)dict["DeviceId"];
            name = dict["Name"] as string;
            var index = Array.IndexOf(devices, id);
            failure = (FailureType)failureType[index];
            dateTime = new DateTime((int)times[index][2], (int)times[index][1], (int)times[index][0]);
        }

        public override string ToString()
        {
            return $"Id: {id}, Name: {name}";
        }
    }

    public class Failure
    {
        public static bool IsFailureSerious(FailureType failureType)
        {
            return failureType == FailureType.HardwareFailures || failureType == FailureType.UnexpectedShutdown;
        }
    }
}