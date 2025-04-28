# CPU Scheduling Simulator

This project is a CPU Scheduling Simulator developed for OwlTech.  
It is a Windows Forms Application (.NET Framework) that visually demonstrates how various CPU scheduling algorithms perform under different workloads.

---

## Features

- Implementation of multiple CPU Scheduling algorithms:
  - First-Come, First-Served (FCFS)
  - Shortest Job First (SJF)
  - Priority Scheduling (non-preemptive)
  - Round Robin (RR)
  - **Shortest Remaining Time First (SRTF)** — (Newly Added)
  - **Highest Response Ratio Next (HRRN)** — (Newly Added)
- Measurement and comparison of key performance metrics:
  - Average Waiting Time (AWT)
  - Average Turnaround Time (ATT)
  - CPU Utilization (%)
  - Throughput (processes/unit time)
- **Metrics Tab** added:
  - Displays a summary table showing the key performance results for each scheduling algorithm after simulation.
  - Allows easy comparison across algorithms.

---

## Changes from Starter Code

- **Two new algorithms implemented:**
  - **Shortest Remaining Time First (SRTF):** A preemptive version of SJF where the currently running process can be preempted if a newly arrived process has a shorter remaining burst time.
  - **Highest Response Ratio Next (HRRN):** A non-preemptive scheduling algorithm that prioritizes jobs based on their response ratio (waiting time + service time) / service time.
- **New Metrics Collection:**
  - Extended the simulator to calculate CPU Utilization and Throughput in addition to Waiting Time and Turnaround Time.
- **New Metrics Tab UI:**
  - Created a new tab in the application where simulation metrics for all algorithms are displayed together in a table.
  - Simplifies performance comparison between algorithms.

---

## Build Instructions

You can build the project using **MSBuild** from the command line:

```bash
msbuild CpuSchedulingWinForms.sln /t:Rebuild /p:Configuration=Debug
```

- This will compile the solution into the `CpuSchedulingWinForms\bin\Debug\` directory.
- Make sure you have Visual Studio Build Tools or Visual Studio installed.

---

## Run Instructions

After building, you can run the simulator executable directly:

```bash
.\CpuSchedulingWinForms\bin\Debug\CpuSchedulingWinForms.exe
```

- The application GUI will launch.
- Use the GUI to:
  - Load or enter process workloads.
  - Select and run different scheduling algorithms.
  - View execution orders and performance metrics.
  - Compare results easily using the **Metrics** tab.

---

## Requirements

- Windows OS
- Visual Studio 2022 or Visual Studio Build Tools (for MSBuild)
- .NET Framework 4.7.2 or higher

---

## Authors

### Philip Nsajja
### Original Application by Francis Nweke
---

