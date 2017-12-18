using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using MessagePassingComm;
using Navigator;

namespace Building
{
    class ChildBuilder
    {
        Comm comm;
        Thread msgHandlerThread;
        Thread builderThread;

        delegate void Handle(CommMessage msg);
        Dictionary<string, Handle> dispatcher;
        Dictionary<string, string> addresses;

        public string workingDirectory { get; }
        public int port { get; }
        public bool open { get; private set; }
        public string errorMessage { get; }


        public ChildBuilder(int port)
        {
            this.port = port;
            comm = new Comm("http://localhost", port);
            open = comm.open;
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
                ServerEnvironment.root = workingDirectory;
                ClientEnvironment.root = workingDirectory;

                msgHandlerThread = new Thread(msgHandlerProc);
                msgHandlerThread.Start();
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
        }

        private void processAddressMsg(CommMessage msg)
        {
            string addressName = msg.arguments[0];
            string addressValue = msg.arguments[1];
            addresses[addressName] = addressValue;
        }

        private void processBuildRequestMsg(CommMessage msg)
        {

        }

        private void processFileReceivedMsg(CommMessage msg)
        {

        }

        private void processShutdownMsg(CommMessage msg)
        {
            open = false;
            CommMessage closingMsg = new CommMessage(CommMessage.MessageType.request);
            closingMsg.to = msg.to;
            closingMsg.from = msg.from;
            closingMsg.command = "SelfClosing";
            comm.postMessage(closingMsg);
            msgHandlerThread.Join();
            comm.close();
            // TODO delete the working directory
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
                }
                else
                {
                    Console.Write("\n  Unrecognized message command: {0}", msg.command);
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
        }
    }
}
