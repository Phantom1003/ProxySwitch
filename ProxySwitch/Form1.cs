using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace ProxySwitch
{
    public partial class Form1 : Form
    {
        // proxy switch option
        const string ProxyKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
        const string ProxyEnableKey = "ProxyEnable";
        const string ProxyServerKey = "ProxyServer";

        bool getProxyEnable => RegistryHelper.Get(Registry.CurrentUser, ProxyKeyPath, ProxyEnableKey, false);
        string getProxyServer => RegistryHelper.Get(Registry.CurrentUser, ProxyKeyPath, ProxyServerKey, "No default proxy!");

        private static Icon proxyOnPath = new Icon("proxy_on.ico");
        private static Icon proxyOffPath = new Icon("proxy_off.ico");


        // auto startup option
        const string StartupKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private string AppNameKey = Application.ProductName;
        private string AppPath = Application.ExecutablePath;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();

            UpdateTray();
            UpdateMenu_ProxyItem();
        }

        private void UpdateTray()
        {
            bool enabled = getProxyEnable;
            notifyIcon1.Icon = enabled ? proxyOnPath : proxyOffPath;
            notifyIcon1.Text = enabled ? getProxyServer : "Direct";
        }

        private void UpdateMenu_ProxyItem()
        {
            bool enabled = getProxyEnable;
            proxyToolStripMenuItem.Text = "Proxy: " + (enabled ? "Enabled" : "Disabled");
        }

        private void ToggleProxy()
        {
            bool enabled = getProxyEnable;
            RegistryHelper.Set(Registry.CurrentUser, ProxyKeyPath, ProxyEnableKey, enabled ? 0 : 1, RegistryValueKind.DWord);
            UpdateTray();
            UpdateMenu_ProxyItem();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleProxy();
            }
        }

        private void proxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleProxy();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
