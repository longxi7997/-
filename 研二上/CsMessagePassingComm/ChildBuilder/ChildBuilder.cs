//////////////////////////////////////////////////////////////////////////////
// ChildBuilder.cs - Demonstrate Child Builder operations                   //
// ver 1.0                                                                  //
//                                                                          //
// Author: Shiqi Zhang, szhang65@syr.edu, Ammar Salman                      //
// Application: CSE681 Project 4-CsMessagePassingComm                       //
// Environment: C# console                                                  //
//////////////////////////////////////////////////////////////////////////////
/* 
* Package Operations: 
* =================== 
* This package uses WCF for communication. Make sure to run as Administrator
* becaue the communication service requires it. 
* 
* private methods: 
* ---------------- 
* - ChildBuilder            - create a working directory where the files can be processed
* - initializeDispatcher    - initialize the actions according to the message requests
* - processAddressMsg       - assign addresses for each message
* - processBuildRequestMsg  - enqueue a build request to the buildrequests queue
* - processFileReceivedMsg  - add filenames to the file list
* - processShutdownMsg      - quit the processes after having done all the builds
* - builderProc             - perform the entire building process
* - msgHandlerProc          - continuously accept message requests and invoke commands
* 
* Required Files: 
* --------------- 
* - MPCommService.cs, Environment.cs          
*
* Build Command:
* --------------
* CSharpCodeProvider 
*  
* Maintenance History: 
* -------------------- 
* ver 1.0 : 06 Dec 2017 
* - first release 
*  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using MessagePassingComm;
using Navigator;

using SWTools;

namespace Building
{
    class ChildBuilder
    {
        Comm comm;
        Thread msgHandlerThread;
        Thread builderThread;

        int index;

        delegate void Handle(CommMessage msg);
        Dictionary<string, Handle> dispatcher;
        Dictionary<string, string> addresses;


        BlockingQueue<CommMessage> buildRequests;
        List<string> receivedFiles;

        public string workingDirectory { get; }
        public string address { get; }
        public int port { get; }
        public bool open { get; private set; }
        public string errorMessage { get; }


        public ChildBuilder(int port)
        {

            //port = 8090 + port;
            Console.Title = "Child Builder on port:" + port ;

            Console.Write ( "\n " );
            this.port = port;
            comm = new Comm("http://localhost", port);
            open = comm.open;


            index = port - 8090 - 1;

            

            dispatcher = new Dictionary<string , Handle>();
            if (open)
            {
                workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "builder_" + port.ToString());
                Directory.CreateDirectory(workingDirectory);
                if (!Directory.Exists(workingDirectory))
                {
                    comm.close();
                    open = false;
                    errorMessage = "Could not create working directory ('" + workingDirectory + ").";
                    return;
                }
                address = "http://localhost:" + port.ToString();

                ServerEnvironment.root = workingDirectory;
                ClientEnvironment.root = workingDirectory;


                initializeDispatcher ( );

                msgHandlerThread = new Thread(msgHandlerProc);
                msgHandlerThread.Start();

                
                builderThread = new Thread(builderProc);
                builderThread.Start();


                Console.Write ( "address: {1}  ; index : {0} ; open : {2}" , index , address  , open);

            }
            else
            {
                errorMessage = "Could not initialize Comm on the specified port (" + port.ToString() + ").";
            }
        }

        private void initializeDispatcher()
        {
            dispatcher["Address"] = processAddressMsg;
            dispatcher["BuildRequest"] = processBuildRequestMsg;
            dispatcher["FileReceived"] = processFileReceivedMsg;
            dispatcher["Shutdown"] = processShutdownMsg;

            dispatcher[ "printConsole" ] = childBuilderPrintConsole;
        }

        private void processAddressMsg(CommMessage msg)
        {
            string addressName = msg.arguments[0];
            string addressValue = msg.arguments[1];
            addresses[addressName] = addressValue;
        }

        private void processBuildRequestMsg(CommMessage msg)
        {
            if (!addresses.ContainsKey("Repository"))
            {
                Console.Write("\n  Cannot find the repository address.");
                Console.Write("\n  Could not process build request.");
                return;
            }

            buildRequests.enQ(msg);
        }

        private void processFileReceivedMsg(CommMessage msg)
        {
            receivedFiles.Add(msg.arguments[0]);
        }
        
    
        private void processShutdownMsg(CommMessage msg)
        {
            open = false;
            CommMessage closingMsg = new CommMessage(CommMessage.MessageType.request);
            closingMsg.to = msg.to;
            closingMsg.from = msg.from;
            closingMsg.command = "SelfClosing";
            buildRequests.enQ(msg);
            builderThread.Join();
            comm.close();
            // TODO delete the working directory
        }



        private void childBuilderPrintConsole ( CommMessage msg )
        {
            Console.Write ( msg.argument );
            Console.Write ( "\n" );

            CommMessage sucessMsg = new CommMessage(CommMessage.MessageType.request);
            sucessMsg.to = msg.from;
            sucessMsg.from = msg.to;

            //Console.Write ( " \n index :{0} \n" ,  index );
            sucessMsg.argument = index.ToString();
            sucessMsg.command = "ChildSuccess";

            comm.postMessage ( sucessMsg );

        }
        private void builderProc()
        {
            while (open)
            {
                try
                {
                    CommMessage request = buildRequests.deQ ( );
                    if ( request.command == "SelfClosing" )
                        return;

                    receivedFiles.Clear ( );

                    CommMessage reqMsg = new CommMessage ( CommMessage.MessageType.request );
                    reqMsg.from = address;
                    reqMsg.to = addresses[ "Repository" ];
                    reqMsg.command = "GetFiles";
                    reqMsg.arguments = request.arguments;

                    comm.postMessage ( reqMsg );

                    while ( receivedFiles.Count != request.arguments.Count )
                        Thread.Sleep ( 100 );

                    // perform the build
                    string dllname = request.author;
                    DllBuilder builder = new DllBuilder ( receivedFiles , dllname );
                    builder.Build ( );
                    string log = builder.Log;


                    // PREPARE message to repository about the log
                    CommMessage logMsg = new CommMessage ( CommMessage.MessageType.reply );
                    logMsg.command = "logMsg";
                    logMsg.author = "ChildBuilder : " + address;
                    logMsg.to = addresses[ "Repository" ];
                    logMsg.from = address;
                    logMsg.argument = log;

                    comm.postMessage ( logMsg );


                    // SEND THE DLL TO TEST HARNESS TO GET TESTED
                    CommMessage dllFile = new CommMessage ( CommMessage.MessageType.reply );
                    logMsg.command = "dllFileBild";
                    logMsg.author = "ChildBuilder : " + address;
                    logMsg.to = ServerEnvironment.endPoint;
                    logMsg.from = address;
                    logMsg.argument = log;

                    comm.postMessage ( logMsg );


                    // DELETE ALL FILES IN WORKING DIRECTORY TO ALLOW FUTURE REQUESTS TO PROCESS
                    // WITHOUT PROBLEMS.
                    DeleteFolder ( workingDirectory );


                }
                catch
                {

                }



            }
        }

        private void msgHandlerProc()
        {
            Console.Write("\n  Starting message handler thread");
            while (open)
            {
                CommMessage msg = comm.getMessage();
                Console.Write("\n  Received message from '{0}'. Command: {1}", msg.from, msg.command);
                if (dispatcher.ContainsKey(msg.command))
                {
                    dispatcher[msg.command].Invoke(msg);

                    Console.Write ( msg.argument );
                }
                else
                {
                    Console.Write("\n  Unrecognized message command: {0}", msg.command);
                }
            }
        }


        // clear workingDirectory folder 
        public static void DeleteFolder ( string dir )
        {
            foreach ( string d in Directory.GetFileSystemEntries ( dir ) )
            {
                if ( File.Exists ( d ) )
                {
                    //delete file in workingDirectory folder 
                    FileInfo fi = new FileInfo ( d );
                    if ( fi.Attributes.ToString ( ).IndexOf ( "ReadOnly" ) != -1 )
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete ( d );
                }
                else
                {
                    // delete files in sub folder 
                    DirectoryInfo d1 = new DirectoryInfo ( d );
                    if ( d1.GetFiles ( ).Length != 0 )
                    {
                        DeleteFolder ( d1.FullName );
                    }
                    Directory.Delete ( d );
                }
            }
        }

        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Write("\n  Please provide one command line argument specifying the port number to start");
                Console.Write("\n  Terminating...\n\n");
                return;
            }

            Console.Write("\n  Starting ChildBuilder on port: {0}", args[0]);

            int port = 0;
            int.TryParse(args[0], out port);

            if(port == 0)
            {
                Console.Write("\n  Invalid port address: '{0}'. \n  Terminating...\n\n", args[0]);
                return;
            }

            ChildBuilder builder = new ChildBuilder(port);
            if (!builder.open)
            {
                Console.Write("\n  Could not initialize ChildBuilder.");
                Console.Write("\n  Details: {0}\n\n", builder.errorMessage);
                return;
            }

            Console.Write("\n  ChildBuilder initialized on port {0}", port);
            Console.Write("\n  ChildBuilder working directory: {0}", builder.workingDirectory);

            //Console.ReadKey();
        }
    }
}
