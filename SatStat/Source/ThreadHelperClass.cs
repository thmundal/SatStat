﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    public static class ThreadHelperClass
    {
        // https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
        delegate void SetBaudrateCallback(Form f, Panel p, ListBox c, SerialSettingsCollection settings);

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

        public static void Invoke(Form f, Panel p, Control c, Action<Hashtable> cb, Hashtable data)
        {
            if(c.InvokeRequired)
            {
                f.Invoke(cb, new object[] { data });
            } else
            {
                cb.Invoke(data);
            }
        }
    }
}