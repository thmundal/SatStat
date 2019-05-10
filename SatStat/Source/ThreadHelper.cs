using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    /// <summary>
    /// A static class containing methods for running methods on a seperate thread. Internally handles winforms UI cross-threading compatibility
    /// </summary>
    public static class ThreadHelper
    {
        // https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
        delegate void SetBaudrateCallback(Form f, Panel p, ListBox c, SerialSettingsCollection settings);

        /// <summary>
        /// Set baud rates on the UI control
        /// </summary>
        /// <param name="f"></param>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <param name="settings"></param>
        public static void SetBaudrates(Form f, Panel p, ListBox c, SerialSettingsCollection settings)
        {
            if (c.InvokeRequired)
            {
                SetBaudrateCallback d = new SetBaudrateCallback(SetBaudrates);
                f.Invoke(d, new object[] { f, p, c, settings });
            }
            else
            {
                p.Show();
                foreach (int baud_rate in settings.baud_rates)
                {
                    c.Items.Add(baud_rate);
                }
            }
        }

        public static void SetCOMConnectionStatus(string status)
        {
            UI_Invoke(Program.app, null, Program.app.GetConnectionStatusControl(), (Hashtable h) =>
            {
                Program.app.SetCOMConnectionStatus(status);
            }, null);
        }

        public static void SetNetworkConnectionStatus(string status)
        {
            UI_Invoke(Program.app, null, Program.app.GetConnectionStatusControl(), (Hashtable h) =>
            {
                Program.app.SetNetworkConnectionStatus(status);
            }, null);
        }

        /// <summary>
        /// Invoke an action on a specific UI control
        /// </summary>
        /// <param name="f">The form containing the control</param>
        /// <param name="p">The panel containing the control</param>
        /// <param name="c">The control itself</param>
        /// <param name="cb">An action to invoke</param>
        /// <param name="data">A HashTable containing data that might be used inside the action</param>
        public static void UI_Invoke(Form f, Panel p, Control c, Action<Hashtable> cb, Hashtable data)
        {
            if(f.IsDisposed || f.Disposing || f == null)
            {
                return;
            }

            if(c.InvokeRequired)
            {
                if(!f.Disposing)
                {
                    try
                    {
                        f.Invoke(cb, new object[] { data });
                    } catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            } else
            {
                cb.Invoke(data);
            }
        }

        /// <summary>
        /// Invoke an action on a specific UI control in a separate task
        /// </summary>
        /// <param name="f">The form containing the control</param>
        /// <param name="p">The panel containing the control</param>
        /// <param name="c">The control itself</param>
        /// <param name="cb">An action to invoke</param>
        /// <param name="data">A HashTable containing data that might be used inside the action</param>
        public static void UI_TaskInvoke(Form f, Panel p, Control c, Action<Hashtable> cb, Hashtable data)
        {
            Task.Run(() =>
            {
                UI_Invoke(f, p, c, cb, data);
            });
        }
    }
}
