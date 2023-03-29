using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor;

namespace FastKing
{
    public class HandwareMonitor: IVisitor
    {
        Computer pc;
        public float? CPU_Load, CPU_Temp, CPU_Power;
        public float? Memory_Load;
        public float? GPU_Load, GPU_Temp, GPU_Power, GPU_RAM;

        public HandwareMonitor()
        {
            pc = new Computer
            {
                MainboardEnabled = true,
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true
            };
            pc.Open();
            VisitComputer(pc);
        }

        public void UpdateValue()
        {
            foreach (var hardwareItem in pc.Hardware)
            {
                VisitHardware(hardwareItem);  //访问前更新？
                switch (hardwareItem.HardwareType)
                {
                    case HardwareType.Mainboard:
                        break;
                    case HardwareType.SuperIO:
                        break;
                    case HardwareType.CPU:
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.Name == "CPU Total")
                            {
                                if (sensor.SensorType == SensorType.Load)
                                {
                                    CPU_Load = sensor.Value;
                                }
                                if (sensor.SensorType == SensorType.Power)
                                {
                                    CPU_Power = sensor.Value;
                                }
                            }

                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                CPU_Temp = sensor.Value;
                            }
                            //if (sensor.Name == "CPU Package")
                            //{
                            //    if (sensor.SensorType == SensorType.Temperature)
                            //    {
                            //        GPU_Temp = sensor.Value;
                            //    }
                            //}
                        }
                        break;
                    case HardwareType.RAM:
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load)
                            {
                                Memory_Load = sensor.Value;
                            }
                        }
                        break;
                    case HardwareType.GpuNvidia:
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.Name == "GPU Core")
                            {
                                if (sensor.SensorType == SensorType.Temperature)
                                {
                                    GPU_Temp = sensor.Value;
                                }
                                if (sensor.SensorType == SensorType.Load)
                                {
                                    GPU_Load = sensor.Value;
                                }
                                if (sensor.SensorType == SensorType.Power)
                                {
                                    GPU_Power = sensor.Value;
                                }
                            }
                            else if (sensor.Name == "GPU Memory")
                            {
                                GPU_RAM = sensor.Value;
                            }
                        }
                        break;
                    case HardwareType.GpuAti:
                        break;
                    case HardwareType.TBalancer:
                        break;
                    case HardwareType.Heatmaster:
                        break;
                    case HardwareType.HDD:
                        break;
                    default:
                        break;
                }
            }
        }
     
        public void VisitComputer(IComputer computer)
        {
            pc.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }

        public void VisitParameter(IParameter parameter)
        {
        }

        public void VisitSensor(ISensor sensor)
        {
        }
    }
}
