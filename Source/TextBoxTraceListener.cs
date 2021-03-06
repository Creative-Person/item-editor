﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace otitemeditor
{
    public class TextBoxTraceListener : TraceListener
    {
        const UInt32 updateFrequency = 10;
        UInt32 updateCounter = 0;

        private TextBox _target;
        private StringSendDelegate _invokeWrite;

        public TextBoxTraceListener(TextBox target)
        {
            _target = target;
            _invokeWrite = new StringSendDelegate(SendString);
        }

        public override void Write(string message)
        {
            _target.Invoke(_invokeWrite, new object[] { message });
        }

        public override void WriteLine(string message)
        {
            _target.Invoke(_invokeWrite, new object[] { message + Environment.NewLine });
        }

        private delegate void StringSendDelegate(string message);
        private void SendString(string message)
        {
            // No need to lock text box as this function will only 

            // ever be executed from the UI thread

            _target.AppendText(message);

            ++updateCounter;
            if (updateCounter >= updateFrequency)
            {
                updateCounter = 0;
                Application.DoEvents();
            }
        }
    }
}
