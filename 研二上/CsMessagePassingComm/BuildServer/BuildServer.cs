//////////////////////////////////////////////////////////////////////////////
// Buildserver.cs - Demonstrate Build Server operations                     //
// ver 1.0                                                                  //
//                                                                          //
// Author: Shiqi Zhang, szhang65@syr.edu                                    //
// Application: CSE681 Project 4-CsMessagePassingComm                       //
// Environment: C# console                                                  //
//////////////////////////////////////////////////////////////////////////////
/* 
* Package Operations: 
* =================== 
* BuildServer is the mother builder of the system. It creates and manages a 
* process pool, sending and receiving messages and file requests.
* 
* private methods: 
* ---------------- 
* - SingleProcessInfo    - initialize the properties of a single process
* - initializeDispatcher - initialize the actions according to the message requests
* - createProcessPool    - create a specified number of process pool according to command
* - requestAction        - enqueue requests
* - shutdownProcessPool  - shutdown process pool when needed
* - msgHandlerProc       - keep listening to messages
* - dequeRequest         - dequeue a request from both the readyQ and requestQ
*  
* Required Files: 
* --------------- 
* Environment.cs, TestHarness.cs TestUtilities.cs IMessagePassingCommService.cs, 
* MessagePassingCommService.cs
*
* Build Command:
* --------------
*  
*  
* Maintenance History: 
* -------------------- 
* ver 1.0 : 06 Dec 2017 
* - first release 
*  
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessagePassingComm;
using Navigator;
using System.IO;
using TestHarness;
using System.Threading;

namespace Building
{
    /*----< define Command as the key in the Dictionary for messageDispatching >--------------*/
    using Command = String;   
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    //manage eveary process' info in process pool
    public class SingleProcessInfo
    {
        public Process process { set; get; }
        public int processUID { set; get; }
        public string adress { set; get; }
        public bool isReady { set; get; }

        public SingleProcessInfo( Process proc, int procIndex )
        {
            process = proc;
            processUID = procIndex;
            //cuildBuilder port  from 8090 + indexer 
            //adress = "http://localhost:" + ( 8090 + processUID ) + "/IMessagePassingComm";
            adress = "http://localhost:" + processUID + "/IMessagePassingComm";
            isReady = true;
        }
    }

    class BuildServer
    {


        Comm comm = null;
        //MessageDispatcher msgDispatcher = null;


        delegate void Handle ( CommMessage msg );
        Dictionary<string , Handle> dispatcher = null;

        List<SingleProcessInfo> allProcessinPool = null;
        SWTools.BlockingQueue<RequestInfo> requestedQ = null;  //  manage request 
        SWTools.BlockingQueue<int> readQ = null;     // record ready process 

        Thread msgHandlerThread;
        Thread builderThread;

        // List<>  manage process list 
        int maxProcessNum { get; set; }
        public bool open { get; private set; }
        public string errorMessage { get; }


        /*----< constructor >------------------------------------------------------*/
        BuildServer()
        {
            Console.Title = "Build Server";

            // start comm
            if ( comm == null )
            {
                string baseAddress = "http://localhost";
                int port = 8080;
                comm = new Comm ( baseAddress , port );
            }
                
            
            try
            {
                // check initial environment
                dispatcher = new Dictionary<Command , Handle> ( );
                maxProcessNum = 10;

                allProcessinPool = new List<SingleProcessInfo> ( );
                requestedQ = new SWTools.BlockingQueue<RequestInfo> ( );
                readQ = new SWTools.BlockingQueue<int> ( );

                initializeDispatcher ( );

                open = comm.open;



                msgHandlerThread = new Thread ( msgHandlerProc );
                msgHandlerThread.Start ( );


                builderThread = new Thread ( dequeRequest );
                builderThread.Start ( );

                Console.Write ( "\n Successfully create build server...");

            }
            catch( Exception ex)
            {
                //errorMessage = "Could not initialize Comm on the specified port (" + port.ToString ( ) + ").";

                Console.Write ( "\n  {0}" , ex.Message );
                GC.SuppressFinalize ( this );
                System.Diagnostics.Process.GetCurrentProcess ( ).Close ( );
            }

        }
        /*----< close server >----------------------------------------------------*/
        ~BuildServer()
        {

            readQ.clear ( );
            requestedQ.clear ( );
            allProcessinPool.Clear ( );

            comm.postMessage(new CommMessage(CommMessage.MessageType.closeSender));
            comm.postMessage(new CommMessage(CommMessage.MessageType.closeReceiver));

            comm.close ( );
        }

        private void initializeDispatcher ( )
        {
            dispatcher[ "startProcessPool" ] = createProcessPool;
            dispatcher[ "request" ] = requestAction;
            dispatcher[ "shutProcessAction" ] = shutdownProcessPool;
            dispatcher[ "printConsole" ] = childBuilderPrintConsole;

            dispatcher[ "ChildSuccess" ] = ChildBuilderRecover;
            
        }

        private void createProcessPool ( CommMessage msg )
        {

            if ( allProcessinPool.Count == 0)
            {
                string childBuilderExePath = "..\\..\\..\\ChildBuilder\\bin\\Debug\\ChildBuilder.exe";
                string absFileSpec = Path.GetFullPath ( childBuilderExePath );

                int processNum = 0;
                if ( int.TryParse ( msg.argument , out processNum ) )
                {
                    if ( processNum <= 0 || processNum > maxProcessNum )
                        processNum = maxProcessNum;
                }

                for ( int i = 0 ; i < processNum ; i++ )
                {
                    try
                    {
                        int processUID = i+1 + 8090;
                        Process proc = Process.Start ( absFileSpec , processUID.ToString ( ) );
                        SingleProcessInfo processInfo = new SingleProcessInfo ( proc , processUID );

                        allProcessinPool.Add ( processInfo );
                        readQ.enQ ( i );

                        Console.Write ( "\n  create ChildBuilder Process : {0} on id {1}" , processUID , proc.Id );

                    }
                    catch ( Exception ex )
                    {
                        Console.Write ( "\n  {0}" , ex.Message );
                    }
                }


                //return message to client
                CommMessage returnMsg = new CommMessage ( CommMessage.MessageType.reply );
                returnMsg.to = msg.from;
                returnMsg.from = msg.to;
                returnMsg.author = "BuildServer";
                returnMsg.command = msg.command;
                returnMsg.argument = "Succeed create process pool . ";
                comm.postMessage ( returnMsg );
            }
            else
            {
                Console.Write ( "\n process pool has exited , please shutdown first." );
            }
            
            
        }

        private void requestAction ( CommMessage msg )
        {

            RequestInfo request = new RequestInfo ( msg );

            requestedQ.enQ ( request );

            //Console.Write ( "\n  Inserted request in Requested Queue , Index : {0}" ,  requestedQ.size() );
            Console.Write ( "\n  Inserted request in Requested Queue. "  );




        }


        private void childBuilderPrintConsole ( CommMessage msg )
        {
            requestAction ( msg );


            //dequeRequest ( );

        }

        private void ChildBuilderRecover( CommMessage msg )
        {

            Console.Write ( "\n ................. from : {0}  ; address : {1}  " , msg.from , msg.argument );

            int childBuilderIndex = int.Parse(msg.argument);
            readQ.enQ ( childBuilderIndex );
            allProcessinPool[ childBuilderIndex ].isReady = true;

        }

        private void shutdownProcessPool ( CommMessage msg )
        {
            //open = false;
            for( int i=0 ;i<allProcessinPool.Count ;i++ )
            {
                try
                {
                    int proIndex = allProcessinPool[ i ].processUID;
                    int procId = allProcessinPool[ i ].process.Id;
                    allProcessinPool[ i ].process.Kill ( );
                    //allProcessinPool[ i ].process.CloseMainWindow ( );
                    //allProcessinPool[ i ].process.Close ( );
                    //allProcessinPool[ i ].process.Dispose ( );
                    Console.Write ( "\n  shutdown ChildBuilder Process : {0} on id {1}" , proIndex , procId );
                }
                catch ( Exception ex )
                {
                    Console.Write ( "\n {0}" , ex.Message );
                }
            }

            readQ.clear ( );
            requestedQ.clear ( );
            allProcessinPool.Clear ( );


            //return message to client
            CommMessage returnMsg = new CommMessage ( CommMessage.MessageType.reply );
            returnMsg.to = msg.from;
            returnMsg.from = msg.to;
            returnMsg.command = msg.command;
            returnMsg.author = "BuildServer";
            returnMsg.argument = "Succeed shutdown process pool . ";
            comm.postMessage ( returnMsg );

        }


        private void msgHandlerProc ( )
        {
            while ( open )
            {
                try
                {
                    CommMessage msg = comm.getMessage ( );
                    Console.Write ( "\n  Received  message : {0} from client: {1}" , msg.command  , msg.author);

                    if ( dispatcher.ContainsKey ( msg.command ) )
                    {
                        dispatcher[ msg.command ].Invoke ( msg );
                    }
                    else
                    {
                        Console.Write ( "\n  Unrecognized message command: {0}" , msg.command );
                    }

                    
                }
                catch
                {

                }
            }
        }

        private void dequeRequest ( )
        {
            while ( open )
            {

                if( requestedQ.size() > 0 && readQ.size()>0 )
                {
                    try
                    {
                        RequestInfo request = requestedQ.deQ ( );
                        int childBuilderIndex = readQ.deQ ( );
                        allProcessinPool[ childBuilderIndex ].isReady = false;



                        Console.Write ( "\n Deque a ready Child Builder({0}) for your request \n " , childBuilderIndex );

                        CommMessage childMsg = new CommMessage ( CommMessage.MessageType.request );
                        childMsg.to = allProcessinPool[ childBuilderIndex ].adress;
                        childMsg.from = "http://localhost:8080/IMessagePassingComm";
                        //childMsg.argument = request.requestStr;
                        childMsg.argument = request.msg.argument;
                        //childMsg.command = "BuildRequest";
                        childMsg.command = request.msg.command;


                        comm.postMessage ( childMsg );

                        //Thread.Sleep ( 100 );
                        //readQ.enQ ( childBuilderIndex );
                        //allProcessinPool[ childBuilderIndex ].isReady = true;


                    }
                    catch
                    {

                    }
                }
               



            }
        }


        static void Main(string[] args)
        {

            //ClientEnvironment
            ClientEnvironment.verbose = true;
            TestUtilities.vbtitle ( "testing Project 2 (Build Server)" , '=' );

            // test Prject 2 : builder server 
            BuildServer bServer = new BuildServer ( );
            


            //CommMessage commMsg = bServer.comm.getMessage ( );


            try
            {
                //bServer.startBuildServer ( );


                // create process pool 
                //bServer.msgDispatcher.excuteCommand ( "StarProcessPool" , commMsg );
                //bServer.createProcessPool (10 );
                // invoke process 

                //shutdown process pooll 



            }
            catch ( InvalidCastException e )
            {
                Console.WriteLine ( e.Source );
            }




            //Console.WriteLine ( "\nPress key to quit\n" );
            //Console.ReadKey ( );

         

        }
    }
}
