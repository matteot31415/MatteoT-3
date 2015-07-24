module AllSamples

open System
open System.IO
open System.Diagnostics
open System.Diagnostics.Tracing
open CommonL

module M =

    // Give your event sources a descriptive name using the EventSourceAttribute, otherwise the name of the class is used. 
    [<EventSource(Name = "Samples-EventSourceDemos-Minimal-3")>]
    // Class must be sealed or abstract
    [<Sealed>]
    type MinimalEventSource private () =    // reminder: private makes the ctor internal
        inherit EventSource()

        // define the singleton instance of the event source
        static let log = lazy new MinimalEventSource()

        static member Log = log.Value

        /// <summary>
        /// Call this method to notify listeners of a Load event for image 'imageName'
        /// </summary>
        /// <param name="baseAddress">The base address where the image was loaded</param>
        /// <param name="imageName">The image name</param>
        member this.Load(baseAddress : Int64 , imageName : string) =
            // Notes:
            //   1. the event ID passed to WriteEvent (1) corresponds to the (implied) event ID
            //      assigned to this method. The event ID could have been explicitly declared
            //      using an EventAttribute for this method
            //   2. the arguments passed to Load are forwarded in the same order to the 
            //      WriteEvent overload called below.
            this.WriteEvent(1, baseAddress, imageName);

    [<EventData>]
    type Payload = { Name : string; Age : int}

    let Run() = 
        use eventListener = new ConsoleEventListener(Console.Out)

        Console.Out.WriteLine("******************** MinimalEventSource Demo ********************")
        Console.Out.WriteLine("Sending 3 'Load' events from the Samples-EventSourceDemos-Minimal-3 source.")

        // Just send out three 'Load' events, with different arguments.
        MinimalEventSource.Log.Load(0x40000L, "MyFile0")
        MinimalEventSource.Log.Load(0x80000L, "MyFile1")
        MinimalEventSource.Log.Load(0xc0000L, "MyFile2")

        // Dynamic 
        let x = new EventSource("Samples-EventSourceDemos-Minimal-2")
        x.Write("Event#1", { Name = "Matteo"; Age = 46 });
        x.Write("Event#2", { Name = "Pippo"; Age = 21 });

        Console.Out.WriteLine("Done.");

M.Run()

//module M =
//    let x = 10
//
//type AllSamples() =
//    static member Out = Console.Out
//
//    static member Run() =
//        use eventListener = new ConsoleEventListener()
//
//        AllSamples.Out.WriteLine("******************** MinimalEventSource Demo ********************")
//        AllSamples.Out.WriteLine("Sending 3 'Load' events from the Samples-EventSourceDemos-Minimal source.")
//
//        // Just send out three 'Load' events, with different arguments.
//        MinimalEventSource.Log.Load(0x40000, "MyFile0");
//        MinimalEventSource.Log.Load(0x80000, "MyFile1");
//        MinimalEventSource.Log.Load(0xc0000, "MyFile2");
//
//        AllSamples.Out.WriteLine("Done.");

//        MinimalEventSourceDemo.Run(); Debugger.Break();         // Break between demos, Hit F5 to continue. 
//        DynamicEventSourceDemo.Run(); Debugger.Break();
//        CustomizedEventSourceDemo.Run(); Debugger.Break();
//        EventLogEventSourceDemo.Run(); Debugger.Break();
//        LocalizedEventSourceDemo.Run(); Debugger.Break();
