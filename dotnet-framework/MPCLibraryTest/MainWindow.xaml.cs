using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace MPCLibraryTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void AddTextToTextBox(string _text);

        private string APP_NAME = "MPCLibraryTest";
        microsensys.MPC.Communication m_Comm;
        private int counter;
        List<microsensys.MPC.Contents.Dataset> m_datasetList = new List<microsensys.MPC.Contents.Dataset>();

        public MainWindow()
        {
            InitializeComponent();

            button_READ.IsEnabled = false;
            button_Delete.IsEnabled = false;
            microsensys.DevTools.Tracer.Initialize(APP_NAME);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //m_Comm = new microsensysMPC.Communication(microsensysMPC.Settings.CommunicationSettings.Load());
            m_Comm = new microsensys.MPC.Communication(null);
            checkBox_ReadNew.IsChecked = m_Comm.ReadOnlyNewDatasets;
            checkBox_ResetPointers.IsChecked = m_Comm.ResetPointersAfterRead;
            
            m_Comm.ReaderInfoEvent += new microsensys.MPC.Communication.ReaderInformationEventHandler(m_Comm_ReaderInfoEvent);
            m_Comm.ConversionEvent += new microsensys.MPC.Communication.ConversionEventHandler(m_Comm_ConversionEvent);
            m_Comm.ProgressEvent += new microsensys.MPC.Communication.ProgressEventHandler(m_Comm_ProgressEvent);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_Comm != null)
            {
                m_Comm.Dispose();
                microsensys.DevTools.Tracer.WriteLine("Communication class disposed");
            }
            microsensys.DevTools.Tracer.Dispose();
        }

        #region Communication EventHandlers
        void m_Comm_ProgressEvent(object sender, microsensys.MPC.Events.ProgressEventArgs e)
        {
            try
            {
                //InvokeRequired gibt es bei WPF nicht mehr
                if (!Dispatcher.Thread.Equals(Thread.CurrentThread))
                {
                    Dispatcher.Invoke(new microsensys.MPC.Communication.ProgressEventHandler(m_Comm_ProgressEvent), sender, e);
                    return;
                }
                switch (e.CurrentOperation)
                {
                    case "Reading Log":
                        if (e.TotalPercentage != progressBar_ReadLog.Value)
                            addDatasetsText(e.ToString());
                        break;
                    case "Converting...":
                        //break;
                    default:
                        addDatasetsText(e.ToString());
                        break;
                }
                //if (e.CurrentOperation != "Reading Log") addDatasetsText(e.ToString());
                progressBar_ReadLog.Value = e.TotalPercentage;
            }
            catch { }
        }

        void m_Comm_ConversionEvent(object sender, microsensys.MPC.Events.ConversionEventInfo info, microsensys.MPC.Contents.Dataset ds)
        {
            try
            {
                //InvokeRequired gibt es bei WPF nicht mehr
                //if (!Dispatcher.Thread.Equals(Thread.CurrentThread))
                //{
                //    Dispatcher.Invoke(new microsensys.MPC.Communication.ConversionEventHandler(m_Comm_ConversionEvent), sender, info, ds);
                //    return;
                //}
                switch (info)
                {
                    case microsensys.MPC.Events.ConversionEventInfo.NewDataset:
                        //addDatasetsText(ds.ToString());
                        m_datasetList.Add(ds);
                        counter++;
                        break;
                    case microsensys.MPC.Events.ConversionEventInfo.START:
                        addDatasetsText(info.ToString());
                        counter = 0;
                        break;
                    case microsensys.MPC.Events.ConversionEventInfo.COMPLETED:
                        addDatasetsText(info.ToString());
                        addDatasetsText(String.Format("Datasets received: {0}", counter));
                        string text = "";
                        foreach (microsensys.MPC.Contents.Dataset datas in m_datasetList)
                        {
                            text += datas.ToString();
                        }
                        addDatasetsText(text);
                        break;
                    case microsensys.MPC.Events.ConversionEventInfo.FAILED:
                        addDatasetsText(info.ToString());
                        addDatasetsText(String.Format("Datasets received: {0}", counter));
                        string textds = "";
                        foreach (microsensys.MPC.Contents.Dataset datas in m_datasetList)
                        {
                            textds += datas.ToString();
                        }
                        addDatasetsText(textds);
                        break;
                }
            }
            catch { }
            
        }

        void m_Comm_ReaderInfoEvent(object sender, microsensys.MPC.Events.ReaderInfoEventArgs e, microsensys.MPC.Events.MemoryEventArgs me)
        {
            try
            {
                //InvokeRequired gibt es bei WPF nicht mehr
                if (!Dispatcher.Thread.Equals(Thread.CurrentThread))
                {
                    Dispatcher.Invoke(new microsensys.MPC.Communication.ReaderInformationEventHandler(m_Comm_ReaderInfoEvent), sender, e, me);
                    return;
                }
                //addText(e.Type.ToString());
                addText("READER FOUND");
                addText(e.ToString());
                addText(me.ToString());
                if (!button_READ.IsEnabled) button_READ.IsEnabled = true;
                if (!button_Delete.IsEnabled) button_Delete.IsEnabled = true;
            }
            catch { }
        }
        #endregion

        public void addText(string _text)
        {
            try
            {
                if (!Dispatcher.Thread.Equals(Thread.CurrentThread))
                {
                    Dispatcher.Invoke(new AddTextToTextBox(addText), _text);
                    return;
                }
                textBox_Log.Text += _text + "\n";
                textBox_Log.ScrollToEnd();
            }
            catch { }
        }
        public void addDatasetsText(string _text)
        {
            try
            {
                if (!Dispatcher.Thread.Equals(Thread.CurrentThread))
                {
                    Dispatcher.Invoke(new AddTextToTextBox(addDatasetsText), _text);
                    return;
                }
                textBox_Datasets.Text += _text + "\n";
                textBox_Datasets.ScrollToEnd();
            }
            catch { }
        }

        private void button_READ_Click(object sender, RoutedEventArgs e)
        {
            textBox_Datasets.Text = "";
            if (m_Comm != null)
            {
                m_datasetList.Clear();
                m_Comm.StartReadContents();
            }
        }

        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (m_Comm != null)
            {
                if (m_Comm.OpenCommunication()) addText("OPEN OK");
                else addText("OPEN FAILED");
            }
        }

        private void checkBox_ReadNew_Checked(object sender, RoutedEventArgs e)
        {
            m_Comm.ReadOnlyNewDatasets = (sender as CheckBox).IsChecked.Value;
        }

        private void checkBox_ResetPointers_Checked(object sender, RoutedEventArgs e)
        {
            m_Comm.ResetPointersAfterRead = (sender as CheckBox).IsChecked.Value;
        }

        private void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            textBox_Datasets.Text = "";
            if (m_Comm != null)
            {
                m_Comm.StartDeleteUsedMemory();
            }
        }

        private void button_Reset_Click(object sender, RoutedEventArgs e)
        {
            textBox_Datasets.Text = "";
            if (m_Comm != null)
            {
                m_Comm.StartResetMPCMemory();
            }
        }
    }
}
