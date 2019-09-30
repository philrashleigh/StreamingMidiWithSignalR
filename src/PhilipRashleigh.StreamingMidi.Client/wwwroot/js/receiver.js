const MidiReceiver = (() => {
    let connection = null;
    let synth = null;

    let midiNoteToFrequency = note => Math.pow(2, ((note - 69) / 12)) * 440;

    let playSynth = (message) => {
        let frequency = midiNoteToFrequency(message[1]);

        if (message[0] === 144 && message[2] > 0) {
            synth.playNote(frequency);
        }

        if (message[0] === 128 || message[2] === 0) {
            synth.stopNote(frequency);
        }
    };
    
    let playRecordingAsync = async (name, listItem) => {
        listItem.className = "list-group-item list-group-item-success";
        
        connection.stream("Receive", name)
            .subscribe({
                next: (item) => {
                    let message = item.split(",").map(item => parseInt(item));
                    playSynth(message);
                },
                complete: () => {
                    console.log("Playback finished");

                    listItem.className = "list-group-item";
                },
                error: (err) => {
                    console.log("Error streaming recording: " + err);
                },
            });
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
        
        let populateRecordingsAsync = async () => {
            let recordings = await connection.invoke("GetRecordings");
            let recordingList = document.getElementById("recording-list");

            recordings.forEach(recording => {
                let listItem = document.createElement('li');
                listItem.className = "list-group-item";
                listItem.innerText = recording;
                
                listItem.addEventListener('click', event => {
                    event.preventDefault();

                    playRecordingAsync(recording, listItem);
                });
                
                recordingList.appendChild(listItem);
            });
        };

        synth = new Synth();
        console.log("Synth initialised");
        
        await connectSignalRAsync();
        await populateRecordingsAsync();
    };
    
    return {
        initAsync: initAsync,
    };
})();