const MidiSender = (() => {
    let connection = null;
    let midiData = null;
    let synth = null;
    let subject = null;

    let midiNoteToFrequency = note => Math.pow(2, ((note - 69) / 12)) * 440;

    let sendMidiToServer = (message) => {
        //Stream midi data to SingalR Hub
        subject.next(message.toString());
    };

    let playSynth = (message) => {
        let frequency = midiNoteToFrequency(message[1]);

        if (message[0] === 144 && message[2] > 0) {
            synth.playNote(frequency);
        }

        if (message[0] === 128 || message[2] === 0) {
            synth.stopNote(frequency);
        }
    };
    
    let subscribeInputs = (midiData) => {
        //Loop through all midi-controllers available and tie them to onMidiMessage event
        for (let input of midiData.inputs.values()) {
            input.onmidimessage = function(event) {
                sendMidiToServer(event.data);
                playSynth(event.data);
            };
        }
    };
    
    let initAsync = async url => {
        let connectSignalRAsync = async () => {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(url, {logger: signalR.LogLevel.Information})
                .withAutomaticReconnect()
                .build();

            await connection.start();
            
            console.log("Connected to SignalR Hub");
        };
        
        let initMidiAsync = async () => {
            midiData = await navigator.requestMIDIAccess({
                //Include software devices
                sysex: true
            });

            console.log("Midi initialised");
            
            synth = new Synth();
            console.log("Synth initialised");
        };
        
        await Promise.all([connectSignalRAsync(), initMidiAsync()]);
   };
    
    let startRecordingAsync = async () => {
        subject = new signalR.Subject();
        let name = document.getElementById('name').value;
        
        await connection.send("Send", name, subject);
        
        document.getElementById('start-button').style.color = 'darkred';
        document.getElementById('status').innerText = 'Recording...';
        
        subscribeInputs(midiData)
    };
    
    let stopRecordingAsync = async () => {
        if(subject) {
            subject.complete();
            subject = null;
        }

        document.getElementById('start-button').style.color = 'black';
        document.getElementById('status').innerText = '';
    };
            
    return {
        initAsync: initAsync,
        startRecordingAsync: startRecordingAsync,
        stopRecordingAsync: stopRecordingAsync
    };
})();