const MidiReceiver = (() => {
    let connection = null;
    let synth = null;
    
    let initSynth = () => {
        synth = new Tone.PolySynth(4, Tone.FMSynth).toMaster();
    };
    
    let midiToNoteName = (midiValue) => {
        const chromatic = [ 'C', 'Db', 'D', 'Eb', 'E', 'F', 'F#', 'G', 'Ab', 'A', 'Bb', 'B' ];
        
        let name = chromatic[midiValue % 12];
        let oct = Math.floor(midiValue / 12) - 1;
        
        return name + oct;
    };
    
    let toggle = (command, midiNote) => {
        let note = midiToNoteName(midiNote);
        
        switch (command) {
            case 144:
                synth.triggerAttack(note);
                break;
            case 128:
                synth.triggerRelease(note);
                break;        
        }
    };
    
    let subscribeToStream = () => {
        connection.on("Receive", (message) => {
            let data = message.split(',').map(item => parseInt(item));
            let [command, midiNote, _] = data;
    
            command = command & 0xf0;
            
            toggle(command, midiNote);
        });
    };
    
    let initAsync = async url => {
        initSynth();
        
        let connectAsync = async function () {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(url, {logger: signalR.LogLevel.Information})
                .withAutomaticReconnect()
                .build();

            await connection.start();

            console.log("Connected to SignalR Hub");          
        };        
        
        await connectAsync();
        
        subscribeToStream();
    };
            
    return {
        initAsync: initAsync
    };
})();