//////////////////////////////////////////////////////////////////////////////
// RepositoryServer.cs - Demonstrate Repository Server operations           //
// ver 1.0                                                                  // 
// Author: Shiqi Zhang, szhang65@syr.edu                                    //
// Application: CSE681 Project 4-CsMessagePassingComm                       //
// Environment: C# console                                                  //
//////////////////////////////////////////////////////////////////////////////
/* 
* Package Operations: 
* =================== 
* Repository server stores source code and test drivers and send and receive requests
* 
* All other functions used in this package... 
* ------------------------------------------- 
* - initializeDispatcher
* - getFiles
* - sendFile
* - fileList
* - msgHandlerProc
* - dequeRequest
* - fileSenderProc
* 
* Required Files: 
* --------------- 
* Environment.cs, TestUtilities.cs IMessagePassingCommService.cs, 
* MessagePassingCommService.cs
* 
* Maintenance History: 
* -------------------- 
* ver 1.0 : 06 Dec 2017 
* - first release 
*  
*/
using MessagePassingComm;
using Navigator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace RepositoryServer
{
    using Command = String;

    class RepositoryServer
    {

        Comm comm = null;
        delegate void Handle ( CommMessage msg );
        Dictionary<string , Handle> dispatcher = null;

        public bool open { get; private set; }
        public string errorMessage { get; }

        Thread msgHandlerThread;
        Thread dequeHandlerThread;


        RepositoryServer ( )
        {

            Console.Title = "RepositoryServer";
            try
            {
                string baseAddress = "http://localhost";
                int port = 8081;
                comm = new Comm ( baseAddress , port );

                try
                {
                    // check initial environment
                    dispatcher = new Dictionary<Command , Handle> ( );

                    initializeDispatcher ( );

                    open = comm.open;

                    initializeDispatcher ( );



                    msgHandlerThread = new Thread ( msgHandlerProc );
                    msgHandlerThread.Start ( );


                    dequeHandlerThread = new Thread ( dequeRequest );
                    dequeHandlerThread.Start ( );

                    Console.Write ( "\n Successfully create Repository server on {0}:{1}" , baseAddress , port );

                }
                catch ( Exception ex )
                {
                    //errorMessage = "Could not initialize Comm on the specified port (" + port.ToString ( ) + ").";

                    Console.Write ( "\n  {0}" , ex.Message );
                    GC.SuppressFinalize ( this );
                    System.Diagnostics.Process.GetCurrentProcess ( ).Close ( );
                }
                

            }
            catch ( Exception ex )
            {
                //errorMessage = "Could not initialize Comm on the specified port (" + port.ToString ( ) + ").";

                Console.Write ( "\n  {0}" , ex.Message );
                GC.SuppressFinalize ( this );
                System.Diagnostics.Process.GetCurrentProcess ( ).Close ( );
            }
        }

        ~RepositoryServer ( )
        {
            comm.postMessage ( new CommMessage ( CommMessage.MessageType.closeSender ) );
            comm.postMessage ( new CommMessage ( CommMessage.MessageType.closeReceiver ) );

            comm.close ( );
        }

        private void initializeDispatcher ( )
        {
            dispatcher[ "getFiles" ] = getFiles;
            dispatcher[ "sendFile" ] = sendFile;
            dispatcher[ "fileList" ] = fileList;
            dispatcher[ "pullRequestFiles" ] = pullRequestFiles;
            
        }

        private void getFiles ( CommMessage msg )
        {

            //CommMessage childBuilder = new CommMessage ( CommMessage.MessageType.request);
            //childBuilder.to = msg.from;
            //childBuilder.from = msg.to;

        }

        private void sendFile( CommMessage msg )
        {
            try
            {
                foreach ( string fileName in msg.arguments )
                {

                    comm.postFile ( fileName );
                    //msg.
                    Console.Write ( "\n Susscess send file in {0}" , ClientEnvironment.root );
                }
            }
            catch(Exception ex)
            {
                Console.Write ( "\n {0}" , ex.Message );
            }

        }

        private void fileList ( CommMessage msg )
        {
            try
            {
                string repositoryExePath = RepositoryEnvironment.root;
                string absFileSpec = Path.GetFullPath ( repositoryExePath );
                
                string[ ] fileList = Directory.GetFiles ( absFileSpec );

                //return message to client
                CommMessage returnMsg = new CommMessage ( CommMessage.MessageType.reply );
                returnMsg.to = msg.from;
                returnMsg.from = msg.to;
                returnMsg.author = "RepositoryServer";
                returnMsg.command = msg.command;
                returnMsg.argument = "Succeed get file list . ";
                
                foreach ( string fileName in fileList )
                {
                    returnMsg.arguments.Add ( fileName );   
                }
                comm.postMessage ( returnMsg );

            }
            catch ( Exception ex )
            {
                Console.Write ( "\n {0}" , ex.Message );
            }

        }


        private void pullRequestFiles ( CommMessage msg )
        {
            try
            {
                foreach (string filename in msg.arguments)
                {
                    comm.postFile ( filename );
                }

                //return message to client
                CommMessage returnMsg = new CommMessage ( CommMessage.MessageType.reply );
                returnMsg.to = msg.from;
                returnMsg.from = msg.to;
                returnMsg.author = "pullRequestFiles";
                returnMsg.command = msg.command;
                returnMsg.argument = "Succeed pull file list to ServerEnvironment root. ";
                foreach ( string fileName in msg.arguments )
                {
                    returnMsg.arguments.Add ( fileName );
                }
                comm.postMessage ( returnMsg );

            }
            catch ( Exception ex )
            {
                Console.Write ( "\n {0}" , ex.Message );
            }

        }

        private void msgHandlerProc ( )
        {
            while ( open )
            {
                try
                {
                    CommMessage msg = comm.getMessage ( );
                    Console.Write ( "\n  Received  message : {0} from client: {1}" , msg.command , msg.author );

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

                try
                {
                }
                catch
                {

                }



            }
        }

        private void fileSenderProc ( )
        {
            while ( open )
            {
                //if (  )
                //{
                //    CommMessage

                //}
            }
        }





        static void Main ( string[ ] args )
        {
            //ClientEnvironment
            ClientEnvironment.verbose = true;
            TestUtilities.vbtitle ( "testing Project 2 (Build Server)" , '=' );

            // test repository server 
            RepositoryServer bServer = new RepositoryServer ( );


            try
            {


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
