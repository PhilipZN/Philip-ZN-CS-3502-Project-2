using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuSchedulingWinForms
{
    public static class Algorithms
    {
        // ======================= FCFS ==========================
        public static void fcfsAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            // no confirmation dialog, always run scheduling

            // read arrival and burst times
            double[] arrivalTimes = new double[np];
            double[] burstTimes   = new double[np];
            for (int i = 0; i < np; i++)
            {
                arrivalTimes[i] = SafePromptDouble(
                    "Enter arrival time:",
                    "Arrival time for P" + (i + 1));
                burstTimes[i] = SafePromptDouble(
                    "Enter burst time:",
                    "Burst time for P" + (i + 1));
            }

            int[] originalBursts = burstTimes.Select(b => (int)b).ToArray();
            int[] waitingTimes   = new int[np];
            int[] responseTimes  = null;
            bool[] done          = new bool[np];
            double currentTime   = 0;
            int completed        = 0;

            // schedule in arrival order
            while (completed < np)
            {
                // find next by FCFS: earliest arrival among not done
                int idx = -1;
                double earliest = double.MaxValue;
                for (int j = 0; j < np; j++)
                {
                    if (!done[j] && arrivalTimes[j] < earliest)
                    {
                        earliest = arrivalTimes[j];
                        idx = j;
                    }
                }
                // advance time to arrival if idle
                if (currentTime < arrivalTimes[idx])
                    currentTime = arrivalTimes[idx];
                // compute waiting time
                waitingTimes[idx] = (int)(currentTime - arrivalTimes[idx]);
                MessageBox.Show(
                    "Waiting time for P" + (idx + 1) + " = " + waitingTimes[idx],
                    "Waiting time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None);
                // mark done and advance
                done[idx] = true;
                completed++;
                currentTime += burstTimes[idx];
            }

            double makespan = currentTime;
            ShowMetrics("FCFS", waitingTimes, originalBursts, makespan, responseTimes);
        }

        // ======================= SJF ==========================
        public static void sjfAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            // no confirmation dialog, always run scheduling

            // read arrival and burst times
            double[] arrivalTimes = new double[np];
            double[] burstTimes   = new double[np];
            for (int i = 0; i < np; i++)
            {
                arrivalTimes[i] = SafePromptDouble(
                    "Enter arrival time:",
                    "Arrival time for P" + (i + 1));
                burstTimes[i] = SafePromptDouble(
                    "Enter burst time:",
                    "Burst time for P" + (i + 1));
            }

            int[] originalBursts = burstTimes.Select(b => (int)b).ToArray();
            int[] waitingTimes   = new int[np];
            int[] responseTimes  = null;
            bool[] done          = new bool[np];
            double currentTime   = 0;
            int completed        = 0;

            // non-preemptive SJF with arrival times
            while (completed < np)
            {
                int idx = -1;
                double shortest = double.MaxValue;
                for (int j = 0; j < np; j++)
                {
                    if (!done[j] && arrivalTimes[j] <= currentTime && burstTimes[j] < shortest)
                    {
                        shortest = burstTimes[j];
                        idx = j;
                    }
                }
                if (idx == -1)
                {
                    currentTime++;
                    continue;
                }

                // compute waiting time
                waitingTimes[idx] = (int)(currentTime - arrivalTimes[idx]);
                MessageBox.Show(
                    "Waiting time for P" + (idx + 1) + " = " + waitingTimes[idx],
                    "Waiting time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None);

                // mark done and advance time
                done[idx] = true;
                completed++;
                currentTime += burstTimes[idx];
            }

            double makespan = currentTime;
            ShowMetrics("SJF", waitingTimes, originalBursts, makespan, responseTimes);
        }

        // ======================= PRIORITY =======================
        public static void priorityAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            // no confirmation dialog, always run scheduling

            // read arrival times
            double[] arrivalTimes = new double[np];
            for (int i = 0; i < np; i++)
            {
                arrivalTimes[i] = SafePromptDouble(
                    "Enter arrival time:",
                    "Arrival time for P" + (i + 1));
            }

            // read burst times
            double[] burstTimes = new double[np];
            for (int i = 0; i < np; i++)
            {
                burstTimes[i] = SafePromptDouble(
                    "Enter burst time:",
                    "Burst time for P" + (i + 1));
            }

            // read priorities
            int[] priorities = new int[np];
            for (int i = 0; i < np; i++)
            {
                priorities[i] = SafePromptInt(
                    "Enter priority (lower number = higher priority):",
                    "Priority for P" + (i + 1));
            }

            int[] originalBursts = burstTimes.Select(b => (int)b).ToArray();
            int[] waitingTimes = new int[np];
            bool[] done = new bool[np];
            int completed = 0;
            double currentTime = 0.0;

            // non-preemptive priority scheduling with arrival times
            while (completed < np)
            {
                int idx = -1;
                int bestPriority = int.MaxValue;
                for (int j = 0; j < np; j++)
                {
                    if (!done[j] && arrivalTimes[j] <= currentTime && priorities[j] < bestPriority)
                    {
                        bestPriority = priorities[j];
                        idx = j;
                    }
                }
                if (idx == -1)
                {
                    currentTime++;
                    continue;
                }

                // calculate waiting time
                waitingTimes[idx] = (int)(currentTime - arrivalTimes[idx]);
                MessageBox.Show(
                    "Waiting time for P" + (idx + 1) + " = " + waitingTimes[idx],
                    "Waiting time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None);

                // mark complete and advance time
                done[idx] = true;
                completed++;
                currentTime += burstTimes[idx];
            }

            double makespan = currentTime;
            ShowMetrics(
                "Priority",
                waitingTimes,
                originalBursts,
                makespan,
                responseTimes: null);
        }

        // ======================= ROUND ROBIN =======================
        public static void roundRobinAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            int i, counter = 0, remaining;
            double total = 0.0, timeQuantum;
            double waitTime = 0, turnaroundTime = 0;
            double averageWaitTime, averageTurnaroundTime;
            double[] arrivalTime = new double[np];
            double[] burstTime   = new double[np];
            double[] temp        = new double[np];

            // no confirmation dialog, always run scheduling

            // read arrival & burst times
            for (i = 0; i < np; i++)
            {
                arrivalTime[i] = SafePromptDouble(
                    "Enter arrival time: ",
                    "Arrival time for P" + (i + 1));
                burstTime[i] = SafePromptDouble(
                    "Enter burst time: ",
                    "Burst time for P" + (i + 1));
                temp[i] = burstTime[i];
            }

            // read quantum
            timeQuantum = SafePromptDouble(
                "Enter time quantum: ",
                "Time Quantum");

            // snapshot bursts & prepare metrics
            int[] originalBursts = burstTime.Select(b => (int)b).ToArray();
            int[] waitingTimes   = new int[np];
            int[] responseTimes  = null;
            remaining = np;

            // execute round robin
            for (total = 0, i = 0; remaining > 0; )
            {
                if (temp[i] <= timeQuantum && temp[i] > 0)
                {
                    total += temp[i];
                    temp[i] = 0;
                    counter = 1;
                }
                else if (temp[i] > 0)
                {
                    temp[i] -= timeQuantum;
                    total += timeQuantum;
                }

                if (temp[i] == 0 && counter == 1)
                {
                    remaining--;
                    double tat = total - arrivalTime[i];
                    double wt  = tat - burstTime[i];
                    MessageBox.Show(
                        "Turnaround time for P" + (i + 1) + " : " + tat,
                        "Turnaround time for P" + (i + 1),
                        MessageBoxButtons.OK);
                    MessageBox.Show(
                        "Wait time for P" + (i + 1) + " : " + wt,
                        "Wait time for P" + (i + 1),
                        MessageBoxButtons.OK);
                    turnaroundTime += tat;
                    waitTime       += wt;
                    waitingTimes[i] = (int)wt;
                    counter = 0;
                }

                if (i == np - 1) i = 0;
                else if (arrivalTime[i + 1] <= total) i++;
                else i = 0;
            }

            averageWaitTime       = waitTime / np;
            averageTurnaroundTime = turnaroundTime / np;
            MessageBox.Show(
                "Average wait time for " + np + " processes: " + averageWaitTime + " sec(s)",
                "",
                MessageBoxButtons.OK);
            MessageBox.Show(
                "Average turnaround time for " + np + " processes: " + averageTurnaroundTime + " sec(s)",
                "",
                MessageBoxButtons.OK);

            // makespan = total time after last slice
            double makespan = total;

            // show performance metrics
            ShowMetrics("Round Robin", waitingTimes, originalBursts, makespan, responseTimes);
        }

        // ========================== SRTF ==========================
        public static void srtfAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            double[] arrival   = new double[np];
            double[] burst     = new double[np];
            double[] remaining = new double[np];
            double[] waitArr   = new double[np];

            for (int i = 0; i < np; i++)
            {
                arrival[i]   = SafePromptDouble($"P{i+1} arrival time:", "Arrival");
                burst[i]     = SafePromptDouble($"P{i+1} burst time:",   "Burst");
                remaining[i] = burst[i];
            }

            // snapshot bursts & prepare metrics
            int[] originalBursts = burst.Select(b => (int)b).ToArray();
            int[] waitingTimes   = new int[np];
            int[] responseTimes  = null;

            double time   = 0, totalWT = 0;
            int completed = 0;
            bool[] done   = new bool[np];

            // simulate SRTF
            while (completed < np)
            {
                int idx = -1;
                double minRem = double.MaxValue;
                for (int i = 0; i < np; i++)
                {
                    if (!done[i] && arrival[i] <= time && remaining[i] < minRem)
                    {
                        minRem = remaining[i];
                        idx    = i;
                    }
                }

                if (idx == -1)
                {
                    time++;
                    continue;
                }

                remaining[idx]--;
                time++;
                if (remaining[idx] == 0)
                {
                    done[idx]   = true;
                    completed++;
                    double wt   = time - arrival[idx] - burst[idx];
                    totalWT    += wt;
                    waitArr[idx] = wt;
                    MessageBox.Show(
                        $"P{idx+1} finished at t={time}\\nWaiting Time={wt}",
                        "SRTF",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.None);
                }
            }

            double awt = totalWT / np;
            MessageBox.Show(
                $"SRTF Average Waiting Time = {awt:F2}",
                "SRTF Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            for (int i = 0; i < np; i++)
                waitingTimes[i] = (int)waitArr[i];

            // makespan = final time
            double makespan = time;

            // show performance metrics
            ShowMetrics("SRTF", waitingTimes, originalBursts, makespan, responseTimes);
        }

        // ========================== HRRN ==========================
        public static void hrrnAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            double[] arrival = new double[np];
            double[] burst   = new double[np];
            double[] waitArr = new double[np];

            for (int i = 0; i < np; i++)
            {
                arrival[i] = SafePromptDouble($"P{i+1} arrival time:", "Arrival");
                burst[i]   = SafePromptDouble($"P{i+1} burst time:",   "Burst");
            }

            // snapshot bursts & prepare metrics
            int[] originalBursts = burst.Select(b => (int)b).ToArray();
            int[] waitingTimes   = new int[np];
            int[] responseTimes  = null;

            double time    = 0, totalWT = 0;
            int completed  = 0;
            bool[] done    = new bool[np];

            // simulate HRRN
            while (completed < np)
            {
                double bestRR = -1;
                int idx = -1;
                for (int i = 0; i < np; i++)
                {
                    if (!done[i] && arrival[i] <= time)
                    {
                        double wait = time - arrival[i];
                        double rr   = (wait + burst[i]) / burst[i];
                        if (rr > bestRR)
                        {
                            bestRR = rr;
                            idx    = i;
                        }
                    }
                }

                if (idx == -1)
                {
                    time++;
                    continue;
                }

                double wt = time - arrival[idx];
                totalWT  += wt;
                waitArr[idx] = wt;
                time     += burst[idx];
                done[idx]   = true;
                completed++;

                MessageBox.Show(
                    $"P{idx+1} ran [{time-burst[idx]}→{time}]\\nWaiting Time={wt:F2}\\nResponse Ratio={bestRR:F2}",
                    "HRRN",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None);
            }

            double awt = totalWT / np;
            MessageBox.Show(
                $"HRRN Average Waiting Time = {awt:F2}",
                "HRRN Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            for (int i = 0; i < np; i++)
                waitingTimes[i] = (int)waitArr[i];

            // makespan = final time
            double makespan = time;

            // show performance metrics
            ShowMetrics("HRRN", waitingTimes, originalBursts, makespan, responseTimes);
        }

        // ====================== METRICS HELPER ======================
        public static void ShowMetrics(
            string algorithmName,
            int[] waitingTimes,
            int[] burstTimes,
            double makespan,
            int[] responseTimes = null)
        {
            int n = waitingTimes.Length;
            double totalWaiting    = waitingTimes.Sum();
            double totalBurst      = burstTimes.Sum();
            double avgWaiting      = totalWaiting    / n;
            double avgTurnaround   = (totalWaiting + totalBurst) / n;
            double cpuUtilization  = totalBurst      / makespan * 100.0;
            double throughput      = (double)n       / makespan;

            var msg =
                $"Average Waiting Time:    {avgWaiting:F2}\n" +
                $"Average Turnaround Time: {avgTurnaround:F2}\n" +
                $"CPU Utilization:          {cpuUtilization:F2}%\n" +
                $"Throughput:               {throughput:F2} proc/unit time";

            if (responseTimes != null)
            {
                double avgResponse = responseTimes.Sum() / (double)n;
                msg += $"\nAverage Response Time:   {avgResponse:F2}";
            }

            MessageBox.Show(msg,
                "Performance Metrics",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // add row to metrics table in main form
            var mainForm = Application.OpenForms.OfType<CpuScheduler>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.AddMetricRow(algorithmName, avgWaiting, avgTurnaround, cpuUtilization, throughput, responseTimes != null ? (double?)responseTimes.Sum() / n : null);
            }
        }

        // ----------------------- Input Helpers -----------------------
        private static double SafePromptDouble(string prompt, string title)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, title, "", -1, -1);
            if (!double.TryParse(input, out double value))
                value = 0;
            return value;
        }

        private static int SafePromptInt(string prompt, string title)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, title, "", -1, -1);
            if (!int.TryParse(input, out int value))
                value = 0;
            return value;
        }
    }
}
