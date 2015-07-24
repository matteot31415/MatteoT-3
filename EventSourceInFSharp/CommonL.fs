module CommonL

open System
open System.IO
open System.Linq
open System.Diagnostics.Tracing

type ConsoleEventListener(tw : TextWriter) =
    inherit EventListener()
    
    /// <summary>
    /// Override this method to get a list of all the eventSources that exist.  
    /// </summary>
    override this.OnEventSourceCreated(eventSource : EventSource) =
        // Because we want to turn on every EventSource, we subscribe to a callback that triggers
        // when new EventSources are created.  It is also fired when the EventListner is created
        // for all pre-existing EventSources.  Thus this callback get called once for every 
        // EventSource regardless of the order of EventSource and EventListener creation.  

        // For any EventSource we learn about, turn it on.   
        this.EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All)

        /// <summary>
        /// We override this method to get a callback on every event we subscribed to with EnableEvents
        /// </summary>
        /// <param name="eventData"></param>
    override this.OnEventWritten(eventData : EventWrittenEventArgs) =
        if not (eventData.EventName.StartsWith("ResourceManager")) then
            // report all event information
            tw.Write("  Event {0} ", eventData.EventName)

            // Events can have formatting strings 'the Message property on the 'Event' attribute.  
            // If the event has a formatted message, print that, otherwise print out argument values.  
            if eventData.Message <> null then
                tw.WriteLine(eventData.Message, if eventData.Payload <> null then eventData.Payload.ToArray() else null)
            else
                let sargs = 
                    if eventData.Payload <> null then 
                        eventData.Payload |> Seq.map (fun o -> o.ToString()) |> Seq.toArray
                    else
                        [||]
                tw.WriteLine("({0}).", String.Join(", ", sargs))

    static member ShortGuid(guid : Guid) = guid.ToString().Substring(0, 8)
