using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Windows.Phone.Notification.Management;


namespace DemoWindowService
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer = null;
        private NotifyIcon notifyIcon = new NotifyIcon
        {
            Icon = new Icon(@"C:\Users\GHM\Documents\avalanche_avax.ico"),
            Visible = true
        };
        public Service1()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
           
            timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Tick;
            timer.Enabled = true;
            Utilities.WriteLogError("Test for 1st run WindowsService");
        }

        private async void timer_Tick(object sender, ElapsedEventArgs args)
        {
            List<ScheduleVM> schedules = await getAllSchedulesNotify(args.SignalTime);
            if (schedules == null)
            {
                return;
            }

            foreach (ScheduleVM schedule in schedules)
            {
                String notify = getNotifycation(schedule);
                Console.WriteLine(notify);

                Console.WriteLine(notifyIcon);
                Console.WriteLine(notifyIcon.Icon);
                notifyIcon.Tag = "This is art";
                notifyIcon.ShowBalloonTip(10000, "Thông báo lịch hẹn", notify, ToolTipIcon.Info);
            }

            Utilities.WriteLogError("Timer has ticked for doing something!!!");
        }

        public async Task<List<ScheduleVM>> getAllSchedulesNotify(DateTime dateTime)
        {
            HttpClient httpClient = new HttpClient();
            DateTime currentTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
            string currentTimeStr = currentTime.ToString("yyyy-MM-dd");
            String link = $"http://localhost:5112/api/Schedules/getAllNotify?currentTime={currentTimeStr}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(link);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseData);
                    List<ScheduleVM> scheduleVMs = JsonConvert.DeserializeObject<List<ScheduleVM>>(responseData);
                    return scheduleVMs;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public String getNotifycation(ScheduleVM scheduleVM)
        {
            return $"Sắp đến lịch hẹn: {scheduleVM.Reason}\nGiờ bắt đầu: {scheduleVM.FromX:D2}: {scheduleVM.FromY:D2}\n đến {scheduleVM.ToX:D2} : {scheduleVM.ToY:D2}";
        }

        protected override void OnStop()
        {
            // Ghi log lại khi Services đã được stop
            timer.Enabled = true;
            Utilities.WriteLogError("1st WindowsService has been stop");
        }
    }
}
