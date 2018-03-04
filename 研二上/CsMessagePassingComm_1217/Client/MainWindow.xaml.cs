using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MessagePassingComm;


namespace Client
{
    using Command = String;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Comm comm = null;
        Thread receiverThread = null;

        delegate void Handle ( CommMessage msg );
        Dictionary<string , Handle> dispatcher;

        public bool open { get; private set; }

        public MainWindow()
        {





            Console.Title = "Client";
            InitializeComponent ();



            
        }

        ~MainWindow ( )
        {
            comm.close ( );
        }

        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            try
            {
                int port = 8082;
                comm = new Comm ( "http://localhost" , port );
                open = comm.open;

                if ( open )
                {
                    dispatcher = new Dictionary<Command , Handle> ( );

                    receiverThread = new Thread ( receiveProc );
                    receiverThread.Start ( );
                }



                Thread.Sleep ( 1000 );
                //execute  text executive to demonstrate all requirements
                TestExecutive exec = new TestExecutive ( this );


                Thread.Sleep ( 1000 );
                Console.Write ( "\n\n\n execute test demostrate finish ...... " );


                Console.Write ( "\n\n\n\n Succesfully create client Comm. \n" );


                

            }
            catch
            {
                Console.Write( "\n Can not create client Comm." );
            }
            
        }

        private void receiveProc()
        {
            while ( open )
            {
                try
                {
                    CommMessage msg = comm.getMessage ( );
                    Console.Write ( "\n  Received  message : {0} from client: {1}" , msg.command , msg.author );

                    if ( msg.command == "startProcessPool" || msg.command == "shutProcessAction" || msg.command == "pullRequestFiles" )
                    {
                        Console.Write ( "\n  Received message from '{0}'. Command: {1} , Status: {2}" , msg.from , msg.command , msg.argument );

                    }

                    if ( msg.command == "fileList" )
                    {
                        //try
                        //{
                        //    fileListBox.Items.Clear ( );
                        //    foreach ( string file in msg.arguments )
                        //    {
                        //        fileListBox.Items.Add ( file );
                        //    }
                        //}
                        //catch( Exception ex)
                        //{
                        //    Console.Write ( "\n  {0}" , ex.Message );
                        //}
                        this.fileListBox.Dispatcher.Invoke ( new Action( delegate {

                            try
                            {
                                this.fileListBox.Items.Clear ( );
                                foreach ( string file in msg.arguments )
                                {
                                    this.fileListBox.Items.Add ( System.IO.Path.GetFileName ( file) );
                                }
                            }
                            catch ( Exception ex )
                            {
                                Console.Write ( "\n  {0}" , ex.Message );
                            }

                        } ) );
                        
                    }

                }
                catch
                {

                }
            }
            
        }

        private void Window_Closing ( object sender , System.ComponentModel.CancelEventArgs e )
        {

        }

        private void btnStart_Click ( object sender , RoutedEventArgs e )
        {
            try
            {
                Console.Write ( "\n staring process pool....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "startProcessPool";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8080/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";
                msg.argument = tbxProcessNum.Text;

                comm.postMessage ( msg );
            }
            catch
            {
                Console.Write ( "\n can not start process pool" );
            }
        }

        private void btnShutdown_Click ( object sender , RoutedEventArgs e )
        {
            try
            {
                Console.Write ( "\n shutdown process pool....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "shutProcessAction";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8080/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";

                comm.postMessage ( msg );
            }
            catch
            {
                Console.Write ( "\n can not shutdown process pool" );
            }
        }

        private void btnShowFiles_Click ( object sender , RoutedEventArgs e )
        {
            try
            {
                Console.Write ( "\n get repository file list....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "fileList";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8081/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";

                comm.postMessage ( msg );
            }
            catch
            {
                Console.Write ( "\n can not shutdown process pool" );
            }
        }

        private void btnSendRequest_Click ( object sender , RoutedEventArgs e )
        {

            //List<string> fileNameList = this.fileListBox.SelectedItems as List<string>;
            List<string> fileNameList = new List<string> ( );
            for ( int i=0 ; i<this.fileListBox.SelectedItems.Count ; i++ )
            {
                fileNameList.Add ( this.fileListBox.SelectedItems[ i ].ToString() );
            }


            if ( fileNameList.Count >0 )
            {
                Console.Write ( "\n get selected file list....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "pullRequestFiles";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8081/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";
                msg.arguments = fileNameList;
                comm.postMessage ( msg );
            }
            else
            {
                Console.Write ( "\n hava not selected any file " );
            }
            


        }

        private void btnBuildDll_Click ( object sender , RoutedEventArgs e )
        {

        }


        //test executive
        public void textStartProcessPool ( )
        {
            Console.Write ( "\n staring process pool....." );

            CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
            msg.command = "startProcessPool";
            msg.author = "ClientGUI";
            msg.to = "http://localhost:8080/IMessagePassingComm";
            msg.from = "http://localhost:8082/IMessagePassingComm";
            int processPoolNum = 3;
            msg.argument = processPoolNum.ToString() ;

            comm.postMessage ( msg );


            //Thread.Sleep ( 6000);
        }
        public void textShutDownProcessPool ( )
        {
            Console.Write ( "\n shutdown process pool....." );

            CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
            msg.command = "shutProcessAction";
            msg.author = "ClientGUI";
            msg.to = "http://localhost:8080/IMessagePassingComm";
            msg.from = "http://localhost:8082/IMessagePassingComm";

            comm.postMessage ( msg );
        }

        public void textShowFileLists (  )
        {
            try
            {
                Console.Write ( "\n get repository file list....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "fileList";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8081/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";

                comm.postMessage ( msg );
            }
            catch
            {
                Console.Write ( "\n can not shutdown process pool" );
            }
        }



        public void testChildBuilder_printMessasge ( )
        {
            try
            {
                Console.Write ( "\n test childbuilder process pool : write the message contents to their consoles....." );

                for( int i=0 ; i<20 ; i++ )
                {
                    CommMessage msg = new CommMessage ( CommMessage.MessageType.reply );
                    msg.command = "printConsole";
                    msg.author = "ClientGUI index :  " + i;
                    msg.argument = " write this message contents to cuildbuilder console to demostrate their function  " + i;
                    msg.to = "http://localhost:8080/IMessagePassingComm";
                    msg.from = "http://localhost:8082/IMessagePassingComm";

                    comm.postMessage ( msg );

                    //Thread.Sleep ( 100 );

                }

            }
            catch
            {
                Console.Write ( "\n can not shutdown process pool" );
            }
        }



        public void textPullRequestFiles ( )
        {
            //List<string> fileNameList = this.fileListBox.SelectedItems as List<string>;
            //List<string> fileNameList = new List<string> ( );
            //for ( int i = 0 ; i < this.fileListBox.SelectedItems.Count ; i++ )
            //{
            //    fileNameList.Add ( this.fileListBox.SelectedItems[ i ].ToString ( ) );
            //}



            List<string> fileNameList = new List<string> ( );

            fileNameList.Add ( "App.xaml" );
            fileNameList.Add ( "CodePopUp.xaml" );

            if ( fileNameList.Count > 0 )
            {
                Console.Write ( "\n get selected file list....." );

                CommMessage msg = new CommMessage ( CommMessage.MessageType.request );
                msg.command = "pullRequestFiles";
                msg.author = "ClientGUI";
                msg.to = "http://localhost:8081/IMessagePassingComm";
                msg.from = "http://localhost:8082/IMessagePassingComm";
                msg.arguments = fileNameList;
                comm.postMessage ( msg );
            }
        }

    }
}
