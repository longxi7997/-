/////////////////////////////////////////////////////////////////////
//  TestExecutive.cs - demonstrate project 4 requirements          //
//  ver 1.0                                                        //
//  Language:      Visual C++ 2017                                 //        
//  Application:   Used to perform code publisher                  //
//  Author:        Shiqi Zhang, szhang65@syr.edu                   //
//                                                                 //
/////////////////////////////////////////////////////////////////////
/*
Package Operations:
==================
This package defines class TestCodePub, that demonstrates each of
the requirements in project #4 met.

Public Interface:
=================
TestExecutive exec = new TestExecutive();  // create an instance
exec.DemoReq(window);                      // demonstrate all requirement

Build Process:
==============
Required files
- TestExecutive.cs
- MainWindow.xaml.cs

Maintenance History:
====================
ver 1.0 : 6 Dec 2017
- first release

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MessagePassingComm;
using Navigator;
using System.Threading;

namespace Client
{
    class TestExecutive
    {
        
        public TestExecutive ( MainWindow wnd  )
        {
            ClientEnvironment.verbose = true;
            TestUtilities.vbtitle ( "TestExecutive" , '=' );


            DemoReq1 ( );
            DemoReq2 ( );
            DemoReq3 ( );
            DemoReq6 ( );
            //DemoReq5 ( wnd );

            DemoChildBuilder ( wnd );

        }




        //----< Demonstrate requirement #1 >-----------------------------------

        private void DemoReq1 ( )
        {
            /*----< #1 Requirement  >-----------------------------------*/
            TestUtilities.title("#1 Requirement ---- Using of C#, .Net Framework and Visual Studio 2017", '=');

            Console.Write("\n  Check the file extension and folder structure to see we are using C# \n");

            /*----< #2 Requirement  >-----------------------------------*/
            TestUtilities.title("#2 Requirement ---- Using of Message-Passing Communication Service built with WCF", '=');

            Console.Write("\n  Check that we are including and utilize Environment, IMPCommService, MPCommService packages.  \n");

            /*----< #3 Requirement  >-----------------------------------*/
            TestUtilities.title("#3 Requirement ---- Providing a process pool component", '=');

            Console.Write("\n  Check that we have implemented a process pool in BuildServer package. \n");

            /*----< #4 Requirement  >-----------------------------------*/
            TestUtilities.title("#4 Requirement ---- Providing a Repository server with several functions", '=');
        }


        private void DemoReq2 ( )
        {
            /*----< #5 Requirement  process pool component >-----------------------------------*/
            TestUtilities.title("#5 Requirement ---- The process pool component provides a specified number of processes on command", '=');

            Console.Write("\n  Check the prompt windows for a specified number of processes that we have created using GUI. \n");

            /*----< #6 Requirement  process pool component >-----------------------------------*/
            TestUtilities.title("#6 Requirement ---- Pool Processes shall use message-passing communication to access messages from the mother Builder process.", '=');
        }
        //----< Demonstrate requirement #5 >-------------------------------
        private void DemoReq3 ( )
        {
     

            /*----< #8 Requirement  process pool component >-----------------------------------*/
            TestUtilities.title("#8 Requirement ---- Upon building succeeds, send a test request and libraries to the Test Harness for execution, and send build log to the repository.", '=');

            Console.Write("\n  We have implemneted the log part, did not realize test harness, thus not able to send logs. \n");

            /*----< #9 Requirement  test harness >-----------------------------------*/
            // request 9 The Test Harness shall attempt to load each test library it receives and execute it.It shall submit the results of testing to the Repository.
            // 
            TestUtilities.title("#9 Requirement ---- Test Harness shall attempt to load each test library it receives and execute it and submit the results to the Repository.", '=');

            Console.Write("\n Not able to use TestHarness to execute tests. \n");

            // request 10 : Shall include a Graphical User Interface, built using WPF
            TestUtilities.title("#10 Requirement ---- Including a Graphical User Interface, built using WPF", '=');

            Console.Write("\n  Check the prompt windows for the GUI build using WPF. \n");


            // request 11 :The GUI client shall be a separate process, implemented with WPF and using message-passing communication. It shall provide mechanisms to 
            // get file lists from the Repository, and select files for packaging into a test library1, e.g., a test element specifying driver and tested files, 
            // added to a build request structure. It shall provide the capability of repeating that process to add other test libraries to the build request structure.
          
        }
        //----< Demonstrate requirement #10-13 >-----------------------------------

        private void DemoReq6 ( )
        {
            TestUtilities.title("#11 Requirement ---- The GUI being a separate process built using WPF and using Message-passing communication", '=');

            Console.Write("\n  Play with the prompt GUI to see its functions. \n");

            // request 12 : the client shall send build request structures to the repository for storage and transmission to the Build Server.
            TestUtilities.title("#12 Requirement ---- The client could send build requests to repository for storage and transmission", '=');

            Console.Write("\n  Click the corresponding button on the GUI to realize this function. \n");

            // request 13 : The client shall be able to request the repository to send a build request in its storage to the Build Server for build processing.
            TestUtilities.title("#13 Requirement ---- The GUI being able to request the repository to send a build request", '=');

            Console.Write("\n  Play with the prompt GUI to see its functions. \n");
        }


        private void DemoReq5 ( MainWindow wnd )
        {
            /*----< #7 Requirement  process pool component >-----------------------------------*/
            TestUtilities.title("#7 Requirement ---- Each Pool Process shall attempt to build each library, cited in a retrieved build request, logging warnings and errors.", '=');

            Console.Write("\n  We are able to build libraries, but did not do the serialization part. \n");

            Console.Write ( "\n  Retrieving file list from Repo Server...\n" );
            wnd.textStartProcessPool ( );



            Thread.Sleep ( 2000 );

            wnd.textShutDownProcessPool ( );

            wnd.textShowFileLists ( );

            wnd.textPullRequestFiles ( );

            Console.Write ( "\n  Sending build request:" );
            Console.Write ( "\n  Client => Repo Server => Mother Builder => Child Builder(if ready)" );
            Console.Write ( "\n  6 requests sent to demonstrate the queue feature." );
            Console.Write ( "\n  Please check whether Child Builder printed out the build request.\n" );

            Console.Write ( "\n  The Child Builder shall request files from Repo Server then." );
            Console.Write ( "\n  You may check the files in Child Storage path.\n" );

            Console.Write ( "\n  The Child Builder will sleep for 3 seconds to mock building process." );
            Console.Write ( "\n  The Child Builder will send a Ready Message to Mother when finish building.\n" );
        }

        private void DemoChildBuilder ( MainWindow wnd )
        {
            /*----< #7 Requirement  process pool component >-----------------------------------*/
            TestUtilities.title ( "#7 Requirement ---- Each Pool Process shall attempt to build each library, cited in a retrieved build request, logging warnings and errors." , '=' );

            Console.Write ( "\n  We are able to build libraries, but did not do the serialization part. \n" );

            Console.Write ( "\n  Retrieving file list from Repo Server...\n" );
            wnd.textStartProcessPool ( );



            //Thread.Sleep ( 2000 );


            wnd.testChildBuilder_printMessasge ( );



        }
    }
}
