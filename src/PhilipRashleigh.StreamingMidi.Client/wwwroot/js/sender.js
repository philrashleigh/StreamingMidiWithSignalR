const MidiSender = (() => {
    let connection = null;
    
    let onMidiMessage = (message, subject) => {
        //Stream midi data to SingalR Hub
        subject.next(message.data.toString());
    };
    
    let subscribeInputs = (midiData, subject) => {
        //Loop through all midi-controllers available and tie them to onMidiMessage event
        for (let input of midiData.inputs.values()) {
            input.onmidimessage = function(message) {
                onMidiMessage(message, subject);
            };
        }
    };
    
    let initAsync = async url => {
        let connectAsync = async function () {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(url, {logger: signalR.LogLevel.Information})
                .withAutomaticReconnect()
                .build();

            await connection.start();

            console.log("Connected to SignalR Hub");          
        };        
        
        await connectAsync();

        let subject = new signalR.Subject();
        await connection.send("Send", subject);
                
        let midiData = await navigator.requestMIDIAccess({ 
            //Include software devices
            sysex: true
        });
        
        subscribeInputs(midiData, subject);
    };
            
    return {
        initAsync: initAsync
    };
})();