using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBackup
{
    public partial class BaseForm : Form
    {
        FileSystemWatcher watcher;

        Handler handler;

        readonly Object locker = new object();

        Queue<string> queue;

        delegate void SetTextCallback(string text);

        public BaseForm()
        {
            InitializeComponent();
            Startup();
        }


        private void Source_Click(object sender, EventArgs e)
        {

        }

        private void Destination_Click(object sender, EventArgs e)
        {

        }

        private void BaseForm_Load(object sender, EventArgs e)
        {

        }

        private void RegisterWatcher()
        {
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            lock (locker)
            {
                if (!this.queue.Contains(e.Name))
                {
                    this.queue.Enqueue(e.Name);
                }
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            lock (locker)
            {
                if (!this.queue.Contains(e.Name))
                {
                    this.queue.Enqueue(e.Name);
                }
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            handler.Delete(e.Name);
            AddLog("----------" + e.Name + " is delete!!----------");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            lock (locker)
            {
                if (!this.queue.Contains(e.Name))
                {
                    this.queue.Enqueue(e.Name);
                }
            }
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                AddLog(ex.Message);
            }
        }

        private void Startup()
        {
            string source = ConfigurationManager.AppSettings["Source"];
            string destination = ConfigurationManager.AppSettings["Destination"];

            if (String.IsNullOrEmpty(source) ||
                String.IsNullOrEmpty(destination) ||
                !Directory.Exists(source) ||
                !Directory.Exists(destination))
            {
                string message = "Configuration is not properly set. Please modify and restart this program!";
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);              
            }
            else
            {
                txtSource.Text = source;
                txtDestination.Text = destination;
                watcher = new FileSystemWatcher(source);
                handler = new Handler(source, destination);
                this.queue = new Queue<string>();

                handler.SyncAll();

                RegisterWatcher();

                Thread thread = new Thread(Worker)
                {
                    IsBackground = true
                };
                thread.Start();
            }

        }

        private void Worker()
        {
            while (true)
            {
                Thread.Sleep(5000);
                lock (locker)
                {
                    if (this.queue.TryDequeue(out string name))
                    {
                        Status status = handler.Add(name);

                        AddLog(String.Format("File {0} is {1}", name, status.ToString()));
                    }
                }
            }
        }

        private void AddLog(string text)
        {
            if (log.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddLog);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                log.Items.Add(text);
            }
        }
    }
}
